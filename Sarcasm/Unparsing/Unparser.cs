using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Utility;
using Sarcasm.GrammarAst;

using Grammar = Sarcasm.GrammarAst.Grammar;
using Sarcasm.DomainCore;

namespace Sarcasm.Unparsing
{
    internal delegate IEnumerable<UtokenBase> UnparseCommentDelegate(UnparsableAst owner, Comment comment);

    public sealed class Unparser : IUnparser, IPostProcessHelper
    {
        #region Tracing

        internal const string logDirectoryName = "unparse_logs";

        internal readonly static TraceSource tsUnparse = new TraceSource("UNPARSE", SourceLevels.Verbose);
        internal readonly static TraceSource tsPriorities = new TraceSource("PRIORITIES", SourceLevels.Verbose);

#if DEBUG && !PCL
        static Unparser()
        {
            Directory.CreateDirectory(logDirectoryName);

            Trace.AutoFlush = true;

            tsUnparse.Listeners.Clear();
            tsUnparse.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "00_unparse.log"))));

            tsPriorities.Listeners.Clear();
            tsPriorities.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "priorities.log"))));
        }
#endif

        #endregion

        #region Constants

        private const Direction directionDefault = Direction.LeftToRight;
        private const bool enablePartialInvalidationDefault = false;
        private const bool enableParallelProcessingDefault = false;
        private static readonly bool multiCoreSystem = Environment.ProcessorCount > 1;
        private static readonly object dictionaryKeySafe_null = new object();

        #endregion

        #region State

        #region Immutable after initialization or public settings

        private Grammar _grammar;
        private LanguageData _language;
        private readonly UnparseControl unparseControl;
        private Func<object, Comments> astValueToComments;
        private HashSet<Comments> unparsedComments;
        public bool EnablePartialInvalidation { get; set; }
        public bool EnableParallelProcessing { get; set; }
        public readonly AsyncLock Lock = new AsyncLock();
        private CancellationToken cancellationToken;

        #endregion

        #region Mutable

        public Formatter Formatter { get; set; }
        private ExpressionUnparser expressionUnparser;
        private Direction _direction;
        private ResourceCounter parallelTaskCounter;
        private Dictionary<ConstantTerminal, Dictionary<object, string>> constantTerminalToInverseConstantTable;

        #endregion

        #endregion

        #region Settings

        public int DegreeOfParallelism
        {
            get { return parallelTaskCounter.TotalNumberOfResources; }

            set
            {
                if (value < 1 || value > Environment.ProcessorCount)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format("DegreeOfParallelism should be more than zero and no more than {0} (Environment.ProcessorCount)", Environment.ProcessorCount)
                        );
                }

                parallelTaskCounter = new ResourceCounter(totalNumberOfResources: value, initialAcquiredNumberOfResources: 1);
            }
        }

        private bool UseParallelProcessing { get { return multiCoreSystem && EnableParallelProcessing; } }

        #endregion

        #region Properties

        public Grammar Grammar
        {
            get { return _grammar; }

            private set
            {
                _grammar = value;
                _language = new LanguageData(value);
            }
        }

        public LanguageData Language { get { return _language; } }

        #endregion

        #region Construction

        public Unparser(Grammar grammar, Formatter formatter)
        {
            this.Grammar = grammar;
            this.Formatter = formatter;
            this.unparseControl = grammar.UnparseControl;
            this.expressionUnparser = new ExpressionUnparser(this, grammar.UnparseControl);
            this.EnablePartialInvalidation = enablePartialInvalidationDefault;
            this.EnableParallelProcessing = enableParallelProcessingDefault;
            this.DegreeOfParallelism = Environment.ProcessorCount;
            this.constantTerminalToInverseConstantTable = new Dictionary<ConstantTerminal, Dictionary<object, string>>();
        }

        public Unparser(Grammar grammar)
            : this(grammar, grammar.UnparseControl.DefaultFormatter)
        {
        }

        private Unparser(Unparser that)
        {
            this._grammar = that.Grammar;   // NOTE: we don't want to recalculate _language so we don't use the Grammar property setter
            this.unparseControl = that.unparseControl;
            this.astValueToComments = that.astValueToComments;
            this.unparsedComments = that.unparsedComments;
            this.EnablePartialInvalidation = that.EnablePartialInvalidation;
            this.EnableParallelProcessing = that.EnableParallelProcessing;
            this.parallelTaskCounter = that.parallelTaskCounter;
            this._direction = that._direction;
            this.cancellationToken = that.cancellationToken;
            this.constantTerminalToInverseConstantTable = that.constantTerminalToInverseConstantTable;

            // the following should be set after the constructor
            this.Formatter = null;
            this.expressionUnparser = null;
        }

        private Unparser Spawn(Formatter.ChildLocation childLocation = Formatter.ChildLocation.Unknown)
        {
            return Spawn(child: null, childLocation: childLocation);
        }

        private Unparser Spawn(UnparsableAst child, Formatter.ChildLocation childLocation = Formatter.ChildLocation.Unknown)
        {
            Unparser spawn = new Unparser(this);
            spawn.Formatter = this.Formatter.Spawn(child, childLocation);
            spawn.expressionUnparser = this.expressionUnparser.Spawn(spawn);
            return spawn;
        }

        #endregion

        #region Unparse logic

        public IEnumerable<Utoken> Unparse(Document document, Direction direction = directionDefault)
        {
            return Unparse(document, CancellationToken.None, direction);
        }

        public IEnumerable<Utoken> Unparse(Document document, CancellationToken cancellationToken, Direction direction = directionDefault)
        {
            return Unparse(document, Grammar.Root, cancellationToken, direction);
        }

        public IEnumerable<Utoken> Unparse(Document document, BnfTerm bnfTerm, Direction direction = directionDefault)
        {
            return Unparse(document, bnfTerm, CancellationToken.None, direction);
        }

        public IEnumerable<Utoken> Unparse(Document document, BnfTerm bnfTerm, CancellationToken cancellationToken, Direction direction = directionDefault)
        {
            this.astValueToComments =
                astValue =>
                {
                    Comments comments;
                    return document.AstValueToComments.TryGetValue(astValue, out comments)
                        ? comments
                        : null;
                };

            this.unparsedComments = new HashSet<Comments>();

            return _Unparse(document.Root, bnfTerm, cancellationToken, direction);
        }

        public IEnumerable<Utoken> Unparse(object astValue, Direction direction = directionDefault)
        {
            return Unparse(astValue, CancellationToken.None, direction);
        }

        public IEnumerable<Utoken> Unparse(object astValue, CancellationToken cancellationToken, Direction direction = directionDefault)
        {
            return Unparse(astValue, Grammar.Root, cancellationToken, direction);
        }

        public IEnumerable<Utoken> Unparse(object astValue, BnfTerm bnfTerm, Direction direction = directionDefault)
        {
            return Unparse(astValue, bnfTerm, CancellationToken.None, direction);
        }

        public IEnumerable<Utoken> Unparse(object astValue, BnfTerm bnfTerm, CancellationToken cancellationToken, Direction direction = directionDefault)
        {
            this.astValueToComments = _ => null;
            this.unparsedComments = new HashSet<Comments>();

            return _Unparse(astValue, bnfTerm, cancellationToken, direction);
        }

        private IEnumerable<Utoken> _Unparse(object astValue, BnfTerm bnfTerm, CancellationToken cancellationToken, Direction direction)
        {
            ResetMutableState();
            this.cancellationToken = cancellationToken;
            this.direction = direction;
            var root = new UnparsableAst(bnfTerm, astValue);
            root.SetAsRoot();
            return UnparseRaw(root)
                .Cook(this);
        }

        private Unparser.Direction direction
        {
            get { return this._direction; }
            set
            {
                this._direction = value;
                Formatter.Direction = value;
            }
        }

        private void ResetMutableState()
        {
            Formatter.ResetMutableState();
            // NOTE: expressionUnparser does ResetMutableState automatically
        }

        internal IEnumerable<UtokenBase> UnparseRaw(UnparsableAst self)
        {
            if (self.BnfTerm == null)
                throw new ArgumentNullException("bnfTerm must not be null", "bnfTerm");

            Formatter.Params @params;

            return expressionUnparser.OngoingOperatorGet
                ? UnparseRawMiddle(self)
                : ConcatIfAnyMiddle(
                    Formatter.YieldBefore(self, out @params),
                    UnparseRawMiddle(self),
                    Formatter.YieldAfter(self, @params)
                    );
        }

        private IEnumerable<UtokenBase> UnparseComment(UnparsableAst owner, Comment comment)
        {
            return direction == Direction.LeftToRight
                ? _UnparseComment(owner, comment)
                : _UnparseComment(owner, comment).Reverse();
        }

        private IEnumerable<UtokenBase> _UnparseComment(UnparsableAst owner, Comment comment)
        {
            string[] commentTextLines = Formatter.GetDecoratedCommentTextLines(owner, comment);
            var commentTerminal = GetProperCommentTerminal(comment);

            yield return UtokenValue.CreateText(commentTerminal.StartSymbol, owner).SetDiscriminator(Formatter.CommentStartSymbol);
            yield return UtokenControl.NoWhitespace();

            foreach (var commentTextLine in commentTextLines.Select((commentLine, index) => new { Value = commentLine, Index = index }))
            {
                if (commentTextLine.Index > 0)
                    yield return UtokenWhitespace.NewLine();

                yield return UtokenValue.CreateText(commentTextLine.Value, owner).SetDiscriminator(Formatter.CommentContent);
            }

            yield return UtokenControl.NoWhitespace();

            if (comment.Kind == CommentKind.SingleLine)
            {
                /*
                 * returning a NewLine is more expressive here than returning commentTerminal.EndSymbols[0] (which is a newline string)
                 * 
                 * NOTE that this NewLine is not a simple formatting whitespace. It should be a NewLine regardless to the formatter,
                 * otherwise the source code that follows this comment will got into the comment, and this would be wrong.
                 * 
                 * NOTE that we are wrapping the NewLine in an InsertedUtokens in order to behave correctly during filtering.
                 * */
                yield return new InsertedUtokens(UtokenWhitespace.NewLine()).SetPriority(double.PositiveInfinity);
            }
            else
                yield return UtokenValue.CreateText(commentTerminal.EndSymbols[0], owner).SetDiscriminator(Formatter.CommentEndSymbol);
        }

        private CommentTerminal GetProperCommentTerminal(Comment comment)
        {
            var commentTerminals = Grammar.NonGrammarTerminals.OfType<CommentTerminal>();
            var matchingCommentTerminal = commentTerminals.FirstOrDefault(commentTerminal => GetCommentKind(commentTerminal) == comment.Kind);

            if (!commentTerminals.Any())
                throw new UnparseException("Grammar has no comment terminals therefore cannot unparse domain comments");

            if (matchingCommentTerminal != null)
                return matchingCommentTerminal;     // we found a matching comment terminal

            else if (comment.Kind == CommentKind.SingleLine || comment.TextLines.Length == 1)
                return commentTerminals.First();    // for a singleline comment (or if it can fit into a singleline comment) any comment terminal would be appropriate

            else
                throw new UnparseException("Grammar has no delimited comment terminals, therefore cannot unparse multiline domain comments");
        }

        private CommentKind GetCommentKind(CommentTerminal commentTerminal)
        {
            return GrammarHelper.GetCommentKind(commentTerminal, Formatter.NewLine);
        }

        private IEnumerable<UtokenBase> UnparseRawMiddle(UnparsableAst self)
        {
            Comments comments = astValueToComments(self.AstValue);

            if (comments != null && unparsedComments.Contains(comments))
                comments = null;

            if (comments != null)
            {
                unparsedComments.Add(comments);

                foreach (UtokenBase utoken in Formatter.YieldBeforeComments(self, comments, UnparseComment))
                    yield return utoken;
            }

            if (self.BnfTerm is KeyTerm)
            {
                tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)self.BnfTerm).Text);
                tsUnparse.Debug("SetAsLeave: {0}", self);
                self.SetAsLeave();
                yield return UtokenValue.CreateText(((KeyTerm)self.BnfTerm).Text, self);
            }
            else if (self.BnfTerm is ConstantTerminal)
            {
                string lexeme = GetLexemeByValue((ConstantTerminal)self.BnfTerm, self.AstValue);

                tsUnparse.Debug("constant_terminal: [\"{0}\" ({1})]", lexeme, self.AstValue.ToString());
                tsUnparse.Debug("SetAsLeave: {0}", self);

                self.SetAsLeave();
                yield return UtokenValue.CreateText(lexeme, self);
            }
            else if (self.BnfTerm is Terminal)
            {
                tsUnparse.Debug("terminal: [\"{0}\"]", self.AstValue.ToString());
                tsUnparse.Debug("SetAsLeave: {0}", self);
                self.SetAsLeave();
                yield return UtokenValue.CreateText(self);
            }
            else if (self.BnfTerm is NonTerminal)
            {
                tsUnparse.Debug("nonterminal: {0}", self);
                tsUnparse.Indent();

                try
                {
                    IUnparsableNonTerminal unparsableSelf = self.BnfTerm as IUnparsableNonTerminal;

                    if (unparsableSelf == null)
                        throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", self.AstValue, self.AstValue.GetType().Name, self.BnfTerm.Name));

                    IEnumerable<UtokenValue> directUtokens;

                    if (unparsableSelf.TryGetUtokensDirectly(this, self, out directUtokens))
                    {
                        tsUnparse.Debug("SetAsLeave: {0}", self);
                        self.SetAsLeave();

                        foreach (UtokenValue directUtoken in directUtokens)
                        {
                            if (directUtoken is UtokenToUnparse)
                            {
                                UnparsableAst child = ((UtokenToUnparse)directUtoken).UnparsableAst;

                                foreach (UtokenBase utoken in UnparseRaw(child))
                                    yield return utoken;
                            }
                            else
                                yield return directUtoken;
                        }

                        tsUnparse.Debug("utokenized: [{0}]", self.AstValue != null ? string.Format("\"{0}\"", self.AstValue) : "<<NULL>>");
                    }
                    else
                    {
                        IEnumerable<UnparsableAst> chosenChildren = ChooseChildrenByPriority(self);

                        if (expressionUnparser.NeedsExpressionUnparse(self.BnfTerm))
                        {
                            /*
                             * NOTE: LinkChildrenToEachOthersAndToSelfLazy is being called inside expressionUnparser.Unparse,
                             * because it may extend the list with automatic parentheses.
                             * */
                            foreach (var utoken in expressionUnparser.Unparse(self, chosenChildren, direction))
                                yield return utoken;
                        }
                        else
                        {
                            bool shouldUnparseParallel;
                            int numberOfParallelTasksToStartActually;
                            int subRangeCount;
                            List<UnparsableAst> chosenChildrenList;

                            shouldUnparseParallel = ShouldUnparseParallelAndAcquireTasksIfNeeded(
                                                        chosenChildren,
                                                        out numberOfParallelTasksToStartActually,
                                                        out subRangeCount,
                                                        out chosenChildrenList);

                            if (shouldUnparseParallel)
                            {
                                LinkChildrenToEachOthersAndToSelfLazy(self, chosenChildrenList, enableUnlinkOfChild: false).ConsumeAll();

                                foreach (var utoken in UnparseRawParallel(chosenChildrenList, numberOfParallelTasksToStartActually, subRangeCount))
                                    yield return utoken;
                            }
                            else
                            {
                                foreach (UnparsableAst chosenChild in LinkChildrenToEachOthersAndToSelfLazy(self, chosenChildren, enableUnlinkOfChild: true))
                                    foreach (UtokenBase utoken in UnparseRaw(chosenChild))
                                        yield return utoken;
                            }
                        }
                    }
                }
                finally
                {
                    tsUnparse.Unindent();
                }
            }
            else if (self.BnfTerm is GrammarHint)
            {
                // GrammarHint is legal, but it does not need any unparse
            }
            else
            {
                throw new ArgumentException(string.Format("bnfTerm '{0}' with unknown type: '{1}'" + self.BnfTerm.Name, self.BnfTerm.GetType().Name));
            }

            if (comments != null)
                foreach (UtokenBase utoken in Formatter.YieldAfterComments(self, comments, UnparseComment))
                    yield return utoken;
        }

        private IEnumerable<UtokenBase> UnparseRawParallel(List<UnparsableAst> chosenChildrenList, int numberOfParallelTasksToStartActually, int subRangeCount)
        {
            var subUnparseTasks = new Task[numberOfParallelTasksToStartActually];
            var subUtokensList = new List<UtokenBase>[numberOfParallelTasksToStartActually];

            int numberOfAllTasksToBeReleased = numberOfParallelTasksToStartActually - 1;
            int numberOfReleasedTasks = 0;

            try
            {
                for (int i = 0; i < subUnparseTasks.Length; i++)
                {
                    int taskIndex = i;      // NOTE: needed for closure working correctly

                    subUnparseTasks[taskIndex] =
#if PCL
                    TaskEx.Run(
#else
                    Task.Run(
#endif
                    () =>
                            {
                                try
                                {
                                    subUtokensList[taskIndex] = new List<UtokenBase>();

                                    int subRangeBeginIndex = taskIndex * subRangeCount;
                                    int subRangeEndIndex = Math.Min(subRangeBeginIndex + subRangeCount, chosenChildrenList.Count);

                                    Formatter.ChildLocation childLocation =
                                        chosenChildrenList.Count == 1                       ?   Formatter.ChildLocation.Only :
                                        subRangeBeginIndex == 0                             ?   Formatter.ChildLocation.First :
                                        subRangeBeginIndex == chosenChildrenList.Count - 1  ?   Formatter.ChildLocation.Last :
                                                                                                Formatter.ChildLocation.Middle;

                                    Unparser subUnparser = this.Spawn(chosenChildrenList[subRangeBeginIndex], childLocation);

                                    for (int childIndex = subRangeBeginIndex; childIndex < subRangeEndIndex; childIndex++)
                                    {
                                        subUtokensList[taskIndex].AddRange(subUnparser.UnparseRaw(chosenChildrenList[childIndex]));
                                        this.cancellationToken.ThrowIfCancellationRequested();
                                    }
                                }
                                finally
                                {
                                    bool released = ReleaseTaskIfNeeded(taskIndex);
                                    if (released) numberOfReleasedTasks++;
                                }
                            },
                            this.cancellationToken
                        );

                    this.cancellationToken.ThrowIfCancellationRequested();
                }

                Task.WaitAll(subUnparseTasks, this.cancellationToken);
            }
            finally
            {
                ReleaseRemainingTasksIfAny(numberOfRemainingTasksToBeReleased: numberOfAllTasksToBeReleased - numberOfReleasedTasks);
            }

            foreach (var subUtokens in subUtokensList)
            {
                foreach (UtokenBase utoken in subUtokens)
                {
                    yield return utoken;
                    this.cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        private bool ShouldUnparseParallelAndAcquireTasksIfNeeded(IEnumerable<UnparsableAst> chosenChildren,
            out int numberOfParallelTasksToStartActually, out int subRangeCount, out List<UnparsableAst> chosenChildrenList)
        {
            numberOfParallelTasksToStartActually = -1;
            subRangeCount = -1;
            chosenChildrenList = null;

            if (!UseParallelProcessing)
                return false;

            chosenChildrenList = chosenChildren.ToList();

            if (chosenChildrenList.Count <= 1)
                return false;

            // heuristics
            if (chosenChildrenList.Count / 2 < parallelTaskCounter.FreeNumberOfResources)
                return false;

            int numberOfParallelTasksToStartIdeally = chosenChildrenList.Count;
            int numberOfNewParallelTasksToStartIdeally = numberOfParallelTasksToStartIdeally - 1;
            int numberOfNewParallelTasksCanBeStartedActually;

            if (!parallelTaskCounter.TryAcquireOrLess(numberOfNewParallelTasksToStartIdeally, out numberOfNewParallelTasksCanBeStartedActually))
                return false;

            int numberOfParallelTasksCanBeStartedActually = numberOfNewParallelTasksCanBeStartedActually + 1;
            subRangeCount = (int)Math.Ceiling((double)(chosenChildrenList.Count) / numberOfParallelTasksCanBeStartedActually);
            numberOfParallelTasksToStartActually = (int)Math.Ceiling((double)(chosenChildrenList.Count) / subRangeCount);

            if (numberOfParallelTasksToStartActually < numberOfParallelTasksCanBeStartedActually)
                parallelTaskCounter.Release(numberOfParallelTasksCanBeStartedActually - numberOfParallelTasksToStartActually);    // we don't need all the tasks

            return true;
        }

        private bool ReleaseTaskIfNeeded(int taskIndex)
        {
            if (taskIndex > 0)
            {
                parallelTaskCounter.Release(1);     // release all tasks except the first one
                return true;
            }
            else
                return false;
        }

        private bool ReleaseRemainingTasksIfAny(int numberOfRemainingTasksToBeReleased)
        {
            if (numberOfRemainingTasksToBeReleased > 0)
            {
                parallelTaskCounter.Release(numberOfRemainingTasksToBeReleased);     // release all tasks except the first one
                return true;
            }
            else
                return false;
        }

        internal IEnumerable<UnparsableAst> LinkChildrenToEachOthersAndToSelfLazy(UnparsableAst self, IEnumerable<UnparsableAst> children, bool enableUnlinkOfChild = true)
        {
            UnparsableAst childPrevSibling = null;

            return children
                .ForAll(
                    executeBeforeEachIteration:
                        child =>
                        {
                            LinkChild(self, child, childPrevSibling, enableUnlinkOfChild);
                            childPrevSibling = child;
                        },

                    executeAfterFinished:
                        () =>
                            LinkChild(self, child: null, childPrevSibling: childPrevSibling, enableUnlinkOfChild: enableUnlinkOfChild)
                );
        }

        private void LinkChild(UnparsableAst self, UnparsableAst child, UnparsableAst childPrevSibling, bool enableUnlinkOfChild)
        {
            if (child != null)
            {
                tsUnparse.Debug("child is linked: {0}", child);

                child.SyntaxParent = self;

                child.AstParent =   self.BnfTerm is BnfiTermRecord  ?   self :
                                    self.IsAstParentCalculated      ?   self.AstParent :
                                                                        UnparsableAst.NonCalculated;     // NOTE: if NonCalculated then it will be calculated later

                child.AstImage =    child.AstValue != self.AstValue ?   self :
                                    self.IsAstImageCalculated       ?   self.AstImage :
                                                                        UnparsableAst.NonCalculated;     // NOTE: if NonCalculated then it will be calculated later

                if (child.AstParentMember == null)
                    child.AstParentMember = self.AstParentMember;
            }

            if (!IsPrevMostChildCalculated(self))
            {
                tsUnparse.Debug("{2} of {0} has been set to {1}", self, child, direction == Direction.LeftToRight ? "LeftMostChild" : "RightMostChild");
                SetPrevMostChild(self, child);
            }

            if (child == null)
            {
                tsUnparse.Debug("{2} of {0} has been set to {1}", self, childPrevSibling, direction == Direction.LeftToRight ? "RightMostChild" : "LeftMostChild");
                SetNextMostChild(self, childPrevSibling);
            }

            if (child != null)
                SetPrevSibling(child, childPrevSibling);

            if (childPrevSibling != null)
            {
                // NOTE: if right-to-left then the next sibling is the left sibling, which is needed for deferred utokens even if we are not building full unparse tree
                if (buildFullUnparseTree || direction == Direction.RightToLeft)
                    SetNextSibling(childPrevSibling, child);  // we have the prev sibling of childPrevSibling set already, and now we set the next sibling too

                if (enableUnlinkOfChild)
                    UnlinkChildFromChildPrevSiblingIfNotFullUnparse(childPrevSibling);      // we unlink child prev sibling if unneeded
            }
        }

        private void UnlinkChildFromChildPrevSiblingIfNotFullUnparse(UnparsableAst child, bool enforce = false)
        {
            if (!enforce && direction == Direction.LeftToRight && child.IsLeftSiblingNeededForDeferredCalculation)
                return;

            if (!buildFullUnparseTree)
                SetPrevSibling(child, UnparsableAst.ThrownOut);  // prev sibling is not needed anymore
        }

        private void SetPrevSibling(UnparsableAst current, UnparsableAst prev)
        {
            if (direction == Direction.LeftToRight)
                current.LeftSibling = prev;
            else
                current.RightSibling = prev;
        }

        private void SetNextSibling(UnparsableAst current, UnparsableAst next)
        {
            if (direction == Direction.LeftToRight)
                current.RightSibling = next;
            else
                current.LeftSibling = next;
        }

        private void SetPrevMostChild(UnparsableAst self, UnparsableAst prevMostChild)
        {
            if (direction == Direction.LeftToRight)
                self.LeftMostChild = prevMostChild;
            else
                self.RightMostChild = prevMostChild;
        }

        private void SetNextMostChild(UnparsableAst self, UnparsableAst nextMostChild)
        {
            if (direction == Direction.LeftToRight)
                self.RightMostChild = nextMostChild;
            else
                self.LeftMostChild = nextMostChild;
        }

        private bool IsPrevMostChildCalculated(UnparsableAst self)
        {
            return direction == Direction.LeftToRight
                ? self.IsLeftMostChildCalculated
                : self.IsRightMostChildCalculated;
        }

        private bool IsNextMostChildCalculated(UnparsableAst self)
        {
            return direction == Direction.LeftToRight
                ? self.IsRightMostChildCalculated
                : self.IsLeftMostChildCalculated;
        }

        private static IEnumerable<UtokenBase> ConcatIfAnyMiddle(IEnumerable<UtokenBase> utokensBefore, IEnumerable<UtokenBase> utokensMiddle, IEnumerable<UtokenBase> utokensAfter)
        {
            bool isAnyUtokenMiddle = false;

            foreach (UtokenBase utokenMiddle in utokensMiddle)
            {
                if (!isAnyUtokenMiddle)
                {
                    foreach (UtokenBase utokenBefore in utokensBefore)
                        yield return utokenBefore;
                }

                isAnyUtokenMiddle = true;

                yield return utokenMiddle;
            }

            if (isAnyUtokenMiddle)
            {
                foreach (UtokenBase utokenAfter in utokensAfter)
                    yield return utokenAfter;
            }
        }

        private IEnumerable<UnparsableAst> ChooseChildrenByPriority(UnparsableAst self)
        {
            IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)self.BnfTerm;

            tsPriorities.Debug("{0} BEGIN priorities", unparsable.AsNonTerminal());
            tsPriorities.Indent();

            var childrenWithPriority = GetChildrenWithMaxPriority(self);

            if (childrenWithPriority.Priority.Value == null)
            {
                throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.",
                    self.AstValue, self.AstValue.GetType().Name, self.BnfTerm.Name));
            }

            tsPriorities.Unindent();
            tsPriorities.Debug("{0} END priorities", unparsable.AsNonTerminal());
            tsPriorities.Debug("");

            return childrenWithPriority.Children;
        }

        private ChildrenWithPriority GetChildrenWithMaxPriority(UnparsableAst self)
        {
            IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)self.BnfTerm;

            return GetChildBnfTermLists(unparsable.AsNonTerminal())
                .Select(
                    (childBnfTerms, childBnfTermsIndex) =>
                    {
                        Children children = new Children(
                            unparsable.GetChildren(new ChildBnfTerms(childBnfTerms, childBnfTermsIndex), self.AstValue, direction),
                            childBnfTermsIndex);

                        return new ChildrenWithPriority(
                            children,
                            GetChildrenPriority(self, children, childBnfTermsIndex)
                                .DebugWriteLinePriority(tsPriorities, self)
                        );
                    }
                )
                .Where(childrenWithPriority => !childrenWithPriority.Children.Contains(self))
                .MaxItem(childrenWithPriority => childrenWithPriority.Priority);
        }

        private Priority GetChildrenPriority(UnparsableAst self, Unparser.Children children, int childrenIndex)
        {
            IUnparsableNonTerminal unparsable = (IUnparsableNonTerminal)self.BnfTerm;

            UnparseHint unparseHint = self.BnfTerm is BnfiTermNonTerminal
                ? ((BnfiTermNonTerminal)self.BnfTerm).GetUnparseHint(childrenIndex)
                : null;

            return unparseHint != null
                ? new Priority(PriorityKind.User, unparseHint.GetChildrenPriority(self.AstValue, children.Select(childUnparsableAst => childUnparsableAst.AstValue)))
                : new Priority(PriorityKind.System, unparsable.GetChildrenPriority(this, self.AstValue, children, direction));
        }

        internal static IEnumerable<BnfTermList> GetChildBnfTermListsLeftToRight(NonTerminal nonTerminal)
        {
            return nonTerminal.Productions.Select(production => production.RValues);
        }

        internal IEnumerable<IEnumerable<BnfTerm>> GetChildBnfTermLists(NonTerminal nonTerminal)
        {
            return GetChildBnfTermListsLeftToRight(nonTerminal)
                .Select(bnfTerms => direction == Direction.LeftToRight ? bnfTerms : bnfTerms.ReverseOptimized());
        }

        private bool buildFullUnparseTree { get { return EnablePartialInvalidation; } }

        private string GetLexemeByValue(ConstantTerminal constantTerminal, object value)
        {
            value = ObjectToDictionaryKeySafe(value);
            Dictionary<object, string> inverseConstantTable;

            /*
             * First we just try to get the value without locking on constantTerminalToInverseConstantTable since the desired inverseConstantTable
             * will be in constantTerminalToInverseConstantTable after the first access, so most of the time we just get the value, and we are done.
             * Always locking on it would be a waste of time, since lock is a time consuming operation.
             * */
            if (!constantTerminalToInverseConstantTable.TryGetValue(constantTerminal, out inverseConstantTable))
            {
                // first access -> we don't have the desired inverseConstantTable -> lock on it, because we will create it and store it in constantTerminalToInverseConstantTable
                lock (constantTerminalToInverseConstantTable)
                {
                    /*
                     * But first we have to query constantTerminalToInverseConstantTable again since we locked only *after* the first query,
                     * and during this period (after the first query, but before the lock) a parallel task might have already locked on constantTerminalToInverseConstantTable,
                     * created the desired inverseConstantTable and stored it in constantTerminalToInverseConstantTable, and released the lock.
                     * */
                    if (!constantTerminalToInverseConstantTable.TryGetValue(constantTerminal, out inverseConstantTable))
                    {
                        inverseConstantTable = CreateInverseConstantTable(constantTerminal);
                        constantTerminalToInverseConstantTable.Add(constantTerminal, inverseConstantTable);
                    }
                }
            }

            return inverseConstantTable[value];
        }

        private static Dictionary<object, string> CreateInverseConstantTable(ConstantTerminal constantTerminal)
        {
            /*
             * NOTE: constantTerminal.Constants might not be bijective
             * */

            var inverseConstantTable = new Dictionary<object, string>();

            foreach (var pair in constantTerminal.Constants)
            {
                object value = ObjectToDictionaryKeySafe(pair.Value);

                if (!inverseConstantTable.ContainsKey(value))
                    inverseConstantTable.Add(value, pair.Key);
            }

            return inverseConstantTable;
        }

        private static object ObjectToDictionaryKeySafe(object obj)
        {
            return obj != null ? obj : dictionaryKeySafe_null;
        }

        #endregion

        #region IUnparser implementation

        int? IUnparser.GetPriority(UnparsableAst unparsableAst)
        {
            IUnparsableNonTerminal unparsable = unparsableAst.BnfTerm as IUnparsableNonTerminal;

            if (unparsable != null)
            {

                tsPriorities.Indent();

                Priority priority = GetChildrenWithMaxPriority(unparsableAst).Priority;

                tsPriorities.Unindent();

                priority.DebugWriteLinePriority(tsPriorities, unparsableAst, messageAfter: " (MAX)");

                return priority.Value;
            }
            else
            {
                Misc.DebugWriteLinePriority(0, tsPriorities, unparsableAst, messageAfter: " (for terminal)");
                return 0;
            }
        }

        IFormatProvider IUnparser.FormatProvider { get { return this.Formatter.FormatProvider; } }

        #endregion

        #region IPostProcessHelper implementation

        Formatter IPostProcessHelper.Formatter { get { return this.Formatter; } }

        Unparser.Direction IPostProcessHelper.Direction
        {
            get { return this.direction; }
        }

        Action<UnparsableAst> IPostProcessHelper.UnlinkChildFromChildPrevSiblingIfNotFullUnparse
        {
            get
            {
                return child =>
                    {
                        if (direction == Direction.LeftToRight && child.IsLeftSiblingNeededForDeferredCalculation)
                            this.UnlinkChildFromChildPrevSiblingIfNotFullUnparse(child, enforce: true);
                    };
            }
        }

        #endregion

        #region Types

        public enum Direction { LeftToRight, RightToLeft }

        public class ChildBnfTerms : IEnumerable<BnfTerm>
        {
            public IEnumerable<BnfTerm> Content { get; private set; }
            public int ContentIndex { get; private set; }
            private int? countCache;

            public ChildBnfTerms(IEnumerable<BnfTerm> content, int contentIndex)
            {
                this.Content = content;
                this.ContentIndex = contentIndex;
                this.countCache = null;
            }

            public IEnumerator<BnfTerm> GetEnumerator()
            {
                return Content.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /*
             * NOTE that Content is either a BnfTermList or a ReverseList<BnfTerm>. Both of them implements the ICollection<BnfTerm> interface,
             * so Enumerable.Count method when calling Content.Count() will notice it and will use the ICollection<BnfTerm>.Count property
             * instead of iterating through the whole enumeration and counting all the items.
             * */
            public int Count { get { return countCache ?? (int)(countCache = Content.Count()); } }
        }

        public class Children : IEnumerable<UnparsableAst>
        {
            public IEnumerable<UnparsableAst> Content { get; private set; }
            public int ContentIndex { get; private set; }

            public Children(IEnumerable<UnparsableAst> content, int contentIndex)
            {
                this.Content = content;
                this.ContentIndex = contentIndex;
            }

            public IEnumerator<UnparsableAst> GetEnumerator()
            {
                return Content.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ChildrenWithPriority
        {
            public readonly Children Children;
            public readonly Priority Priority;

            public ChildrenWithPriority(Children children, Priority priority)
            {
                this.Children = children;
                this.Priority = priority;
            }
        }

        internal enum PriorityKind { System, User }

        internal struct Priority : IComparable<Priority>
        {
            public readonly PriorityKind Kind;
            public readonly int? Value;

            public Priority(PriorityKind kind, int? value)
            {
                this.Kind = kind;
                this.Value = value;
            }

            public int CompareTo(Priority that)
            {
                if (this.Kind == PriorityKind.User && that.Kind == PriorityKind.System)
                    return this.Value != null ? 1 : Compare(this.Value, that.Value);
                else if (this.Kind == PriorityKind.System && that.Kind == PriorityKind.User)
                    return that.Value != null ? -1 : Compare(this.Value, that.Value);
                else // this.Kind == that.Kind
                    return Compare(this.Value, that.Value);
            }

            private static int Compare(int? priorityValue1, int? priorityValue2)
            {
                if (priorityValue1 == null && priorityValue2 == null)
                    return 0;
                else if (priorityValue1 != null && priorityValue2 == null)
                    return 1;
                else if (priorityValue1 == null && priorityValue2 != null)
                    return -1;
                else
                    return priorityValue1.Value.CompareTo(priorityValue2.Value);
            }
        }

        #endregion
    }
}
