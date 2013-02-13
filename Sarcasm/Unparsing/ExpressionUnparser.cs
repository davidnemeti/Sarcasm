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
using Sarcasm.Ast;

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

                public Actual(BnfTerm flaggedOrDerivedOperator, BnfTerm flaggedOperator, bool isInsideParentheses, IEnumerable<Utoken> utokens)
                    : base(flaggedOrDerivedOperator, utokens)
                {
                    this.FlaggedOperator = flaggedOperator;
                    this.IsInsideParentheses = isInsideParentheses;
                }
            }

            public class Parenthesis : Operator
            {
                public Parenthesis(BnfTerm parenthesis, IEnumerable<Utoken> utokens)
                    : base(parenthesis, utokens)
                {
                }
            }

            #endregion

            public readonly BnfTerm BnfTerm;
            public readonly IEnumerable<Utoken> Utokens;

            public Operator(BnfTerm bnfTerm, IEnumerable<Utoken> utokens)
            {
                this.BnfTerm = bnfTerm;
                this.Utokens = utokens;
            }
        }

        private class Parentheses : IEquatable<Parentheses>
        {
            public BnfTerm Expression;
            public BnfTerm Left;
            public BnfTerm Right;

            public override bool Equals(object obj)
            {
                return obj is Parentheses && Equals((Parentheses)obj);
            }

            public bool Equals(Parentheses that)
            {
                return object.ReferenceEquals(this, that)
                    ||
                    that != null &&
                    this.Expression == that.Expression && 
                    this.Left == that.Left &&
                    this.Right == that.Right;
            }

            public override int GetHashCode()
            {
                return Util.GetHashCodeMulti(Expression, Left, Right);
            }

            public static bool operator==(Parentheses parentheses1, Parentheses parentheses2)
            {
                return object.ReferenceEquals(parentheses1, parentheses2) || !object.ReferenceEquals(parentheses1, null) && parentheses1.Equals(parentheses2);
            }

            public static bool operator !=(Parentheses parentheses1, Parentheses parentheses2)
            {
                return !(parentheses1 == parentheses2);
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

        #region State

        #region Immutable after initialization

        private readonly Unparser unparser;

        private ISet<BnfTerm> expressionsThatCanCauseOthersBeingParenthesized;
        private IDictionary<BnfTerm, Parentheses> expressionThatMayNeedParenthesesToParentheses;
        private IDictionary<BnfTerm, Parentheses> expressionToParentheses;
        private IDictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKind;
        private IDictionary<BnfTerm, MultiOperatorInfo> flaggedOrDerivedOperatorToMultiOperatorInfo;
        private Bag<BnfTerm> examinedBnfTermsInCurrentPath;

        #endregion

        #region Mutable

        private Stack<SurroundingOperators> surroundingOperators = new Stack<SurroundingOperators>();
        private Stack<Parentheses> surroundingParentheses = new Stack<Parentheses>();
        private OperatorInfo _operatorInfo = null;
        private int ongoingExpressionUnparseLevel = 0;
        private int ongoingOperatorUnparseLevel = 0;

        #endregion

        #endregion

        #region Construction

        public ExpressionUnparser(Unparser unparser)
        {
            this.unparser = unparser;

            InitParentheses();
            ongoingExpressionUnparseLevel = 0;
        }

        #endregion

        #region Initialization

        private void InitParentheses()
        {
            DetermineOperatorsParenthesesExpressions();
            RegisteringExpressionsThatNeedParentheses();

            tsParentheses.Debug("");
            tsParentheses.Debug("----------------------------------------");
            tsParentheses.Debug("");
            tsParentheses.Debug("expressions:");
            tsParentheses.Debug("");

#if DEBUG
            foreach (var pair in expressionThatMayNeedParenthesesToParentheses)
                tsParentheses.Debug("{0}; left parenthesis: {1}, right parenthesis: {2}", pair.Key, pair.Value.Left, pair.Value.Right);
#endif
        }

        private void DetermineOperatorsParenthesesExpressions()
        {
            this.expressionToParentheses = new Dictionary<BnfTerm, Parentheses>();
            this.bnfTermToBnfTermKind = new Dictionary<BnfTerm, BnfTermKind>();
            this.flaggedOrDerivedOperatorToMultiOperatorInfo = new Dictionary<BnfTerm, MultiOperatorInfo>();

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
            {
                bool? nonTerminalLooksLikeAnOperator = null;
                bool? nonTerminalLooksLikeALeftParenthesis = null;
                bool? nonTerminalLooksLikeARightParenthesis = null;

                var childOperators = new List<BnfTerm>();

                foreach (BnfTermList children in Unparser.GetChildBnfTermLists((NonTerminal)current))
                {
                    int operatorCount = 0;
                    int leftParenthesisCount = 0;
                    int rightParenthesisCount = 0;
                    int otherCount = 0;

                    /*
                     * NOTE: we should not read bnfTermToBnfTermInfo in this method (because of recursive definitions in grammar),
                     * that's why we store BnfTermInfo as well
                     * */
                    BnfTerm prevChild = null;
                    BnfTermKind? prevChildKind = null;
                    Parentheses parentheses = null;
                    BnfTerm childExpression = null;

                    if (children.Count == 0)
                        continue;

                    BnfTerm childOperator = null;

                    foreach (BnfTerm child in children)
                    {
                        BnfTermKind childKind = CalculateBnfTermKind(child);

                        Debug.Assert(childKind != BnfTermKind.Undetermined);

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
                                parentheses = new Parentheses() { Left = child };
                                childExpression = null;
                                break;

                            case BnfTermKind.RightParenthesis:
                                rightParenthesisCount++;
                                if (prevChildKind == BnfTermKind.Other && childExpression == prevChild)
                                {
                                    Debug.Assert(parentheses != null && parentheses.Left != null && parentheses.Expression != null);

                                    parentheses.Right = child;

                                    Parentheses registeredParentheses;
                                    if (expressionToParentheses.TryGetValue(childExpression, out registeredParentheses))
                                    {
                                        if (parentheses != registeredParentheses)
                                            expressionToParentheses[childExpression] = null;  // ambiguous parentheses -> set to null, and check later (we might not need the parentheses at all)
                                    }
                                    else
                                        expressionToParentheses.Add(childExpression, parentheses);
                                }
                                childExpression = null;
                                parentheses = null;
                                break;

                            case BnfTermKind.Other:
                                otherCount++;
                                if (prevChildKind == BnfTermKind.LeftParenthesis)
                                {
                                    Debug.Assert(parentheses != null && parentheses.Left != null);
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

                        if (childKind != BnfTermKind.GrammarHint)
                        {
                            prevChild = child;
                            prevChildKind = childKind;
                        }
                    }

                    bool childBnfTermListLooksLikeAnOperator = operatorCount == 1 && leftParenthesisCount == 0 && rightParenthesisCount == 0 && otherCount == 0;
                    bool childBnfTermListLooksLikeALeftParenthesis = leftParenthesisCount == 1 && operatorCount == 0 && rightParenthesisCount == 0 && otherCount == 0;
                    bool childBnfTermListLooksLikeARightParenthesis = rightParenthesisCount == 1 && operatorCount == 0 && leftParenthesisCount == 0 && otherCount == 0;

                    if (childBnfTermListLooksLikeAnOperator)
                        childOperators.Add(childOperator);

                    nonTerminalLooksLikeAnOperator = (nonTerminalLooksLikeAnOperator ?? true) && childBnfTermListLooksLikeAnOperator;
                    nonTerminalLooksLikeALeftParenthesis = (nonTerminalLooksLikeALeftParenthesis ?? true) && childBnfTermListLooksLikeALeftParenthesis;
                    nonTerminalLooksLikeARightParenthesis = (nonTerminalLooksLikeARightParenthesis ?? true) && childBnfTermListLooksLikeARightParenthesis;
                }

                if (nonTerminalLooksLikeAnOperator == true)
                {
                    currentKind = BnfTermKind.Operator;

                    BnfTerm strongestFlaggedChildOperator = childOperators.Aggregate(
                        (BnfTerm)null,
                        (strongestFlaggedChildOperatorSoFar, childOperator) => strongestFlaggedChildOperatorSoFar == null ||
                            GetPrecedence(flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].StrongestFlaggedOperator) > GetPrecedence(flaggedOrDerivedOperatorToMultiOperatorInfo[strongestFlaggedChildOperatorSoFar].StrongestFlaggedOperator)
                            ? flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].StrongestFlaggedOperator
                            : strongestFlaggedChildOperatorSoFar
                        );

                    BnfTerm weakestFlaggedChildOperator = childOperators.Aggregate(
                        (BnfTerm)null,
                        (weakestFlaggedChildOperatorSoFar, childOperator) => weakestFlaggedChildOperatorSoFar == null ||
                            GetPrecedence(flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].WeakestFlaggedOperator) > GetPrecedence(flaggedOrDerivedOperatorToMultiOperatorInfo[weakestFlaggedChildOperatorSoFar].WeakestFlaggedOperator)
                            ? flaggedOrDerivedOperatorToMultiOperatorInfo[childOperator].WeakestFlaggedOperator
                            : weakestFlaggedChildOperatorSoFar
                        );

                    flaggedOrDerivedOperatorToMultiOperatorInfo.Add(current, new MultiOperatorInfo(strongestFlaggedChildOperator, weakestFlaggedChildOperator));
                }
                else if (nonTerminalLooksLikeALeftParenthesis == true)
                    currentKind = BnfTermKind.LeftParenthesis;
                else if (nonTerminalLooksLikeARightParenthesis == true)
                    currentKind = BnfTermKind.RightParenthesis;
                else
                    currentKind = BnfTermKind.Other;
            }
            else
                currentKind = BnfTermKind.Other;

            // e.g. operator can be non-terminal and terminal as well (or at least they can be flagged so); if it is non-terminal, then we override bnfTermInfo
            if (IsFlaggedOperator(current))
            {
                currentKind = BnfTermKind.Operator;

                flaggedOrDerivedOperatorToMultiOperatorInfo[current] = new MultiOperatorInfo(current, current);
            }
            else if (current.IsOpenBrace())
                currentKind = BnfTermKind.LeftParenthesis;
            else if (current.IsCloseBrace())
                currentKind = BnfTermKind.RightParenthesis;
            else if (current is GrammarHint)
                currentKind = BnfTermKind.GrammarHint;

            tsParentheses.Unindent();
            currentKind.DebugWriteLineBnfTermKind(tsParentheses, current);

            Debug.Assert(currentKind != BnfTermKind.Undetermined);

            this.bnfTermToBnfTermKind[current] = currentKind;        // not using Add, because bnfTermToBnfTermKind[current] has already been set to Undetermined or Other

            return currentKind;
        }

        private void RegisteringExpressionsThatNeedParentheses()
        {
            this.expressionsThatCanCauseOthersBeingParenthesized = new HashSet<BnfTerm>();
            this.expressionThatMayNeedParenthesesToParentheses = new Dictionary<BnfTerm, Parentheses>();
            this.examinedBnfTermsInCurrentPath = new Bag<BnfTerm>();

            RegisteringExpressionsThatNeedParentheses(unparser.Grammar.Root);
        }

        private void RegisteringExpressionsThatNeedParentheses(NonTerminal nonTerminal)
        {
            foreach (var childBnfTerms in Unparser.GetChildBnfTermLists(nonTerminal))
            {
                Unparse(
                    new UnparsableObject(nonTerminal, obj: null),
                    childBnfTerms.Select(childBnfTerm => new UnparsableObject(childBnfTerm, obj: null)),
                    simulation: true
                    );
            }
        }

        #endregion

        #region Unparse

        public bool NeedsExpressionUnparse(BnfTerm bnfTerm)
        {
            return ongoingExpressionUnparseLevel > 0 || expressionsThatCanCauseOthersBeingParenthesized.Contains(bnfTerm);
        }

        public IReadOnlyList<Utoken> Unparse(UnparsableObject unparsableObject, IEnumerable<UnparsableObject> chosenChildren)
        {
            ongoingExpressionUnparseLevel++;

            try
            {
                return Unparse(unparsableObject, chosenChildren, simulation: false);
            }
            finally
            {
                ongoingExpressionUnparseLevel--;
            }
        }

        private IReadOnlyList<Utoken> Unparse(UnparsableObject unparsableObject, IEnumerable<UnparsableObject> chosenChildren, bool simulation)
        {
            if (ongoingOperatorUnparseLevel > 0)
            {
                var utokens = chosenChildren.SelectMany(chosenChild => UnparseRawEager(chosenChild, simulation)).ToList();
                UpdateOperatorInfo(unparsableObject.bnfTerm);
                return utokens;
            }

            if (simulation && examinedBnfTermsInCurrentPath.GetCount(unparsableObject.bnfTerm) == 2)
            {
                UpdateOperatorInfo(unparsableObject.bnfTerm);
                return new Utoken[0];   // we have already expanded the current bnfTerm recursively twice during the current path, so no need to go further
            }

            if (simulation)
                examinedBnfTermsInCurrentPath.Add(unparsableObject.bnfTerm);

            BeginParenthesesContext(unparsableObject.bnfTerm, simulation);

            try
            {
                var utokens = new List<Utoken>();

                chosenChildren = chosenChildren.ToList();   // we are going to read the children twice, so we enforce them to be fully loaded into a list

                IReadOnlyList<Operator> operators = GetOperators(chosenChildren, simulation).ToList();

                bool needParentheses =
                    operators
                        .OfType<Operator.Actual>()
                        .Any(
                            @operator => !@operator.IsInsideParentheses &&
                            NeedParentheses(GetFlaggedOperatorForOutside(@operator, simulation))
                            );

                if (needParentheses && simulation)
                {
                    Parentheses parentheses = GetSurroundingParentheses();
                    Parentheses registeredParentheses;
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
                        throw new UnparserInitializationException(string.Format("Cannot automatically determine parentheses for expression '{0}'", unparsableObject.bnfTerm));
                    }
                }

                if (needParentheses && !simulation)
                    utokens.AddRange(UnparseParenthesis(ParenthesisKind.Left, unparsableObject));

                using (var childEnumerator = chosenChildren.GetEnumerator())
                {
                    Operator prevOperator = null;

                    foreach (Operator @operator in operators.Concat(new Operator[] { null }))    // extra 'null' element for the inner 'while' cycle to finish
                    {
                        BeginSurroundingOperatorsContext(
                            unparsableObject.bnfTerm,
                            GetLeftFlaggedOperatorForInside(prevOperator, simulation),
                            GetRightFlaggedOperatorForInside(@operator, simulation)
                            );

                        try
                        {
                            while (childEnumerator.MoveNext() && (@operator == null || childEnumerator.Current.bnfTerm != @operator.BnfTerm))
                                utokens.AddRange(UnparseRawEager(childEnumerator.Current, simulation));
                        }
                        finally
                        {
                            EndSurroundingOperatorsContext();
                        }

                        if (@operator != null)
                        {
                            ongoingOperatorUnparseLevel++;
                            try
                            {
                                utokens.AddRange(UnparseRawEager(childEnumerator.Current, simulation));
                            }
                            finally
                            {
                                ongoingOperatorUnparseLevel--;
                            }

                            // NOTE that we cannot use the utokens that we may get by GetOperators, because real unparse needs its context to be fully unparsed (see: after, before, between insertions)

                            // NOTE that we begin with a childEnumerator.MoveNext() call in the next iteration for the next operator, so we won't unparse the current operator twice
                        }

                        prevOperator = @operator;
                    }
                }

                if (needParentheses && !simulation)
                    utokens.AddRange(UnparseParenthesis(ParenthesisKind.Right, unparsableObject));

                UpdateOperatorInfo(unparsableObject.bnfTerm);

                return utokens;
            }
            finally
            {
                EndParenthesesContext(unparsableObject.bnfTerm, simulation);

                if (simulation)
                    examinedBnfTermsInCurrentPath.Remove(unparsableObject.bnfTerm);
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

        private BnfTerm GetLeftFlaggedOperatorForInside(Operator @operator, bool simulation)
        {
            return _GetFlaggedOperatorForInside(@operator, simulation, left: true);
        }

        private BnfTerm GetRightFlaggedOperatorForInside(Operator @operator, bool simulation)
        {
            return _GetFlaggedOperatorForInside(@operator, simulation, left: false);
        }

        private BnfTerm _GetFlaggedOperatorForInside(Operator @operator, bool simulation, bool left)
        {
            if (@operator == null)
                return left ? GetLeftFlaggedSurroundingOperator() : GetRightFlaggedSurroundingOperator();
            else if (@operator is Operator.Parenthesis)
                return null;
            else if (@operator is Operator.Actual)
                return GetFlaggedOperatorForInside((Operator.Actual)@operator, simulation);
            else
                throw new ArgumentException("invalid @operator", "@operator");
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject, bool simulation)
        {
            OperatorInfo operatorInfoUnused;
            return UnparseRawEager(unparsableObject, simulation, out operatorInfoUnused);
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject, bool simulation, out OperatorInfo operatorInfo)
        {
            if (simulation)
            {
                NonTerminal nonTerminal = unparsableObject.bnfTerm as NonTerminal;

                if (nonTerminal != null)
                    RegisteringExpressionsThatNeedParentheses(nonTerminal);

                operatorInfo = null;
                return new Utoken[0];
            }
            else
            {
                var utokens = unparser.UnparseRaw(unparsableObject).ToList();
                UpdateOperatorInfo(unparsableObject.bnfTerm);
                operatorInfo = GetOperatorInfo();
                return utokens;
            }
        }

        private IEnumerable<Utoken> UnparseParenthesis(ParenthesisKind parenthesisKind, UnparsableObject unparsableExpression)
        {
            try
            {
                BnfTerm parenthesis = parenthesisKind == ParenthesisKind.Left
                    ? GetSurroundingParentheses().Left
                    : GetSurroundingParentheses().Right;

                return UnparseRawEager(new UnparsableObject(parenthesis, unparsableExpression.obj), simulation: false);
            }
            catch (UnparseException e)
            {
                throw new UnparseException(e.Message + " for " + unparsableExpression.bnfTerm.ToString());
            }
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

        private BnfTerm GetFlaggedOperatorForOutside(Operator.Actual @operator, bool simulation)
        {
            return simulation
                ? this.flaggedOrDerivedOperatorToMultiOperatorInfo[@operator.FlaggedOrDerivedOperator].WeakestFlaggedOperator
                : @operator.FlaggedOperator;
        }

        private BnfTerm GetFlaggedOperatorForInside(Operator.Actual @operator, bool simulation)
        {
            return simulation
                ? this.flaggedOrDerivedOperatorToMultiOperatorInfo[@operator.FlaggedOrDerivedOperator].StrongestFlaggedOperator
                : @operator.FlaggedOperator;
        }

        private IEnumerable<Operator> GetOperators(IEnumerable<UnparsableObject> children, bool simulation)
        {
            ongoingOperatorUnparseLevel++;

            try
            {
                int parenthesesLevel = 0;

                foreach (UnparsableObject child in children)
                {
                    if (IsFlaggedOrDerivedOperator(child.bnfTerm))
                    {
                        BnfTerm flaggedOperator;
                        IEnumerable<Utoken> utokens;

                        if (simulation)
                        {
                            utokens = new Utoken[0];
                            flaggedOperator = null;
                        }
                        else
                        {
                            OperatorInfo operatorInfo;
                            utokens = UnparseRawEager(child, simulation, out operatorInfo);
                            flaggedOperator = operatorInfo.FlaggedOperator;
                        }

                        yield return new Operator.Actual(
                            flaggedOrDerivedOperator: child.bnfTerm,
                            flaggedOperator: flaggedOperator,
                            isInsideParentheses: parenthesesLevel > 0,
                            utokens: utokens
                            );
                    }
                    else if (child.bnfTerm.IsBrace())
                    {
                        if (child.bnfTerm.IsOpenBrace())
                            parenthesesLevel++;
                        else if (child.bnfTerm.IsCloseBrace())
                            parenthesesLevel--;

                        yield return new Operator.Parenthesis(child.bnfTerm, simulation ? new Utoken[0] : UnparseRawEager(child, simulation));
                    }
                }
            }
            finally
            {
                ongoingOperatorUnparseLevel--;
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

        private Parentheses GetSurroundingParentheses()
        {
            if (surroundingParentheses.Count == 0)
                throw new UnparseException("Parenthesis needed but not found");

            return surroundingParentheses.Peek();
        }

        private void BeginParenthesesContext(BnfTerm expression, bool simulation)
        {
            Parentheses parentheses;
            if (GetMappingForParenthesesContext(simulation).TryGetValue(expression, out parentheses))
                surroundingParentheses.Push(parentheses);
        }

        private void EndParenthesesContext(BnfTerm expression, bool simulation)
        {
            if (GetMappingForParenthesesContext(simulation).ContainsKey(expression))
                surroundingParentheses.Pop();
        }

        private IDictionary<BnfTerm, Parentheses> GetMappingForParenthesesContext(bool simulation)
        {
            return simulation ? expressionToParentheses : expressionThatMayNeedParenthesesToParentheses;
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
    }
}
