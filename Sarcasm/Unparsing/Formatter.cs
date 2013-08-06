using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;
using Sarcasm.Utility;

using Grammar = Sarcasm.GrammarAst.Grammar;
using System.Globalization;
using Sarcasm.Parsing;
using Sarcasm.DomainCore;

namespace Sarcasm.Unparsing
{
    public interface ICommentFormatter : ICommentCleaner
    {
        string[] GetDecoratedCommentTextLines(UnparsableAst owner, Comment comment);
    }

    public class Formatter : ICommentFormatter
    {
        #region Tracing

        internal readonly static TraceSource tsRaw = new TraceSource("RAW", SourceLevels.Verbose);
        internal readonly static TraceSource tsCalculatedDeferred = new TraceSource("CALCULATED_DEFERRED", SourceLevels.Verbose);
        internal readonly static TraceSource tsCalculatedDeferredDetailed = new TraceSource("CALCULATED_DEFERRED_DETAILED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFiltered = new TraceSource("FILTERED", SourceLevels.Verbose);
        internal readonly static TraceSource tsFlattened = new TraceSource("FLATTENED", SourceLevels.Verbose);
        internal readonly static TraceSource tsProcessedControls = new TraceSource("PROCESSED_CONTROLS", SourceLevels.Verbose);

#if DEBUG
        static Formatter()
        {
            tsRaw.Listeners.Clear();
            tsRaw.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "01_raw.log"))));

            tsCalculatedDeferred.Listeners.Clear();
            tsCalculatedDeferred.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "02_calculated_deferred.log"))));

            tsCalculatedDeferredDetailed.Listeners.Clear();
            tsCalculatedDeferredDetailed.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "02_calculated_deferred_detailed.log"))));

            tsFiltered.Listeners.Clear();
            tsFiltered.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "03_filtered.log"))));

            tsFlattened.Listeners.Clear();
            tsFlattened.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "04_flattened.log"))));

            tsProcessedControls.Listeners.Clear();
            tsProcessedControls.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "05_processed_controls.log"))));
        }
#endif

        #endregion

        #region Types

        private enum State { Before, After }

        internal enum ChildLocation { Unknown, First, Middle, Last, Only }

        internal class Params
        {
            public readonly BlockIndentation blockIndentation;
            public readonly InsertedUtokens insertedUtokensAfter;

            public Params(BlockIndentation blockIndentation, InsertedUtokens insertedUtokensAfter)
            {
                this.blockIndentation = blockIndentation;
                this.insertedUtokensAfter = insertedUtokensAfter;
            }
        }

        private delegate InsertedUtokens GetUtokensBeforeAfter(UnparsableAst target);

        #endregion

        #region Default values

        private static readonly string newLineDefault = Environment.NewLine;
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private const bool indentEmptyLinesDefault = false;
        private const string multiLineCommentDecoratorDefault = "";
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        #endregion

        #region State

        #region Immutable after initialization

        #region Settings

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public bool IndentEmptyLines { get; set; }
        public string MultiLineCommentDecorator { get; set; }
        public IFormatProvider FormatProvider { get; private set; }

        #endregion

        private List<Parser> attachedParsers;

        #endregion

        #region Mutable

        private UnparsableAst topAncestorCacheForLeft = UnparsableAst.NonCalculated;
        private UnparsableAst leftTerminalLeaveCache = UnparsableAst.NonCalculated;
        private Unparser.Direction direction;

        #endregion

        #endregion

        #region Construction

        public Formatter(Grammar grammar)
            : this(grammar.DefaultCulture)
        {
            this.attachedParsers = new List<Parser>();
        }

        public Formatter(Parser parserToAttach)
            : this(parserToAttach.Context.Culture)
        {
            this.attachedParsers = new List<Parser>();
            AttachParser(parserToAttach);
        }

        public Formatter(MultiParser multiParserToAttach)
            : this(multiParserToAttach.GetMainParser().Context.Culture)
        {
            this.attachedParsers = new List<Parser>();
            AttachParser(multiParserToAttach);
        }

        protected Formatter(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

            this.NewLine = newLineDefault;
            this.Space = spaceDefault;
            this.Tab = tabDefault;
            this.IndentUnit = indentUnitDefault;
            this.WhiteSpaceBetweenUtokens = whiteSpaceBetweenUtokensDefault;
            this.IndentEmptyLines = indentEmptyLinesDefault;
            this.MultiLineCommentDecorator = multiLineCommentDecoratorDefault;
        }

        private Formatter(Formatter that)
        {
            this.NewLine = that.NewLine;
            this.Space = that.Space;
            this.Tab = that.Tab;
            this.IndentUnit = that.IndentUnit;
            this.WhiteSpaceBetweenUtokens = that.WhiteSpaceBetweenUtokens;
            this.IndentEmptyLines = that.IndentEmptyLines;
            this.FormatProvider = that.FormatProvider;
            this.MultiLineCommentDecorator = that.MultiLineCommentDecorator;

            this.attachedParsers = that.attachedParsers;

            this.direction = that.direction;
        }

        internal Formatter Spawn(ChildLocation childLocation = ChildLocation.Unknown)
        {
            return Spawn(child: null, childLocation: childLocation);
        }

        internal Formatter Spawn(UnparsableAst child, ChildLocation childLocation = ChildLocation.Unknown)
        {
            if (childLocation == ChildLocation.Unknown)
            {
                return new Formatter(this)
                {
                    topAncestorCacheForLeft = UnparsableAst.NonCalculated
                };
            }
            else
            {
                bool isLeftMostChild =
                    childLocation == ChildLocation.Only
                    ||
                    direction == Unparser.Direction.LeftToRight && childLocation == ChildLocation.First
                    ||
                    direction == Unparser.Direction.RightToLeft && childLocation == ChildLocation.Last;

                return new Formatter(this)
                {
                    topAncestorCacheForLeft = isLeftMostChild
                        ? this.topAncestorCacheForLeft
                        : (child ?? UnparsableAst.NonCalculated)
                };
            }
        }

        #endregion

        #region Interface to grammar

        public CultureInfo CultureInfo
        {
            get { return (CultureInfo)FormatProvider; }

            set
            {
                FormatProvider = value;

                foreach (Parser parser in attachedParsers)
                    parser.Context.Culture = value;
            }
        }

        public void AttachParser(MultiParser multiParserToAttach)
        {
            foreach (Parser parser in multiParserToAttach.GetParsers())
                AttachParser(parser);
        }

        public void AttachParser(Parser parserToAttach)
        {
            attachedParsers.Add(parserToAttach);
        }

        public void SetFormatProviderIndependentlyFromAttachedParsers(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;
        }

        #endregion

        #region Overridables for derived formatter

        protected virtual void GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
        {
            leftInsertedUtokens = InsertedUtokens.None;
            rightInsertedUtokens = InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensBetween(UnparsableAst leftTerminalLeaveTarget, UnparsableAst rightTarget)
        {
            return InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensBetweenCommentAndOwner(UnparsableAst owner, Comment comment, int commentIndex, int commentCount,
            IEnumerable<Comment> commentsBetweenThisAndOwner)
        {
            if (comment.LineIndexDistanceFromOwner > 0)
            {
                int newLinesCount;

                if (comment.Placement == CommentPlacement.OwnerLeft)
                {
                    newLinesCount = comment.LineIndexDistanceFromOwner - (comment.TextLines.Length - 1);

                    Comment nextComment = commentsBetweenThisAndOwner.FirstOrDefault();
                    if (nextComment != null)
                        newLinesCount -= nextComment.LineIndexDistanceFromOwner;
                }
                else
                {
                    newLinesCount = comment.LineIndexDistanceFromOwner;

                    Comment prevComment = commentsBetweenThisAndOwner.FirstOrDefault();
                    if (prevComment != null)
                        newLinesCount -= prevComment.LineIndexDistanceFromOwner + (prevComment.TextLines.Length - 1);
                }

                if (comment.Placement == CommentPlacement.OwnerLeft && comment.Kind == CommentKind.SingleLine)
                {
                    /*
                     * We have already yielded a NewLine in Unparser.UnparseComment (see comment there),
                     * thus we have to yield one less "formatting" NewLine here.
                     * */
                    newLinesCount--;
                }

                return new UtokenRepeat(UtokenWhitespace.NewLine(), newLinesCount);
            }
            else
                return UtokenWhitespace.Space();
        }

        internal protected virtual string[] GetDecoratedCommentTextLines(UnparsableAst owner, Comment comment)
        {
            if (comment.TextLines.Length > 1)
            {
                return comment.TextLines
                    .Select((textLine, lineIndex) => lineIndex > 0 && comment.IsDecorated ? MultiLineCommentDecorator + textLine : textLine)
                    .ToArray();
            }
            else
                return comment.TextLines;
        }

        protected virtual string[] GetCleanedUpCommentTextLines(string[] commentTextLines, int columnIndex, CommentTerminal commentTerminal, out bool isDecorated)
        {
            if (commentTextLines.Length > 1)
            {
                bool _isDecorated = false;

                commentTextLines = commentTextLines
                    .Select(
                        (commentTextLine, lineIndex) =>
                        {
                            string trimmedStartCommentTextLine = commentTextLine.TrimStart();
                            string trimmedStartMultiLineCommentDecorator = MultiLineCommentDecorator.TrimStart();
                            string trimmedMultiLineCommentDecorator = trimmedStartMultiLineCommentDecorator.TrimEnd();

                            if (trimmedStartCommentTextLine.StartsWith(trimmedStartMultiLineCommentDecorator))
                            {
                                _isDecorated = true;
                                return trimmedStartCommentTextLine.Remove(0, trimmedStartMultiLineCommentDecorator.Length);   // decorated
                            }
                            else if (trimmedStartCommentTextLine.StartsWith(trimmedMultiLineCommentDecorator))
                            {
                                _isDecorated = true;
                                return trimmedStartCommentTextLine.Remove(0, trimmedMultiLineCommentDecorator.Length);    // decorated with missing whitespaces at the right of decorator
                            }
                            else if (commentTextLine.Length >= columnIndex && commentTextLine.Take(columnIndex).All(ch => char.IsWhiteSpace(ch)))
                                return commentTextLine.Remove(0, columnIndex);    // undecorated -> remove indentation
                            else
                                return commentTextLine;   // undecorated -> could not remove indentation (the indentation of this line is smaller then the indentation of the start of the comment)
                        }
                    )
                    .ToArray();

                isDecorated = _isDecorated;
                return commentTextLines;
            }
            else
            {
                isDecorated = false;
                return commentTextLines;
            }
        }

        protected virtual BlockIndentation GetBlockIndentation(UnparsableAst leftTerminalLeaveIfAny, UnparsableAst target)
        {
            return BlockIndentation.IndentNotNeeded;
        }

        protected virtual IDecoration GetDecoration(Utoken utoken, UnparsableAst target)
        {
            return Decoration.None;
        }

        #endregion

        #region Interface to unparser

        internal Unparser.Direction Direction
        {
            set
            {
                if (direction != value)
                    topAncestorCacheForLeft = UnparsableAst.NonCalculated;

                this.direction = value;
            }
        }

        /// <summary>
        /// This method needs to be fully executed before UnparseRawMiddle because this method modifies the state of Unparser and,
        /// which state is used by UnparseRawMiddle. Thus, always call this method prior to UnparseRawMiddle.
        /// </summary>
        internal IReadOnlyList<UtokenBase> YieldBefore(UnparsableAst self, out Params @params)
        {
            /*
             * To achieve fully execution before UnparseRawMiddle, this method is not an iterator block rather populates a list.
             * Returning IReadOnlyList instead of IEnumerable is just an explicit guarantee to the caller to ensure that
             * the returned utokens does not need to be iterated through e.g. by converting it to a list in order to achieve full execution.
             * */

            Unparser.tsUnparse.Debug("YieldBefore");

            UpdateCacheOnTheFly(self, State.Before);

            var utokens = new List<UtokenBase>();
            BlockIndentation blockIndentation = BlockIndentation.ToBeSet;

            if (direction == Unparser.Direction.LeftToRight)
            {
                utokens.AddRange(YieldBetween(self));
                utokens.AddRange(YieldIndentationLeft(self, ref blockIndentation));
            }
            else
            {
                utokens.AddRange(YieldIndentationRight(self, ref blockIndentation));
            }

            InsertedUtokens leftInsertedUtokens;
            InsertedUtokens rightInsertedUtokens;

            _GetUtokensAround(self, out leftInsertedUtokens, out rightInsertedUtokens);

            InsertedUtokens insertedUtokensBefore;
            InsertedUtokens insertedUtokensAfter;

            if (direction == Unparser.Direction.LeftToRight)
            {
                insertedUtokensBefore = leftInsertedUtokens;
                insertedUtokensAfter = rightInsertedUtokens;
            }
            else
            {
                insertedUtokensBefore = rightInsertedUtokens;
                insertedUtokensAfter = leftInsertedUtokens;
            }

            if (insertedUtokensBefore != InsertedUtokens.None)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBefore);
                utokens.Add(insertedUtokensBefore);
            }

            @params = new Params(blockIndentation, insertedUtokensAfter);

            return utokens;
        }

        internal IEnumerable<UtokenBase> YieldAfter(UnparsableAst self, Params @params)
        {
            Unparser.tsUnparse.Debug("YieldAfter");

            InsertedUtokens insertedUtokensAfter = @params.insertedUtokensAfter;

            if (insertedUtokensAfter != InsertedUtokens.None)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensAfter);
                yield return insertedUtokensAfter;
            }

            BlockIndentation blockIndentation = @params.blockIndentation;

            if (direction == Unparser.Direction.LeftToRight)
            {
                foreach (UtokenBase utoken in YieldIndentationRight(self, blockIndentation))
                    yield return utoken;
            }
            else
            {
                foreach (UtokenBase utoken in YieldIndentationLeft(self, blockIndentation))
                    yield return utoken;

                foreach (UtokenBase utoken in YieldBetween(self))
                    yield return utoken;
            }

            UpdateCacheOnTheFly(self, State.After);
        }

        internal IDecoration GetDecoration(Utoken utoken)
        {
            UnparsableAst target = utoken is UtokenText
                ? ((UtokenText)utoken).Reference
                : null;

            return GetDecoration(utoken, target);
        }

        internal IEnumerable<UtokenBase> YieldBeforeComments(UnparsableAst owner, Comments comments, UnparseCommentDelegate unparseComment)
        {
            IReadOnlyList<Comment> beforeComments;
            IReadOnlyList<Comment> beforeCommentsForFormatter;
            Func<IEnumerable<Comment>, int, IEnumerable<Comment>> skipOrTake;

            if (direction == Unparser.Direction.LeftToRight)
            {
                beforeComments = comments.Left;
                beforeCommentsForFormatter = comments.Left;
                skipOrTake = Enumerable.Skip;
            }
            else
            {
                beforeComments = comments.Right.ReverseOptimized();
                beforeCommentsForFormatter = comments.Right;
                skipOrTake = Enumerable.Take;
            }

            foreach (var beforeComment in beforeComments.Select((beforeComment, commentIndex) => new { Value = beforeComment, Index = commentIndex }))
            {
                int index;
                int commentCountToSkipOrTake;

                if (direction == Unparser.Direction.LeftToRight)
                {
                    index = beforeComment.Index;
                    commentCountToSkipOrTake = index + 1;
                }
                else
                {
                    index = beforeComments.Count - 1 - beforeComment.Index;
                    commentCountToSkipOrTake = index;
                }

                foreach (UtokenBase utoken in unparseComment(owner, beforeComment.Value))
                    yield return utoken;

                yield return GetUtokensBetweenCommentAndOwner(owner, beforeComment.Value, index, beforeComments.Count, skipOrTake(beforeCommentsForFormatter, commentCountToSkipOrTake))
                    .SetKind(InsertedUtokens.Kind.Between)
                    .SetAffected(owner);
            }
        }

        internal IEnumerable<UtokenBase> YieldAfterComments(UnparsableAst owner, Comments comments, UnparseCommentDelegate unparseComment)
        {
            IReadOnlyList<Comment> afterComments;
            IReadOnlyList<Comment> afterCommentsForFormatter;
            Func<IEnumerable<Comment>, int, IEnumerable<Comment>> skipOrTake;

            if (direction == Unparser.Direction.LeftToRight)
            {
                afterComments = comments.Right;
                afterCommentsForFormatter = comments.Right;
                skipOrTake = Enumerable.Take;
            }
            else
            {
                afterComments = comments.Left.ReverseOptimized();
                afterCommentsForFormatter = comments.Left;
                skipOrTake = Enumerable.Skip;
            }

            foreach (var afterComment in afterComments.Select((beforeComment, commentIndex) => new { Value = beforeComment, Index = commentIndex }))
            {
                int index;
                int commentCountToSkipOrTake;

                if (direction == Unparser.Direction.LeftToRight)
                {
                    index = afterComment.Index;
                    commentCountToSkipOrTake = index;
                }
                else
                {
                    index = afterComments.Count - 1 - afterComment.Index;
                    commentCountToSkipOrTake = index + 1;
                }

                yield return GetUtokensBetweenCommentAndOwner(owner, afterComment.Value, index, afterComments.Count, skipOrTake(afterCommentsForFormatter, commentCountToSkipOrTake))
                    .SetKind(InsertedUtokens.Kind.Between)
                    .SetAffected(owner);

                foreach (UtokenBase utoken in unparseComment(owner, afterComment.Value))
                    yield return utoken;
            }
        }

        internal static IEnumerable<Utoken> PostProcess(IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return utokens                                      .DebugWriteLines(tsRaw)
                .CalculateDeferredUtokens(postProcessHelper)    .DebugWriteLines(tsCalculatedDeferred)
                .FilterInsertedUtokens(postProcessHelper)       .DebugWriteLines(tsFiltered)
                .Flatten()                                      .DebugWriteLines(tsFlattened)
                .ProcessControls(postProcessHelper)             .DebugWriteLines(tsProcessedControls)
                .Decorate(postProcessHelper)
                ;
        }

        internal void ResetMutableState()
        {
            topAncestorCacheForLeft = UnparsableAst.NonCalculated;
        }

        string[] ICommentFormatter.GetDecoratedCommentTextLines(UnparsableAst owner, Comment comment)
        {
            return GetDecoratedCommentTextLines(owner, comment);
        }

        #endregion

        #region Interface to parser

        string[] ICommentCleaner.GetCleanedUpCommentTextLines(string[] commentTextLines, int columnIndex, CommentTerminal commentTerminal, out bool isDecorated)
        {
            return GetCleanedUpCommentTextLines(commentTextLines, columnIndex, commentTerminal, out isDecorated);
        }

        #endregion

        #region Yield logic for indentation and between

        private void UpdateCacheOnTheFly(UnparsableAst self, State state)
        {
            if (state == State.Before)
            {
                if (self.SyntaxParent == null)
                    topAncestorCacheForLeft = null;     // self is root node
                else if (self.IsLeftSiblingCalculated && self.LeftSibling != null)
                    topAncestorCacheForLeft = self;
                else
                    topAncestorCacheForLeft = UnparsableAst.NonCalculated;
            }
            else if (state == State.After)
            {
                if (direction == Unparser.Direction.LeftToRight && self.BnfTerm is Terminal)
                    leftTerminalLeaveCache = self;
                else if (direction == Unparser.Direction.RightToLeft && self == topAncestorCacheForLeft)
                    leftTerminalLeaveCache = UnparsableAst.NonCalculated;
            }
        }

        private IReadOnlyList<UtokenBase> YieldBetween(UnparsableAst self)
        {
            Unparser.tsUnparse.Debug("YieldBetween");

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by _YieldBetween
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldBetween(self, formatter: this).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution

                return new[]
                {
                    new DeferredUtokens(
                        () => _YieldBetween(self, formatter: this),
                        self: self,
                        helpMessage: "YieldBetween"
                        )
                };
            }
        }

        /// <exception cref="UnparsableAst.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldBetween(UnparsableAst self, Formatter formatter)
        {
            // NOTE: topAncestorCacheForLeft may get updated by GetUsedLeftsFromTopToBottomB

            UnparsableAst leftObject = GetLeftTerminalLeave(self);

            if (leftObject != null)
            {
                InsertedUtokens insertedUtokensBetween = formatter._GetUtokensBetween(leftObject, self);

                if (insertedUtokensBetween != InsertedUtokens.None)
                {
                    Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBetween);
                    yield return insertedUtokensBetween;
                }
            }
        }

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableAst self, BlockIndentation blockIndentation)
        {
#if DEBUG
            BlockIndentation originalBlockIndentation = blockIndentation;
#endif
            var utokens = YieldIndentationLeft(self, ref blockIndentation);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentation, originalBlockIndentation), "unwanted change of blockIndentationParameter reference in YieldIndentationLeft");
#endif
            return utokens;
        }

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableAst self, ref BlockIndentation blockIndentation)
        {
            Unparser.tsUnparse.Debug("YieldIndentationLeft");

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by YieldIndentation
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldIndentation(self, ref blockIndentation, direction, formatter: this, left: true).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                Debug.Assert(blockIndentation == BlockIndentation.ToBeSet || blockIndentation.IsDeferred());

                if (blockIndentation == BlockIndentation.ToBeSet)
                    blockIndentation = BlockIndentation.Defer();

                Debug.Assert(blockIndentation.IsDeferred());

                BlockIndentation _blockIndentation = blockIndentation;

                return new[]
                {
                    blockIndentation.LeftDeferred = new DeferredUtokens(
                        () => _YieldIndentation(self, _blockIndentation, direction, formatter: this, left: true),
                        self: self,
                        helpMessage: "YieldIndentationLeft",
                        helpCalculatedObject: _blockIndentation
                        )
                };
            }
        }

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableAst self, BlockIndentation blockIndentation)
        {
#if DEBUG
            BlockIndentation originalBlockIndentation = blockIndentation;
#endif
            var utokens = YieldIndentationRight(self, ref blockIndentation);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentation, originalBlockIndentation), "unwanted change of blockIndentationParameter reference in YieldIndentationRight");
#endif
            return utokens;
        }

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableAst self, ref BlockIndentation blockIndentation)
        {
            Unparser.tsUnparse.Debug("YieldIndentationRight");

            try
            {
                // NOTE: topAncestorCacheForLeft may get updated by YieldIndentation
                // NOTE: ToList is needed to fully evaluate the called function, so we can catch the exception
                return _YieldIndentation(self, ref blockIndentation, direction, formatter: this, left: false).ToList();
            }
            catch (NonCalculatedException)
            {
                // top left node or someone in the chain is non-calculated -> defer execution
                Debug.Assert(blockIndentation == BlockIndentation.ToBeSet || blockIndentation.IsDeferred());

                if (blockIndentation == BlockIndentation.ToBeSet)
                    blockIndentation = BlockIndentation.Defer();

                Debug.Assert(blockIndentation.IsDeferred());

                BlockIndentation _blockIndentation = blockIndentation;

                return new[]
                {
                    blockIndentation.RightDeferred = new DeferredUtokens(
                        () => _YieldIndentation(self, _blockIndentation, direction, formatter: this, left: false),
                        self: self,
                        helpMessage: "YieldIndentationRight",
                        helpCalculatedObject: _blockIndentation
                        )
                };
            }
        }

        /// <exception cref="UnparsableAst.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableAst self, BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatter formatter, bool left)
        {
#if DEBUG
            BlockIndentation originalBlockIndentationParameter = blockIndentationParameter;
#endif
            var utokens = _YieldIndentation(self, ref blockIndentationParameter, direction, formatter, left);
#if DEBUG
            Debug.Assert(object.ReferenceEquals(blockIndentationParameter, originalBlockIndentationParameter),
                string.Format("unwanted change of blockIndentationParameter reference in _YieldIndentation ({0})", left ? "left" : "right"));
#endif
            return utokens;
        }

        /// <exception cref="UnparsableAst.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableAst self, ref BlockIndentation blockIndentationParameter, Unparser.Direction direction,
            Formatter formatter, bool left)
        {
            if (!left && direction == Unparser.Direction.RightToLeft && blockIndentationParameter.IsDeferred() && blockIndentationParameter.LeftDeferred != null)
            {
                /*
                 * We are in a right-to-left unparse and this deferred right indentation depends on a deferred left indentation,
                 * so let's try to calculate the deferred left indentation, which - if succeed - will change the shared blockIndentation
                 * object state from deferred to a valid indentation.
                 * 
                 * (If the left indentation wasn't deferred then it would be calculated which means that the shared blockIndentation
                 * object won't be deferred.)
                 * */
                blockIndentationParameter.LeftDeferred.CalculateUtokens();

                // either CalculateUtokens succeeded, and blockIndentation is not deferred, or CalculateUtokens threw a NonCalculatedException exception

                Debug.Assert(!blockIndentationParameter.IsDeferred(), "CalculateUtokens succeeded, but blockIndentation remained deferred");
            }

            if (blockIndentationParameter == BlockIndentation.ToBeSet || blockIndentationParameter.IsDeferred())
            {
                UnparsableAst leftObject = GetLeftTerminalLeave(self);
                BlockIndentation blockIndentation = formatter._GetBlockIndentation(leftObject, self);

                // NOTE: topAncestorCacheForLeft gets updated by GetUsedLeftsFromTopToBottomB

                if (blockIndentation != BlockIndentation.IndentNotNeeded)
                    Unparser.tsUnparse.Debug("blockindentation {0} for leftTarget '{1}' and for target '{2}'", blockIndentation, leftObject, self);

                Debug.Assert(!blockIndentation.IsDeferred());

                if (blockIndentationParameter == BlockIndentation.ToBeSet)
                    blockIndentationParameter = blockIndentation;
                else
                    blockIndentationParameter.CopyKindFrom(blockIndentation);
            }

            if (direction == Unparser.Direction.LeftToRight)
            {
                return left
                    ? BlockIndentationToUtokenControlBefore(blockIndentationParameter)
                    : BlockIndentationToUtokenControlAfter(blockIndentationParameter);
            }
            else
            {
                return left
                    ? BlockIndentationToUtokenControlAfter(blockIndentationParameter)
                    : BlockIndentationToUtokenControlBefore(blockIndentationParameter);
            }
        }

        private static IEnumerable<UtokenControl> BlockIndentationToUtokenControlBefore(BlockIndentation blockIndentation)
        {
            if (blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.IncreaseIndentLevel;

            else if (blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.DecreaseIndentLevel;

            else if (blockIndentation == BlockIndentation.ZeroIndent)
                yield return UtokenControl.SetIndentLevelToNone;
        }

        private static IEnumerable<UtokenControl> BlockIndentationToUtokenControlAfter(BlockIndentation blockIndentation)
        {
            if (blockIndentation == BlockIndentation.Indent)
                yield return UtokenControl.DecreaseIndentLevel;

            else if (blockIndentation == BlockIndentation.Unindent)
                yield return UtokenControl.IncreaseIndentLevel;

            else if (blockIndentation == BlockIndentation.ZeroIndent)
                yield return UtokenControl.RestoreIndentLevel;
        }

        #endregion

        #region Helpers

        private void _GetUtokensAround(UnparsableAst target, out InsertedUtokens leftInsertedUtokens, out InsertedUtokens rightInsertedUtokens)
        {
            GetUtokensAround(target, out leftInsertedUtokens, out rightInsertedUtokens);

            leftInsertedUtokens
                .SetKind(InsertedUtokens.Kind.Left)
                .SetAffected(target);

            rightInsertedUtokens
                .SetKind(InsertedUtokens.Kind.Right)
                .SetAffected(target);
        }

        private InsertedUtokens _GetUtokensBetween(UnparsableAst leftTarget, UnparsableAst rightTarget)
        {
            return GetUtokensBetween(leftTarget, rightTarget)
                .SetKind(InsertedUtokens.Kind.Between)
                .SetAffected(leftTarget, rightTarget);
        }

        private BlockIndentation _GetBlockIndentation(UnparsableAst leftIfAny, UnparsableAst target)
        {
            return GetBlockIndentation(leftIfAny, target);
        }

        private static IEnumerable<UnparsableAst> GetSelfAndAncestors(UnparsableAst self)
        {
            return Util.RecurseStopBeforeNull(self, current => current.SyntaxParent);
        }

        private static IEnumerable<BnfTerm> GetSelfAndAncestorsB(UnparsableAst self)
        {
            return GetSelfAndAncestors(self).Select(current => current.BnfTerm);
        }

        private static UnparsableAst GetTopAncestorForLeft(UnparsableAst self, Formatter formatter = null)
        {
            // NOTE: topAncestorCacheForLeft will not be changed if we have no formatter, that's why we use topAncestorForLeft + the static vs. instance behavior
            UnparsableAst topAncestorForLeft;

            if (formatter == null || !UnparsableAst.IsCalculated(formatter.topAncestorCacheForLeft))
            {
                topAncestorForLeft = CalculateTopAncestorForLeft(self);

                if (formatter != null)
                    formatter.topAncestorCacheForLeft = topAncestorForLeft;
            }
            else
            {
                topAncestorForLeft = formatter.topAncestorCacheForLeft;
                //Unparser.tsUnparse.Debug(formatter.topAncestorCacheForLeft != CalculateTopAncestorForLeft(self),
                //    "!!!!!!!! should be equal for {0}, but topAncestorCacheForLeft is '{1}' and calculated value is '{2}'", self, formatter.topAncestorCacheForLeft, CalculateTopAncestorForLeft(self));
                Debug.Assert(formatter.topAncestorCacheForLeft == CalculateTopAncestorForLeft(self));
            }

            return topAncestorForLeft;
        }

        private static UnparsableAst CalculateTopAncestorForLeft(UnparsableAst self)
        {
            return GetSelfAndAncestors(self).FirstOrDefault(current => current.LeftSibling != null);
        }

        /// <exception cref="UnparsableAst.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static IEnumerable<UnparsableAst> GetLeftsFromTopToBottom(UnparsableAst self, Formatter formatter = null)
        {
            UnparsableAst topAncestorForLeft = GetTopAncestorForLeft(self, formatter);

            return topAncestorForLeft != null
                ? Util.RecurseStopBeforeNull(topAncestorForLeft.LeftSibling, current => current.RightMostChild)
                : Enumerable.Empty<UnparsableAst>();
        }

        /// <exception cref="UnparsableAst.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static UnparsableAst GetLeftTerminalLeave(UnparsableAst self, Formatter formatter = null)
        {
            // NOTE: topAncestorCacheForLeft will not be changed if we have no formatter, that's why we use topAncestorForLeft + the static vs. instance behavior
            UnparsableAst leftTerminalLeave;

            if (formatter == null || !UnparsableAst.IsCalculated(formatter.topAncestorCacheForLeft))
            {
                leftTerminalLeave = CalculateLeftTerminalLeave(self, formatter);

                if (formatter != null)
                    formatter.leftTerminalLeaveCache = leftTerminalLeave;
            }
            else
            {
                leftTerminalLeave = formatter.leftTerminalLeaveCache;
                //Unparser.tsUnparse.Debug(formatter.leftTerminalLeaveCache != CalculateLeftTerminalLeave(self, formatter),
                //    "!!!!!!!! should be equal for {0}, but leftTerminalLeaveCache is '{1}' and calculated value is '{2}'", self, formatter.leftTerminalLeaveCache, CalculateLeftTerminalLeave(self, formatter));
                Debug.Assert(formatter.leftTerminalLeaveCache == CalculateLeftTerminalLeave(self, formatter));
            }

            return leftTerminalLeave;
        }

        private static UnparsableAst CalculateLeftTerminalLeave(UnparsableAst self, Formatter formatter)
        {
            return GetLeftsFromTopToBottom(self, formatter).LastOrDefault();
        }

        #endregion
    }

    internal static class InsertedUtokensExtensions
    {
        public static InsertedUtokens SetKind(this InsertedUtokens insertedUtokens, InsertedUtokens.Kind kind)
        {
            if (insertedUtokens != null)
                insertedUtokens.kind = kind;

            return insertedUtokens;
        }

        public static InsertedUtokens SetAffected(this InsertedUtokens insertedUtokens, params UnparsableAst[] affectedUnparsableAsts)
        {
            if (insertedUtokens != null)
                insertedUtokens.affectedUnparsableAsts_FOR_DEBUG = affectedUnparsableAsts;

            return insertedUtokens;
        }
    }
}
