using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Utility;
using Sarcasm.GrammarAst;

namespace Sarcasm.Unparsing
{
    internal class ExpressionUnparser
    {
        #region Tracing

        internal readonly static TraceSource tsParentheses = new TraceSource("PARENTHESES", SourceLevels.Verbose);

#if DEBUG
        static ExpressionUnparser()
        {
            Directory.CreateDirectory(Unparser.logDirectoryName);

            Trace.AutoFlush = true;

            tsParentheses.Listeners.Clear();
            tsParentheses.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(Unparser.logDirectoryName, "parentheses.log"))));
        }
#endif

        #endregion

        #region State

        #region Immutable after initialization

        #region Used only during initialization

        private IDictionary<BnfTerm, ParenthesizedExpression> _expressionToParentheses;
        private IDictionary<BnfTerm, MultiOperatorInfo> _flaggedOrDerivedOperatorToMultiOperatorInfo;
        private Bag<BnfTerm> _examinedBnfTermsInCurrentPath;

        #endregion

        private readonly Unparser unparser;

        private ISet<BnfTerm> expressionsThatCanCauseOthersBeingParenthesized;
        private Dictionary<BnfTerm, ParenthesizedExpression> expressionThatMayNeedParenthesesToParentheses;
        private Dictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKind;

        #endregion

        #region Mutable

        private Stack<SurroundingOperators> surroundingOperators = new Stack<SurroundingOperators>();
        private Stack<ParenthesizedExpression> surroundingParentheses = new Stack<ParenthesizedExpression>();
        private OperatorInfo _operatorInfo = null;
        private AutoCleanupCounter ongoingExpressionUnparseLevel = new AutoCleanupCounter(0);
        private AutoCleanupCounter ongoingOperatorUnparseLevel = new AutoCleanupCounter(0);
        private AutoCleanupCounter ongoingOperatorGetLevel = new AutoCleanupCounter(0);
        private Unparser.Direction direction;

        #endregion

        #endregion

        #region Construction

        public ExpressionUnparser(Unparser unparser, UnparseControl unparseControl)
        {
            this.unparser = unparser;
            InitParentheses(unparseControl);
        }

        private ExpressionUnparser(ExpressionUnparser that, Unparser newUnparser)
        {
            this.unparser = newUnparser;

            this.expressionsThatCanCauseOthersBeingParenthesized = that.expressionsThatCanCauseOthersBeingParenthesized;
            this.expressionThatMayNeedParenthesesToParentheses = that.expressionThatMayNeedParenthesesToParentheses;
            this.bnfTermToBnfTermKind = that.bnfTermToBnfTermKind;
            this.direction = that.direction;
        }

        public ExpressionUnparser Spawn(Unparser newUnparser)
        {
            if (this.OngoingExpressionUnparse)
                throw new InvalidOperationException("Cannot spawn unparser during an ongoing expression unparse");

            return new ExpressionUnparser(this, newUnparser);
        }

        #endregion

        #region Initialization

        private void InitParentheses(UnparseControl unparseControl)
        {
            // NOTE: operators should be determined regardless that the user specified expressions and parentheses
            DetermineOperatorsParenthesesExpressions();
            RegisteringExpressionsThatNeedParentheses(unparseControl);

#if DEBUG
            tsParentheses.Debug("");
            tsParentheses.Debug("----------------------------------------");
            tsParentheses.Debug("");
            tsParentheses.Debug("expressionsThatCanCauseOthersBeingParenthesized:");
            tsParentheses.Debug("");

            foreach (BnfTerm expression in expressionsThatCanCauseOthersBeingParenthesized)
                tsParentheses.Debug(expression);

            tsParentheses.Debug("");
            tsParentheses.Debug("expressionThatMayNeedParenthesesToParentheses:");
            tsParentheses.Debug("");

            foreach (var pair in expressionThatMayNeedParenthesesToParentheses)
                tsParentheses.Debug("{0}; left parenthesis: '{1}', right parenthesis: '{2}'", pair.Key, pair.Value.Left, pair.Value.Right);
#endif
        }

        private void DetermineOperatorsParenthesesExpressions()
        {
            this.bnfTermToBnfTermKind = new Dictionary<BnfTerm, BnfTermKind>();
            this._expressionToParentheses = new Dictionary<BnfTerm, ParenthesizedExpression>();
            this._flaggedOrDerivedOperatorToMultiOperatorInfo = new Dictionary<BnfTerm, MultiOperatorInfo>();

            CalculateBnfTermKind(unparser.Grammar.Root);
        }

        private BnfTermKind CalculateBnfTermKind(BnfTerm current)
        {
            BnfTermKind currentKind;

            if (this.bnfTermToBnfTermKind.TryGetValue(current, out currentKind))
            {
                /*
                 * if currentKind is Undetermined here it means that we are in a recursion,
                 * which means that currentKind can be only Other (i.e. not an operator, nor a parenthesis, etc.)
                 * */
                if (currentKind == BnfTermKind.Undetermined)
                    this.bnfTermToBnfTermKind[current] = currentKind = BnfTermKind.Other;

                return currentKind;
            }

            this.bnfTermToBnfTermKind.Add(current, BnfTermKind.Undetermined);

            tsParentheses.Indent();

            // need to "label" every bnfterm
            if (current is NonTerminal)
                currentKind = CalculateBnfTermKindForNonTerminal((NonTerminal)current);
            else
                currentKind = BnfTermKind.Other;

            // e.g. operator can be non-terminal and terminal as well (or at least they can be flagged so); if it is non-terminal, then we override currentKind
            OverrideBnfTermKindInCaseOfSpecificInfo(current, ref currentKind);

            tsParentheses.Unindent();
            currentKind.DebugWriteLineBnfTermKind(tsParentheses, current);

            Debug.Assert(currentKind != BnfTermKind.Undetermined);

            this.bnfTermToBnfTermKind[current] = currentKind;        // not using Add, because bnfTermToBnfTermKind[current] has already been set to Undetermined or Other

            return currentKind;
        }

        private void OverrideBnfTermKindInCaseOfSpecificInfo(BnfTerm current, ref BnfTermKind currentKind)
        {
            if (IsFlaggedOperator(current))
            {
                currentKind = BnfTermKind.Operator;
                _flaggedOrDerivedOperatorToMultiOperatorInfo[current] = new MultiOperatorInfo(current, current);
            }
            else if (current.IsOpenBrace())
                currentKind = BnfTermKind.LeftParenthesis;
            else if (current.IsCloseBrace())
                currentKind = BnfTermKind.RightParenthesis;
            else if (current is GrammarHint)
                currentKind = BnfTermKind.GrammarHint;
        }

        private BnfTermKind CalculateBnfTermKindForNonTerminal(NonTerminal current)
        {
            BnfTermKind? currentKind = null;

            var childOperators = new List<BnfTerm>();

            foreach (BnfTermList children in Unparser.GetChildBnfTermListsLeftToRight(current))
            {
                int operatorCount = 0;
                int leftParenthesisCount = 0;
                int rightParenthesisCount = 0;
                int otherCount = 0;

                /*
                 * NOTE: we should not read bnfTermToBnfTermKind in this method (because of recursive definitions in grammar),
                 * that's why we store BnfTermKind as well
                 * */
                BnfTerm prevChild = null;
                BnfTermKind? prevChildKind = null;
                ParenthesizedExpression parentheses = null;
                BnfTerm childExpression = null;

                if (children.Count == 0)
                    continue;

                BnfTerm childOperator = null;

                foreach (BnfTerm child in children)
                {
                    BnfTermKind childKind = CalculateBnfTermKind(child);

                    Debug.Assert(childKind != BnfTermKind.Undetermined);

                    #region Handle childKind

                    switch (childKind)
                    {
                        case BnfTermKind.Operator:
                            operatorCount++;
                            childOperator = child;
                            childExpression = null;
                            parentheses = null;
                            break;

                        case BnfTermKind.LeftParenthesis:
                            leftParenthesisCount++;
                            parentheses = new ParenthesizedExpression() { LeftParenthesis = child };
                            childExpression = null;
                            break;

                        case BnfTermKind.RightParenthesis:
                            rightParenthesisCount++;
                            if (prevChildKind == BnfTermKind.Other && childExpression == prevChild)
                            {
                                Debug.Assert(parentheses != null && parentheses.LeftParenthesis != null && parentheses.Expression != null);

                                parentheses.RightParenthesis = child;

                                ParenthesizedExpression registeredParentheses;
                                if (_expressionToParentheses.TryGetValue(childExpression, out registeredParentheses))
                                {
                                    if (parentheses != registeredParentheses)
                                        _expressionToParentheses[childExpression] = null;  // ambiguous parentheses -> set to null, and check later (we might not need the parentheses at all)
                                }
                                else
                                    _expressionToParentheses.Add(childExpression, parentheses);
                            }
                            childExpression = null;
                            parentheses = null;
                            break;

                        case BnfTermKind.Other:
                            otherCount++;
                            if (prevChildKind == BnfTermKind.LeftParenthesis)
                            {
                                Debug.Assert(parentheses != null && parentheses.LeftParenthesis != null);
                                childExpression = child;
                                parentheses.Expression = childExpression;
                            }
                            else
                            {
                                childExpression = null;
                                parentheses = null;
                            }
                            break;
                    }

                    #endregion

                    if (childKind != BnfTermKind.GrammarHint)
                    {
                        prevChild = child;
                        prevChildKind = childKind;
                    }
                }

                #region Determine childListLooksLike

                BnfTermKind? childListLooksLike;

                if (operatorCount == 1 && leftParenthesisCount == 0 && rightParenthesisCount == 0 && otherCount == 0)
                    childListLooksLike = BnfTermKind.Operator;
                else if (leftParenthesisCount == 1 && operatorCount == 0 && rightParenthesisCount == 0 && otherCount == 0)
                    childListLooksLike = BnfTermKind.LeftParenthesis;
                else if (rightParenthesisCount == 1 && operatorCount == 0 && leftParenthesisCount == 0 && otherCount == 0)
                    childListLooksLike = BnfTermKind.RightParenthesis;
                else
                    childListLooksLike = BnfTermKind.Other;

                #endregion

                if (childListLooksLike == BnfTermKind.Operator)
                    childOperators.Add(childOperator);

                #region Determine currentKind

                if ((currentKind == null || currentKind == BnfTermKind.Operator) && childListLooksLike == BnfTermKind.Operator)
                    currentKind = BnfTermKind.Operator;
                else if ((currentKind == null || currentKind == BnfTermKind.LeftParenthesis) && childListLooksLike == BnfTermKind.LeftParenthesis)
                    currentKind = BnfTermKind.LeftParenthesis;
                else if ((currentKind == null || currentKind == BnfTermKind.RightParenthesis) && childListLooksLike == BnfTermKind.RightParenthesis)
                    currentKind = BnfTermKind.RightParenthesis;
                else
                    currentKind = BnfTermKind.Other;

                #endregion
            }

            if (currentKind == BnfTermKind.Operator)
                HandleOperators(current, childOperators);

            return currentKind ?? BnfTermKind.Other;
        }

        private void HandleOperators(BnfTerm @operator, IEnumerable<BnfTerm> childOperators)
        {
            BnfTerm strongestFlaggedChildOperator = childOperators.Aggregate(
                (BnfTerm)null,
                (strongestFlaggedChildOperatorSoFar, childOperator) => strongestFlaggedChildOperatorSoFar == null ||
                    GetPrecedence(_flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].StrongestFlaggedOperator) > GetPrecedence(_flaggedOrDerivedOperatorToMultiOperatorInfo[strongestFlaggedChildOperatorSoFar].StrongestFlaggedOperator)
                    ? _flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].StrongestFlaggedOperator
                    : strongestFlaggedChildOperatorSoFar
                );

            BnfTerm weakestFlaggedChildOperator = childOperators.Aggregate(
                (BnfTerm)null,
                (weakestFlaggedChildOperatorSoFar, childOperator) => weakestFlaggedChildOperatorSoFar == null ||
                    GetPrecedence(_flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].WeakestFlaggedOperator) > GetPrecedence(_flaggedOrDerivedOperatorToMultiOperatorInfo[weakestFlaggedChildOperatorSoFar].WeakestFlaggedOperator)
                    ? _flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].WeakestFlaggedOperator
                    : weakestFlaggedChildOperatorSoFar
                );

            _flaggedOrDerivedOperatorToMultiOperatorInfo.Add(@operator, new MultiOperatorInfo(strongestFlaggedChildOperator, weakestFlaggedChildOperator));
        }

        private void RegisteringExpressionsThatNeedParentheses(UnparseControl unparseControl)
        {
            if (unparseControl.ExpressionToParenthesesHasBeenSet)
            {
                this.expressionsThatCanCauseOthersBeingParenthesized = new HashSet<BnfTerm>(unparseControl.GetExpressionToParentheses().Keys);
                this.expressionThatMayNeedParenthesesToParentheses = unparseControl.GetExpressionToParentheses().ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else
            {
                this.expressionsThatCanCauseOthersBeingParenthesized = new HashSet<BnfTerm>();
                this.expressionThatMayNeedParenthesesToParentheses = new Dictionary<BnfTerm, ParenthesizedExpression>();
                this._examinedBnfTermsInCurrentPath = new Bag<BnfTerm>();

                RegisteringExpressionsThatNeedParentheses(unparser.Grammar.Root);

                lock (unparseControl)
                {   // handle concurrent unparsers initializations with one shared grammar and unparseControl
                    if (!unparseControl.ExpressionToParenthesesHasBeenSet)
                        unparseControl.SetExpressionToParentheses(expressionThatMayNeedParenthesesToParentheses);
                }
            }
        }

        private void RegisteringExpressionsThatNeedParentheses(NonTerminal nonTerminal)
        {
            foreach (var childBnfTerms in Unparser.GetChildBnfTermListsLeftToRight(nonTerminal))
            {
                Unparse(
                    new UnparsableAst(nonTerminal, astValue: null),
                    childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue: null)),
                    initialization: true
                    );
            }
        }

        #endregion

        #region Unparse

        public bool OngoingExpressionUnparse { get { return ongoingExpressionUnparseLevel > 0; } }

        public bool OngoingOperatorGet { get { return ongoingOperatorGetLevel > 0; } }

        public bool NeedsExpressionUnparse(BnfTerm bnfTerm)
        {
            return OngoingExpressionUnparse || expressionsThatCanCauseOthersBeingParenthesized.Contains(bnfTerm);
        }

        public IReadOnlyList<UtokenBase> Unparse(UnparsableAst self, IEnumerable<UnparsableAst> children, Unparser.Direction direction)
        {
            if (ongoingExpressionUnparseLevel == 0)
            {
                ResetMutableState();
                this.direction = direction;
            }

            using (ongoingExpressionUnparseLevel.IncrAutoDecr())
                return Unparse(self, children, initialization: false);
        }

        private void ResetMutableState()
        {
            surroundingOperators.Clear();
            surroundingParentheses.Clear();
            _operatorInfo = null;

            // ongoingExpressionUnparseLevel and ongoingOperatorUnparseLevel do not need reset, since they are automatically in good state
        }

        private IReadOnlyList<UtokenBase> Unparse(UnparsableAst self, IEnumerable<UnparsableAst> children, bool initialization)
        {
            if (ongoingOperatorUnparseLevel > 0)
            {
                if (!initialization)
                    children = unparser.LinkChildrenToEachOthersAndToSelfLazy(self, children);

                var utokens = children.SelectMany(child => UnparseRawEager(child, initialization)).ToList();
                UpdateOperatorInfo(self.BnfTerm);
                return utokens;
            }

            #region Initialization

            if (initialization && _examinedBnfTermsInCurrentPath.GetCount(self.BnfTerm) == 2)
            {
                UpdateOperatorInfo(self.BnfTerm);
                return new UtokenBase[0];   // we have already expanded the current bnfTerm recursively twice during the current path, so no need to go further
            }

            #endregion

            using (initialization
                ? new AutoCleanup(
                    () => _examinedBnfTermsInCurrentPath.Add(self.BnfTerm),
                    () => _examinedBnfTermsInCurrentPath.Remove(self.BnfTerm))
                : AutoCleanup.None)
            using (new AutoCleanup(
                () => BeginParenthesesContext(self.BnfTerm, initialization),
                () => EndParenthesesContext(self.BnfTerm, initialization)))
            {
                var utokens = new List<UtokenBase>();

                var childList = children.ToList();   // we are going to read the children twice, so we enforce them to be fully loaded into a list
                children = childList;

                IReadOnlyList<Operator> operators = GetOperators(children, initialization).ToList();

                bool needParentheses =
                    operators
                        .OfType<Operator.Actual>()
                        .Any(
                            @operator => !@operator.IsInsideParentheses &&
                            NeedParentheses(GetFlaggedOperatorForOutside(@operator, initialization))
                            );

                #region Initialization

                if (needParentheses && initialization)
                {
                    try
                    {
                        RegisterSurroundingParenthesesDuringInitialization();
                    }
                    catch (UnparserInitializationException e)
                    {
                        throw new UnparserInitializationException(e.Message + string.Format("for expression '{0}'", self.BnfTerm));
                    }
                }

                #endregion

                if (needParentheses && !initialization)
                {
                    childList.Insert(0, GetParenthesis(direction == Unparser.Direction.LeftToRight ? ParenthesisKind.Left : ParenthesisKind.Right, self));
                    childList.Add(GetParenthesis(direction == Unparser.Direction.LeftToRight ? ParenthesisKind.Right : ParenthesisKind.Left, self));
                }

                if (!initialization)
                    children = unparser.LinkChildrenToEachOthersAndToSelfLazy(self, children);

                using (var childEnumerator = children.GetEnumerator())
                {
                    Operator prevOperator = null;

                    foreach (Operator @operator in operators.Concat((Operator)null))    // extra 'null' element for the inner 'while' cycle to finish
                    {
                        using (new AutoCleanup(
                            () => BeginSurroundingOperatorsContext(
                                    self.BnfTerm,
                                    GetLeftFlaggedOperatorForInside(direction == Unparser.Direction.LeftToRight ? prevOperator : @operator, initialization),
                                    GetRightFlaggedOperatorForInside(direction == Unparser.Direction.LeftToRight ? @operator : prevOperator, initialization)),
                            () => EndSurroundingOperatorsContext()))
                        {
                            while (childEnumerator.MoveNext() && (@operator == null || childEnumerator.Current.BnfTerm != @operator.BnfTerm))
                                utokens.AddRange(UnparseRawEager(childEnumerator.Current, initialization));
                        }

                        if (@operator != null)
                        {
                            using (ongoingOperatorUnparseLevel.IncrAutoDecr())
                                utokens.AddRange(UnparseRawEager(childEnumerator.Current, initialization));

                            // NOTE that we cannot use the utokens that we may get by GetOperators, because real unparse needs its context to be fully unparsed (see: after, before, between insertions)

                            // NOTE that we begin with a childEnumerator.MoveNext() call in the next iteration for the next operator, so we won't unparse the current operator twice
                        }

                        prevOperator = @operator;
                    }
                }

                UpdateOperatorInfo(self.BnfTerm);

                return utokens;
            }
        }

        private void RegisterSurroundingParenthesesDuringInitialization()
        {
            ParenthesizedExpression parentheses = GetSurroundingParentheses();
            ParenthesizedExpression registeredParentheses;
            bool alreadyRegistered;

            if (parentheses != null &&
                (!(alreadyRegistered = expressionThatMayNeedParenthesesToParentheses.TryGetValue(parentheses.Expression, out registeredParentheses)) || parentheses == registeredParentheses))
            {
                if (!alreadyRegistered)
                    expressionThatMayNeedParenthesesToParentheses.Add(parentheses.Expression, parentheses);

                expressionsThatCanCauseOthersBeingParenthesized.Add(GetExpressionOfSurroundingOperator());
            }
            else
            {
                // we don't have surrounding parentheses at all, or we have more than one parentheses which causes ambiguity
                throw new UnparserInitializationException(string.Format("Cannot automatically determine parentheses"));
            }
        }

        private void UpdateOperatorInfo(BnfTerm bnfTerm)
        {
            if (IsFlaggedOperator(bnfTerm))
                _operatorInfo = new OperatorInfo(flaggedOperator: bnfTerm);
            else if (!IsFlaggedOrDerivedOperator(bnfTerm))
                _operatorInfo = null;
        }

        private OperatorInfo GetOperatorInfo()
        {
            return _operatorInfo;
        }

        private BnfTerm GetLeftFlaggedOperatorForInside(Operator @operator, bool initialization)
        {
            return _GetFlaggedOperatorForInside(@operator, initialization, left: true);
        }

        private BnfTerm GetRightFlaggedOperatorForInside(Operator @operator, bool initialization)
        {
            return _GetFlaggedOperatorForInside(@operator, initialization, left: false);
        }

        private BnfTerm _GetFlaggedOperatorForInside(Operator @operator, bool initialization, bool left)
        {
            if (@operator == null)
                return left ? GetLeftFlaggedSurroundingOperator() : GetRightFlaggedSurroundingOperator();
            else if (@operator is Operator.Parenthesis)
                return null;
            else if (@operator is Operator.Actual)
                return GetFlaggedOperatorForInside((Operator.Actual)@operator, initialization);
            else
                throw new ArgumentException("invalid operator", "@operator");
        }

        private IReadOnlyList<UtokenBase> UnparseRawEager(UnparsableAst unparsableAst, bool initialization)
        {
            OperatorInfo operatorInfoUnused;
            return UnparseRawEager(unparsableAst, initialization, out operatorInfoUnused);
        }

        private IReadOnlyList<UtokenBase> UnparseRawEager(UnparsableAst unparsableAst, bool initialization, out OperatorInfo operatorInfo)
        {
            if (initialization)
            {
                NonTerminal nonTerminal = unparsableAst.BnfTerm as NonTerminal;

                if (nonTerminal != null)
                    RegisteringExpressionsThatNeedParentheses(nonTerminal);

                operatorInfo = null;
                return new UtokenBase[0];
            }
            else
            {
                var utokens = unparser.UnparseRaw(unparsableAst).ToList();
                UpdateOperatorInfo(unparsableAst.BnfTerm);
                operatorInfo = GetOperatorInfo();
                return utokens;
            }
        }

        private UnparsableAst GetParenthesis(ParenthesisKind parenthesisKind, UnparsableAst unparsableExpression)
        {
            BnfTerm parenthesis = parenthesisKind == ParenthesisKind.Left
                ? GetSurroundingParentheses().LeftParenthesis
                : GetSurroundingParentheses().RightParenthesis;

            return new UnparsableAst(parenthesis, unparsableExpression.AstValue);
        }

        private bool NeedParentheses(BnfTerm flaggedOperator)
        {
            BnfTerm leftFlaggedOperator = GetLeftFlaggedSurroundingOperator();
            BnfTerm rightFlaggedOperator = GetRightFlaggedSurroundingOperator();

            return leftFlaggedOperator != null &&
                ((GetPrecedence(leftFlaggedOperator) > GetPrecedence(flaggedOperator) ||
                GetPrecedence(leftFlaggedOperator) == GetPrecedence(flaggedOperator) && GetAssociativity(flaggedOperator) == Associativity.Left))
                ||
                rightFlaggedOperator != null &&
                ((GetPrecedence(rightFlaggedOperator) > GetPrecedence(flaggedOperator) ||
                GetPrecedence(rightFlaggedOperator) == GetPrecedence(flaggedOperator) && GetAssociativity(flaggedOperator) == Associativity.Right));
        }

        private BnfTerm GetFlaggedOperatorForOutside(Operator.Actual @operator, bool initialization)
        {
            return initialization
                ? this._flaggedOrDerivedOperatorToMultiOperatorInfo[@operator.FlaggedOrDerivedOperator].WeakestFlaggedOperator
                : @operator.FlaggedOperator;
        }

        private BnfTerm GetFlaggedOperatorForInside(Operator.Actual @operator, bool initialization)
        {
            return initialization
                ? this._flaggedOrDerivedOperatorToMultiOperatorInfo[@operator.FlaggedOrDerivedOperator].StrongestFlaggedOperator
                : @operator.FlaggedOperator;
        }

        private IEnumerable<Operator> GetOperators(IEnumerable<UnparsableAst> children, bool initialization)
        {
            using (ongoingOperatorGetLevel.IncrAutoDecr())
            using (ongoingOperatorUnparseLevel.IncrAutoDecr())
            {
                int parenthesesLevel = 0;

                foreach (UnparsableAst child in children)
                {
                    if (IsFlaggedOrDerivedOperator(child.BnfTerm))
                    {
                        BnfTerm flaggedOperator;
                        IEnumerable<UtokenBase> utokens;

                        if (initialization)
                        {
                            utokens = new UtokenBase[0];
                            flaggedOperator = null;
                        }
                        else
                        {
                            OperatorInfo operatorInfo;
                            utokens = UnparseRawEager(child, initialization, out operatorInfo);
                            flaggedOperator = operatorInfo.FlaggedOperator;
                        }

                        yield return new Operator.Actual(
                            flaggedOrDerivedOperator: child.BnfTerm,
                            flaggedOperator: flaggedOperator,
                            isInsideParentheses: parenthesesLevel > 0,
                            utokens: utokens
                            );
                    }
                    else if (child.BnfTerm.IsBrace())
                    {
                        if (child.BnfTerm.IsOpenBrace())
                            parenthesesLevel++;
                        else if (child.BnfTerm.IsCloseBrace())
                            parenthesesLevel--;

                        yield return new Operator.Parenthesis(child.BnfTerm, initialization ? new UtokenBase[0] : UnparseRawEager(child, initialization));
                    }
                }
            }
        }

        private static bool IsFlaggedOperator(BnfTerm bnfTerm)
        {
            return bnfTerm.IsOperator();    // has IsOperator as flag
        }

        private bool IsFlaggedOrDerivedOperator(BnfTerm bnfTerm)
        {
            return bnfTermToBnfTermKind[bnfTerm] == BnfTermKind.Operator;
        }

        private static int GetPrecedence(BnfTerm flaggedOperator)
        {
            return flaggedOperator.Precedence;
        }

        private static Associativity GetAssociativity(BnfTerm flaggedOperator)
        {
            return flaggedOperator.Associativity;
        }

        private ParenthesizedExpression GetSurroundingParentheses()
        {
            if (surroundingParentheses.Count == 0)
                throw new UnparseException("Parenthesis needed but not found");

            return surroundingParentheses.Peek();
        }

        private void BeginParenthesesContext(BnfTerm expression, bool initialization)
        {
            ParenthesizedExpression parentheses;
            if (GetMappingForParenthesesContext(initialization).TryGetValue(expression, out parentheses))
                surroundingParentheses.Push(parentheses);
        }

        private void EndParenthesesContext(BnfTerm expression, bool initialization)
        {
            if (GetMappingForParenthesesContext(initialization).ContainsKey(expression))
                surroundingParentheses.Pop();
        }

        private IDictionary<BnfTerm, ParenthesizedExpression> GetMappingForParenthesesContext(bool initialization)
        {
            return initialization ? _expressionToParentheses : expressionThatMayNeedParenthesesToParentheses;
        }

        private void BeginSurroundingOperatorsContext(BnfTerm expression, BnfTerm leftFlaggedOperator, BnfTerm rightFlaggedOperator)
        {
            surroundingOperators.Push(
                new SurroundingOperators(
                    expression,
                    leftFlaggedOperator,
                    rightFlaggedOperator
                    )
                );
        }

        private void EndSurroundingOperatorsContext()
        {
            surroundingOperators.Pop();
        }

        private BnfTerm GetExpressionOfSurroundingOperator()
        {
            return surroundingOperators.Count > 0 ? surroundingOperators.Peek().Expression : null;
        }

        private BnfTerm GetLeftFlaggedSurroundingOperator()
        {
            return surroundingOperators.Count > 0 ? surroundingOperators.Peek().LeftFlaggedOperator : null;
        }

        private BnfTerm GetRightFlaggedSurroundingOperator()
        {
            return surroundingOperators.Count > 0 ? surroundingOperators.Peek().RightFlaggedOperator : null;
        }

        #endregion

        #region Types

        private class OperatorInfo
        {
            public readonly BnfTerm FlaggedOperator;

            public OperatorInfo(BnfTerm flaggedOperator)
            {
                this.FlaggedOperator = flaggedOperator;
            }
        }

        private class SurroundingOperators
        {
            public readonly BnfTerm Expression;
            public readonly BnfTerm LeftFlaggedOperator;
            public readonly BnfTerm RightFlaggedOperator;

            public SurroundingOperators(BnfTerm expression, BnfTerm leftFlaggedOperator, BnfTerm rightFlaggedOperator)
            {
                this.Expression = expression;
                this.LeftFlaggedOperator = leftFlaggedOperator;
                this.RightFlaggedOperator = rightFlaggedOperator;
            }
        }

        private class Operator
        {
            #region Types

            public class Actual : Operator
            {
                public readonly BnfTerm FlaggedOperator;
                public readonly bool IsInsideParentheses;

                public BnfTerm FlaggedOrDerivedOperator { get { return base.BnfTerm; } }

                public Actual(BnfTerm flaggedOrDerivedOperator, BnfTerm flaggedOperator, bool isInsideParentheses, IEnumerable<UtokenBase> utokens)
                    : base(flaggedOrDerivedOperator, utokens)
                {
                    this.FlaggedOperator = flaggedOperator;
                    this.IsInsideParentheses = isInsideParentheses;
                }
            }

            public class Parenthesis : Operator
            {
                public Parenthesis(BnfTerm parenthesis, IEnumerable<UtokenBase> utokens)
                    : base(parenthesis, utokens)
                {
                }
            }

            #endregion

            public readonly BnfTerm BnfTerm;
            public readonly IEnumerable<UtokenBase> Utokens;

            public Operator(BnfTerm bnfTerm, IEnumerable<UtokenBase> utokens)
            {
                this.BnfTerm = bnfTerm;
                this.Utokens = utokens;
            }
        }

        private class MultiOperatorInfo
        {
            public readonly BnfTerm StrongestFlaggedOperator;
            public readonly BnfTerm WeakestFlaggedOperator;

            public MultiOperatorInfo(BnfTerm strongestFlaggedOperator, BnfTerm weakestFlaggedOperator)
            {
                this.StrongestFlaggedOperator = strongestFlaggedOperator;
                this.WeakestFlaggedOperator = weakestFlaggedOperator;
            }
        }

        private enum ParenthesisKind { Left, Right }

        internal enum BnfTermKind { Undetermined, Other, Operator, LeftParenthesis, RightParenthesis, GrammarHint }

        #endregion
    }

    #region ParenthesizedExpression

    public class ParenthesizedExpression : IEquatable<ParenthesizedExpression>
    {
        public BnfTerm Expression { get; internal set; }
        public BnfTerm LeftParenthesis { get; internal set; }
        public BnfTerm RightParenthesis { get; internal set; }

        internal ParenthesizedExpression() { }

        public ParenthesizedExpression(BnfTerm expression, BnfTerm leftParenthesis, BnfTerm rightParenthesis)
        {
            this.Expression = expression;
            this.LeftParenthesis = leftParenthesis;
            this.RightParenthesis = rightParenthesis;
        }

        public override bool Equals(object obj)
        {
            return obj is ParenthesizedExpression && Equals((ParenthesizedExpression)obj);
        }

        public bool Equals(ParenthesizedExpression that)
        {
            return object.ReferenceEquals(this, that)
                ||
                !object.ReferenceEquals(that, null) &&
                this.Expression == that.Expression &&
                this.LeftParenthesis == that.LeftParenthesis &&
                this.RightParenthesis == that.RightParenthesis;
        }

        public override int GetHashCode()
        {
            return Util.GetHashCodeMulti(Expression, LeftParenthesis, RightParenthesis);
        }

        public static bool operator ==(ParenthesizedExpression parentheses1, ParenthesizedExpression parentheses2)
        {
            return object.ReferenceEquals(parentheses1, parentheses2) || !object.ReferenceEquals(parentheses1, null) && parentheses1.Equals(parentheses2);
        }

        public static bool operator !=(ParenthesizedExpression parentheses1, ParenthesizedExpression parentheses2)
        {
            return !(parentheses1 == parentheses2);
        }
    }

    #endregion
}
