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

        private class UnparseInfo
        {
            public readonly BnfTerm RegisteredOperator;

            private UnparseInfo(BnfTerm registeredOperator)
            {
                this.RegisteredOperator = registeredOperator;
            }

            public static UnparseInfo CreateOperator(BnfTerm registeredOperator)
            {
                return new UnparseInfo(registeredOperator);
            }
        }

        private class SurroundingOperators
        {
            public readonly BnfTerm LeftRegisteredOperator;
            public readonly BnfTerm RightRegisteredOperator;

            public SurroundingOperators(BnfTerm leftRegisteredOperator, BnfTerm rightRegisteredOperator)
            {
                this.LeftRegisteredOperator = leftRegisteredOperator;
                this.RightRegisteredOperator = rightRegisteredOperator;
            }
        }

        private class UnparsedOperator
        {
            public readonly UnparsableObject UnparsableObject;
            public readonly BnfTerm RegisteredOperator;
            public readonly IEnumerable<Utoken> Utokens;

            public UnparsedOperator(UnparsableObject unparsableObject, BnfTerm registeredOperator, IEnumerable<Utoken> utokens)
            {
                this.UnparsableObject = unparsableObject;
                this.RegisteredOperator = registeredOperator;
                this.Utokens = utokens;
            }
        }

        private class Parentheses
        {
            public BnfTerm Left;
            public BnfTerm Right;
        }

        internal enum BnfTermKind { Other, Operator, ExpressionWithOperator, LeftParenthesis, RightParenthesis, Literal, GrammarHint }

        #endregion

        #region State

        private readonly Unparser unparser;

        private ISet<BnfTerm> bnfTermsSeenForCalculation = new HashSet<BnfTerm>();
        private IDictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKind = new Dictionary<BnfTerm, BnfTermKind>();
        private IDictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKindHint = new Dictionary<BnfTerm, BnfTermKind>();
        private IDictionary<BnfTerm, Parentheses> expressionToParentheses = new Dictionary<BnfTerm, Parentheses>();
        private Stack<SurroundingOperators> surroundingOperators = new Stack<SurroundingOperators>();
        private Stack<Parentheses> surroundingParentheses = new Stack<Parentheses>();
        private UnparseInfo _unparseInfo = null;

        #endregion

        #region Construction

        public ExpressionUnparser(Unparser unparser)
        {
            this.unparser = unparser;

            InitParentheses();
        }

        #endregion

        #region Initialization

        private void InitParentheses()
        {
            CalculateBnfTermKind(unparser.Grammar.Root);
            CheckParentheses();

            tsParentheses.Debug("");
            tsParentheses.Debug("----------------------------------------");
            tsParentheses.Debug("");
            tsParentheses.Debug("expressions:");
            tsParentheses.Debug("");

#if DEBUG
            foreach (var pair in this.bnfTermToBnfTermKind)
            {
                if (!(pair.Key is BnfiTermMember) && pair.Value == BnfTermKind.ExpressionWithOperator)
                    pair.Value.DebugWriteLineBnfTermKind(tsParentheses, pair.Key);
            }
#endif
        }

        private void CheckParentheses()
        {
            foreach (var pair in expressionToParentheses)
            {
                if (pair.Value.Left == null)
                    throw new UnparseException(string.Format("Left parenthesis is missing for expression '{0}'", pair.Key));

                if (pair.Value.Right == null)
                    throw new UnparseException(string.Format("Right parenthesis is missing for expression '{0}'", pair.Key));
            }
        }

        private BnfTermKind CalculateBnfTermKind(BnfTerm bnfTerm)
        {
            if (bnfTermToBnfTermKind.ContainsKey(bnfTerm))
                return bnfTermToBnfTermKind[bnfTerm].DebugWriteLineBnfTermKind(tsParentheses, bnfTerm, " (already calculated)");
            else if (bnfTermToBnfTermKindHint.ContainsKey(bnfTerm))
                return bnfTermToBnfTermKindHint[bnfTerm].DebugWriteLineBnfTermKind(tsParentheses, bnfTerm, " (already calculated hint)");
            else if (bnfTermsSeenForCalculation.Contains(bnfTerm))
            {
                // expressions can be recursively defined, so let's assume that it's an expression, then check for it at the end
                BnfTermKind _bnfTermKindHint = BnfTermKind.ExpressionWithOperator;
                bnfTermToBnfTermKindHint.Add(bnfTerm, _bnfTermKindHint);
                return _bnfTermKindHint.DebugWriteLineBnfTermKind(tsParentheses, bnfTerm, " (hint)");
            }

            tsParentheses.Indent();

            bnfTermsSeenForCalculation.Add(bnfTerm);

            BnfTermKind bnfTermKind;

            // need to "label" every bnfterm
            if (bnfTerm is NonTerminal)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;

                bool? nonTerminalLooksLikeAnOperator = null;
                bool? nonTerminalLooksLikeAnExpressionWithOperator = null;

                foreach (BnfTermList childBnfTermList in Unparser.GetChildBnfTermLists(nonTerminal))
                {
                    bool expressionWithOperatorExists = false;
                    bool operatorExists = false;
                    bool literalExists = false;
                    bool parenthesisExists = false;
                    bool otherExists = false;

                    /*
                     * NOTE: we should not read bnfTermToBnfTermInfo in this method (because of recursive definitions in grammar),
                     * that's why we store BnfTermInfo as well
                     * */
                    BnfTerm prevChildBnfTerm = null;
                    BnfTermKind? prevChildBnfTermKind = null;

                    if (childBnfTermList.Count == 0)
                        continue;

                    foreach (BnfTerm childBnfTerm in childBnfTermList)
                    {
                        BnfTermKind childBnfTermKind = CalculateBnfTermKind(childBnfTerm);

                        switch (childBnfTermKind)
                        {
                            case BnfTermKind.Operator:
                                operatorExists = true;
                                break;

                            case BnfTermKind.ExpressionWithOperator:
                                if (prevChildBnfTermKind != null && prevChildBnfTermKind == BnfTermKind.LeftParenthesis)
                                    SetLeftParenthesisForExpression(childBnfTerm, prevChildBnfTerm);

                                expressionWithOperatorExists = true;
                                break;

                            case BnfTermKind.Literal:
                                literalExists = true;
                                break;

                            case BnfTermKind.LeftParenthesis:
                                parenthesisExists = true;
                                break;

                            case BnfTermKind.RightParenthesis:
                                if (prevChildBnfTermKind != null && prevChildBnfTermKind == BnfTermKind.ExpressionWithOperator)
                                    SetRightParenthesisForExpression(prevChildBnfTerm, childBnfTerm);

                                parenthesisExists = true;
                                break;

                            case BnfTermKind.Other:
                                otherExists = true;
                                break;
                        }

                        if (childBnfTermKind != BnfTermKind.GrammarHint)
                        {
                            prevChildBnfTerm = childBnfTerm;
                            prevChildBnfTermKind = childBnfTermKind;
                        }
                    }

                    bool childBnfTermListLooksLikeAnExpressionWithOperator = operatorExists || expressionWithOperatorExists && !literalExists && !otherExists;
                    bool childBnfTermListLooksLikeAnOperator = operatorExists && !expressionWithOperatorExists && !literalExists && !parenthesisExists && !otherExists;

                    nonTerminalLooksLikeAnExpressionWithOperator = (nonTerminalLooksLikeAnExpressionWithOperator ?? false) || childBnfTermListLooksLikeAnExpressionWithOperator;
                    nonTerminalLooksLikeAnOperator = (nonTerminalLooksLikeAnOperator ?? false) || childBnfTermListLooksLikeAnOperator;
                }

                if (nonTerminalLooksLikeAnOperator == true)
                    bnfTermKind = BnfTermKind.Operator;
                else if (nonTerminalLooksLikeAnExpressionWithOperator == true)
                    bnfTermKind = BnfTermKind.ExpressionWithOperator;
                else
                    bnfTermKind = BnfTermKind.Other;
            }
            else
                bnfTermKind = BnfTermKind.Other;

            // e.g. operator can be non-terminal and terminal as well; if it is non-terminal, then we override bnfTermInfo
            if (bnfTerm.IsOperator())
                bnfTermKind = BnfTermKind.Operator;
            else if (bnfTerm.IsOpenBrace())
                bnfTermKind = BnfTermKind.LeftParenthesis;
            else if (bnfTerm.IsCloseBrace())
                bnfTermKind = BnfTermKind.RightParenthesis;
            else if (bnfTerm.Flags.HasFlag(TermFlags.IsLiteral) || bnfTerm.Flags.HasFlag(TermFlags.IsConstant))
                bnfTermKind = BnfTermKind.Literal;
            else if (bnfTerm is GrammarHint)
                bnfTermKind = BnfTermKind.GrammarHint;

            BnfTermKind bnfTermKindHint;
            if (!bnfTermToBnfTermKindHint.TryGetValue(bnfTerm, out bnfTermKindHint) || bnfTermKind == bnfTermKindHint)
                bnfTermToBnfTermKind.Add(bnfTerm, bnfTermKind);
            else
                bnfTermToBnfTermKind.Add(bnfTerm, BnfTermKind.Other);  // our assumption was wrong -> it is "Other"

            bnfTermToBnfTermKindHint.Remove(bnfTerm);

            tsParentheses.Unindent();
            bnfTermKind.DebugWriteLineBnfTermKind(tsParentheses, bnfTerm);

            return bnfTermKind;
        }

        #endregion

        #region Unparse

        public void UpdateUnparseInfo(BnfTerm bnfTerm)
        {
            if (IsRegisteredOperator(bnfTerm))
                _unparseInfo = UnparseInfo.CreateOperator(registeredOperator: bnfTerm);
            else if (!IsOperator(bnfTerm))
                _unparseInfo = null;
        }

        public bool IsExpressionWithOperator(BnfTerm bnfTerm)
        {
            return bnfTermToBnfTermKind[bnfTerm] == BnfTermKind.ExpressionWithOperator;
        }

        public IReadOnlyList<Utoken> Unparse(UnparsableObject unparsableObject, IEnumerable<UnparsableObject> chosenChildren)
        {
            var utokens = new List<Utoken>();

            BeginParenthesesContext(unparsableObject.bnfTerm);

            List<UnparsedOperator> unparsedOperators = GetUnparsedOperators(chosenChildren).ToList();

            if (unparsedOperators.Count > 0)
                utokens.AddRange(LeftParenthesesIfNeeded(unparsableObject, unparsedOperators[0]));

            using (var childEnumerator = chosenChildren.GetEnumerator())
            {
                UnparsedOperator prevUnparsedOperator = null;
                foreach (UnparsedOperator unparsedOperator in unparsedOperators.Concat(new UnparsedOperator[] { null }))    // extra 'null' element for the inner 'while' cycle to finish
                {
                    BeginSurroundingOperatorsContext(leftUnparsedOperator: prevUnparsedOperator, rightUnparsedOperator: unparsedOperator);

                    while (childEnumerator.MoveNext() && (unparsedOperator == null || childEnumerator.Current != unparsedOperator.UnparsableObject))
                        utokens.AddRange(UnparseRawEager(childEnumerator.Current));

                    EndSurroundingOperatorsContext();

                    prevUnparsedOperator = unparsedOperator;
                }
            }

            if (unparsedOperators.Count > 0)
                utokens.AddRange(RightParenthesesIfNeeded(unparsableObject, unparsedOperators[unparsedOperators.Count - 1]));

            EndParenthesesContext(unparsableObject.bnfTerm);

            return utokens;
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject)
        {
            UnparseInfo unparseInfoUnused;
            return UnparseRawEager(unparsableObject, out unparseInfoUnused);
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject, out UnparseInfo unparseInfo)
        {
            var utokens = unparser.UnparseRaw(unparsableObject).ToList();
            unparseInfo = this._unparseInfo;
            return utokens;
        }

        private IEnumerable<Utoken> LeftParenthesesIfNeeded(UnparsableObject unparsableExpression, UnparsedOperator unparsedOperator)
        {
            try
            {
                if (NeedParentheses(unparsableExpression.bnfTerm, unparsedOperator))
                    return UnparseRawEager(new UnparsableObject(GetLeftSurroundingParenthesis(), unparsableExpression.obj));
                else
                    return new Utoken[0];
            }
            catch (UnparseException e)
            {
                throw new UnparseException(e.Message + " for " + unparsableExpression.bnfTerm.ToString());
            }
        }

        private IEnumerable<Utoken> RightParenthesesIfNeeded(UnparsableObject unparsableExpression, UnparsedOperator unparsedOperator)
        {
            try
            {
                if (NeedParentheses(unparsableExpression.bnfTerm, unparsedOperator))
                    return UnparseRawEager(new UnparsableObject(GetRightSurroundingParenthesis(), unparsableExpression.obj));
                else
                    return new Utoken[0];
            }
            catch (UnparseException e)
            {
                throw new UnparseException(e.Message + " for " + unparsableExpression.bnfTerm.ToString());
            }
        }

        private bool NeedParentheses(BnfTerm expression, UnparsedOperator unparsedOperator)
        {
            if (unparsedOperator == null)
                return false;

            BnfTerm registeredOperator = unparsedOperator.RegisteredOperator;
            BnfTerm leftRegisteredOperator = GetLeftRegisteredSurroundingOperator();
            BnfTerm rightRegisteredOperator = GetRightRegisteredSurroundingOperator();

            return leftRegisteredOperator != null &&
                ((GetPrecedence(leftRegisteredOperator) > GetPrecedence(registeredOperator) ||
                GetPrecedence(leftRegisteredOperator) == GetPrecedence(registeredOperator) && GetAssociativity(registeredOperator) == Associativity.Left))
                ||
                rightRegisteredOperator != null &&
                ((GetPrecedence(rightRegisteredOperator) > GetPrecedence(registeredOperator) ||
                GetPrecedence(rightRegisteredOperator) == GetPrecedence(registeredOperator) && GetAssociativity(registeredOperator) == Associativity.Right));
        }

        private IEnumerable<UnparsedOperator> GetUnparsedOperators(IEnumerable<UnparsableObject> children)
        {
            foreach (UnparsableObject child in children)
            {
                if (IsOperator(child.bnfTerm))
                {
                    UnparseInfo unparseInfo;
                    var utokens = UnparseRawEager(child, out unparseInfo);
                    yield return new UnparsedOperator(child, unparseInfo.RegisteredOperator, utokens);
                }
            }
        }

        private static bool IsRegisteredOperator(BnfTerm bnfTerm)
        {
            return bnfTerm.IsOperator();
        }

        private bool IsOperator(BnfTerm bnfTerm)
        {
            return bnfTermToBnfTermKind[bnfTerm] == BnfTermKind.Operator;
        }

        private int GetPrecedence(BnfTerm registeredOperator)
        {
            return registeredOperator.Precedence;
        }

        private Associativity GetAssociativity(BnfTerm registeredOperator)
        {
            return registeredOperator.Associativity;
        }

        private BnfTerm GetLeftSurroundingParenthesis()
        {
            if (surroundingParentheses.Count == 0)
                throw new UnparseException("Parenthesis needed but not found");

            return surroundingParentheses.Peek().Left;
        }

        private BnfTerm GetRightSurroundingParenthesis()
        {
            if (surroundingParentheses.Count == 0)
                throw new UnparseException("Parenthesis needed but not found");

            return surroundingParentheses.Peek().Right;
        }

        private void SetLeftParenthesisForExpression(BnfTerm expression, BnfTerm leftParenthesis)
        {
            GetEnforcedParenthesisForExpression(expression).Left = leftParenthesis;
        }

        private void SetRightParenthesisForExpression(BnfTerm expression, BnfTerm rightParenthesis)
        {
            GetEnforcedParenthesisForExpression(expression).Right = rightParenthesis;
        }

        private Parentheses GetEnforcedParenthesisForExpression(BnfTerm expression)
        {
            Parentheses parentheses;
            if (!expressionToParentheses.TryGetValue(expression, out parentheses))
                parentheses = expressionToParentheses[expression] = new Parentheses();

            return parentheses;
        }

        private void BeginParenthesesContext(BnfTerm expression)
        {
            Parentheses parentheses;
            if (expressionToParentheses.TryGetValue(expression, out parentheses))
                surroundingParentheses.Push(parentheses);
        }

        private void EndParenthesesContext(BnfTerm expression)
        {
            if (expressionToParentheses.ContainsKey(expression))
                surroundingParentheses.Pop();
        }

        private void BeginSurroundingOperatorsContext(UnparsedOperator leftUnparsedOperator, UnparsedOperator rightUnparsedOperator)
        {
            BnfTerm leftRegisteredOperator = leftUnparsedOperator != null ? leftUnparsedOperator.RegisteredOperator : null;
            BnfTerm rightRegisteredOperator = rightUnparsedOperator != null ? rightUnparsedOperator.RegisteredOperator : null;

            surroundingOperators.Push(
                new SurroundingOperators(
                    leftRegisteredOperator ?? GetLeftRegisteredSurroundingOperator(),
                    rightRegisteredOperator ?? GetRightRegisteredSurroundingOperator()
                    )
                );
        }

        private void EndSurroundingOperatorsContext()
        {
            surroundingOperators.Pop();
        }

        private BnfTerm GetLeftRegisteredSurroundingOperator()
        {
            return surroundingOperators.Count > 0 ? surroundingOperators.Peek().LeftRegisteredOperator : null;
        }

        private BnfTerm GetRightRegisteredSurroundingOperator()
        {
            return surroundingOperators.Count > 0 ? surroundingOperators.Peek().RightRegisteredOperator : null;
        }

        #endregion
    }
}
