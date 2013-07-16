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

namespace Sarcasm.Unparsing
{
    public class Formatter
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

            public Params(BlockIndentation blockIndentation)
            {
                this.blockIndentation = blockIndentation;
            }
        }

        private delegate InsertedUtokens GetUtokensBeforeAfter(UnparsableObject target);

        #endregion

        #region Default values

        private static readonly string newLineDefault = Environment.NewLine;
        private const string spaceDefault = " ";
        private const string tabDefault = "\t";
        private static readonly string indentUnitDefault = string.Concat(Enumerable.Repeat(spaceDefault, 4));
        private const string whiteSpaceBetweenUtokensDefault = spaceDefault;
        private const bool indentEmptyLinesDefault = false;
        private static readonly IFormatProvider formatProviderDefault = CultureInfo.InvariantCulture;

        #endregion

        #region State

        #region Immutable after initialization

        public string NewLine { get; set; }
        public string Space { get; set; }
        public string Tab { get; set; }
        public string IndentUnit { get; set; }
        public string WhiteSpaceBetweenUtokens { get; set; }
        public bool IndentEmptyLines { get; set; }
        public IFormatProvider FormatProvider { get; private set; }

        #endregion

        #region Mutable

        private UnparsableObject topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        private UnparsableObject leftTerminalLeaveCache = UnparsableObject.NonCalculated;
        private Unparser.Direction direction;

        #endregion

        #endregion

        #region Construction

        public Formatter()
            : this(formatProviderDefault)
        {
        }

        public Formatter(Grammar grammar)
            : this(grammar.DefaultCulture)
        {
        }

        public Formatter(Parser parser)
            : this(parser.Context.Culture)
        {
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

            this.direction = that.direction;
        }

        internal Formatter Spawn(ChildLocation childLocation = ChildLocation.Unknown)
        {
            return Spawn(child: null, childLocation: childLocation);
        }

        internal Formatter Spawn(UnparsableObject child, ChildLocation childLocation = ChildLocation.Unknown)
        {
            if (childLocation == ChildLocation.Unknown)
            {
                return new Formatter(this)
                {
                    topAncestorCacheForLeft = UnparsableObject.NonCalculated
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
                        : (child ?? UnparsableObject.NonCalculated)
                };
            }
        }

        #endregion

        #region Interface to grammar

        public CultureInfo CultureInfo { get { return FormatProvider as CultureInfo; } }

        public void SetFormatProviderIndependentlyFromParser(IFormatProvider formatProvider)
        {
            this.FormatProvider = formatProvider;

        }

        public void SetCultureInfoIndependentlyFromParser(CultureInfo cultureInfo)
        {
            SetFormatProviderIndependentlyFromParser(cultureInfo);
        }

        public void SetCultureInfo(CultureInfo cultureInfo, Parser parser)
        {
            SetCultureInfoIndependentlyFromParser(cultureInfo);
            parser.Context.Culture = cultureInfo;
        }

        #endregion

        #region Virtuals

        protected virtual InsertedUtokens GetUtokensLeft(UnparsableObject target)
        {
            return InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensRight(UnparsableObject target)
        {
            return InsertedUtokens.None;
        }

        protected virtual InsertedUtokens GetUtokensBetween(UnparsableObject leftTerminalLeaveTarget, UnparsableObject rightTarget)
        {
            return InsertedUtokens.None;
        }

        protected virtual BlockIndentation GetBlockIndentation(UnparsableObject leftTerminalLeaveIfAny, UnparsableObject target)
        {
            return BlockIndentation.IndentNotNeeded;
        }

        protected virtual IDecoration GetDecoration(UnparsableObject target)
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
                    topAncestorCacheForLeft = UnparsableObject.NonCalculated;

                this.direction = value;
            }
        }

        /// <summary>
        /// This method needs to be fully executed before UnparseRawMiddle because this method modifies the state of Unparser and,
        /// which state is used by UnparseRawMiddle. Thus, always call this method prior to UnparseRawMiddle.
        /// </summary>
        internal IReadOnlyList<UtokenBase> YieldBefore(UnparsableObject self, out Params @params)
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

            @params = new Params(blockIndentation);

            GetUtokensBeforeAfter getUtokensBefore = direction == Unparser.Direction.LeftToRight
                ? (GetUtokensBeforeAfter)_GetUtokensLeft
                : (GetUtokensBeforeAfter)_GetUtokensRight;

            InsertedUtokens insertedUtokensBefore = getUtokensBefore(self);

            if (insertedUtokensBefore != InsertedUtokens.None)
            {
                Unparser.tsUnparse.Debug("inserted utokens: {0}", insertedUtokensBefore);
                utokens.Add(insertedUtokensBefore);
            }

            return utokens;
        }

        internal IEnumerable<UtokenBase> YieldAfter(UnparsableObject self, Params @params)
        {
            Unparser.tsUnparse.Debug("YieldAfter");

            GetUtokensBeforeAfter getUtokensAfter = direction == Unparser.Direction.LeftToRight
                ? (GetUtokensBeforeAfter)_GetUtokensRight
                : (GetUtokensBeforeAfter)_GetUtokensLeft;

            InsertedUtokens insertedUtokensAfter = getUtokensAfter(self);

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

        internal static IEnumerable<Utoken> PostProcess(IEnumerable<UtokenBase> utokens, IPostProcessHelper postProcessHelper)
        {
            return utokens                                      .DebugWriteLines(tsRaw)
                .CalculateDeferredUtokens(postProcessHelper)    .DebugWriteLines(tsCalculatedDeferred)
                .FilterInsertedUtokens(postProcessHelper)       .DebugWriteLines(tsFiltered)
                .Flatten()                                      .DebugWriteLines(tsFlattened)
                .ProcessControls(postProcessHelper)             .DebugWriteLines(tsProcessedControls)
                .Decorate(postProcessHelper)
                .Cast<Utoken>()
                ;
        }

        internal void ResetMutableState()
        {
            topAncestorCacheForLeft = UnparsableObject.NonCalculated;
        }

        #endregion

        #region Yield logic for indentation and between

        private void UpdateCacheOnTheFly(UnparsableObject self, State state)
        {
            if (state == State.Before)
            {
                if (self.SyntaxParent == null)
                    topAncestorCacheForLeft = null;     // self is root node
                else if (self.IsLeftSiblingCalculated && self.LeftSibling != null)
                    topAncestorCacheForLeft = self;
                else
                    topAncestorCacheForLeft = UnparsableObject.NonCalculated;
            }
            else if (state == State.After)
            {
                if (direction == Unparser.Direction.LeftToRight && self.BnfTerm is Terminal)
                    leftTerminalLeaveCache = self;
                else if (direction == Unparser.Direction.RightToLeft && self == topAncestorCacheForLeft)
                    leftTerminalLeaveCache = UnparsableObject.NonCalculated;
            }
        }

        private IReadOnlyList<UtokenBase> YieldBetween(UnparsableObject self)
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

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldBetween(UnparsableObject self, Formatter formatter)
        {
            // NOTE: topAncestorCacheForLeft may get updated by GetUsedLeftsFromTopToBottomB

            UnparsableObject leftObject = GetLeftTerminalLeave(self);

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

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableObject self, BlockIndentation blockIndentation)
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

        private IReadOnlyList<UtokenBase> YieldIndentationLeft(UnparsableObject self, ref BlockIndentation blockIndentation)
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

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableObject self, BlockIndentation blockIndentation)
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

        private IEnumerable<UtokenBase> YieldIndentationRight(UnparsableObject self, ref BlockIndentation blockIndentation)
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

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableObject self, BlockIndentation blockIndentationParameter, Unparser.Direction direction,
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

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated or thrown out.
        /// </exception>
        private static IEnumerable<UtokenBase> _YieldIndentation(UnparsableObject self, ref BlockIndentation blockIndentationParameter, Unparser.Direction direction,
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
                UnparsableObject leftObject = GetLeftTerminalLeave(self);
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

        private InsertedUtokens _GetUtokensLeft(UnparsableObject target)
        {
            return GetUtokensLeft(target)
                .SetKind(InsertedUtokens.Kind.Left)
                .SetAffected(target);
        }

        private InsertedUtokens _GetUtokensRight(UnparsableObject target)
        {
            return GetUtokensRight(target)
                .SetKind(InsertedUtokens.Kind.Right)
                .SetAffected(target);
        }

        private InsertedUtokens _GetUtokensBetween(UnparsableObject leftTarget, UnparsableObject rightTarget)
        {
            return GetUtokensBetween(leftTarget, rightTarget)
                .SetKind(InsertedUtokens.Kind.Between)
                .SetAffected(leftTarget, rightTarget);
        }

        private BlockIndentation _GetBlockIndentation(UnparsableObject leftIfAny, UnparsableObject target)
        {
            return GetBlockIndentation(leftIfAny, target);
        }

        internal IDecoration _GetDecoration(UnparsableObject target)
        {
            return GetDecoration(target);
        }

        private static IEnumerable<UnparsableObject> GetSelfAndAncestors(UnparsableObject self)
        {
            return Util.RecurseStopBeforeNull(self, current => current.SyntaxParent);
        }

        private static IEnumerable<BnfTerm> GetSelfAndAncestorsB(UnparsableObject self)
        {
            return GetSelfAndAncestors(self).Select(current => current.BnfTerm);
        }

        private static UnparsableObject GetTopAncestorForLeft(UnparsableObject self, Formatter formatter = null)
        {
            // NOTE: topAncestorCacheForLeft will not be changed if we have no formatter, that's why we use topAncestorForLeft + the static vs. instance behavior
            UnparsableObject topAncestorForLeft;

            if (formatter == null || !UnparsableObject.IsCalculated(formatter.topAncestorCacheForLeft))
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

        private static UnparsableObject CalculateTopAncestorForLeft(UnparsableObject self)
        {
            return GetSelfAndAncestors(self).FirstOrDefault(current => current.LeftSibling != null);
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static IEnumerable<UnparsableObject> GetLeftsFromTopToBottom(UnparsableObject self, Formatter formatter = null)
        {
            UnparsableObject topAncestorForLeft = GetTopAncestorForLeft(self, formatter);

            return topAncestorForLeft != null
                ? Util.RecurseStopBeforeNull(topAncestorForLeft.LeftSibling, current => current.RightMostChild)
                : Enumerable.Empty<UnparsableObject>();
        }

        /// <exception cref="UnparsableObject.NonCalculatedException">
        /// If topLeft is non-calculated.
        /// </exception>
        private static UnparsableObject GetLeftTerminalLeave(UnparsableObject self, Formatter formatter = null)
        {
            // NOTE: topAncestorCacheForLeft will not be changed if we have no formatter, that's why we use topAncestorForLeft + the static vs. instance behavior
            UnparsableObject leftTerminalLeave;

            if (formatter == null || !UnparsableObject.IsCalculated(formatter.topAncestorCacheForLeft))
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

        private static UnparsableObject CalculateLeftTerminalLeave(UnparsableObject self, Formatter formatter)
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

        public static InsertedUtokens SetAffected(this InsertedUtokens insertedUtokens, params UnparsableObject[] affectedUnparsableObjects)
        {
            if (insertedUtokens != null)
                insertedUtokens.affectedUnparsableObjects_FOR_DEBUG = affectedUnparsableObjects;

            return insertedUtokens;
        }
    }
}
