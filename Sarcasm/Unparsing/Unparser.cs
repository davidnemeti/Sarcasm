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
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public class Unparser : IUnparser
    {
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

        internal const string logDirectoryName = "unparse_logs";

        internal readonly static TraceSource tsUnparse = new TraceSource("UNPARSE", SourceLevels.Verbose);
        internal readonly static TraceSource tsPriorities = new TraceSource("PRIORITIES", SourceLevels.Verbose);
        internal readonly static TraceSource tsParentheses = new TraceSource("PARENTHESES", SourceLevels.Verbose);

#if DEBUG
        static Unparser()
        {
            Directory.CreateDirectory(logDirectoryName);

            Trace.AutoFlush = true;

            tsUnparse.Listeners.Clear();
            tsUnparse.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "00_unparse.log"))));

            tsPriorities.Listeners.Clear();
            tsPriorities.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "priorities.log"))));

            tsParentheses.Listeners.Clear();
            tsParentheses.Listeners.Add(new TextWriterTraceListener(File.Create(Path.Combine(logDirectoryName, "parentheses.log"))));
        }
#endif

        public Grammar Grammar { get; private set; }
        public Formatting Formatting { get; private set; }

        private readonly Formatter formatter;

        private ISet<BnfTerm> bnfTermsSeenForCalculation = new HashSet<BnfTerm>();
        private IDictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKind = new Dictionary<BnfTerm, BnfTermKind>();
        private IDictionary<BnfTerm, BnfTermKind> bnfTermToBnfTermKindHint = new Dictionary<BnfTerm, BnfTermKind>();
        private IDictionary<BnfTerm, Parentheses> expressionToParentheses = new Dictionary<BnfTerm, Parentheses>();
        private Stack<SurroundingOperators> surroundingOperators = new Stack<SurroundingOperators>();
        private Stack<Parentheses> surroundingParentheses = new Stack<Parentheses>();
        private UnparseInfo _unparseInfo = null;

        public Unparser(Grammar grammar)
            : this(grammar, grammar.DefaultFormatting)
        {
        }

        public Unparser(Grammar grammar, Formatting formatting)
        {
            this.Grammar = grammar;
            this.Formatting = formatting;

            this.formatter = new Formatter(formatting);

            InitParentheses();
        }

        private void InitParentheses()
        {
            CalculateBnfTermKind(Grammar.Root);
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

            tsParentheses.Unindent();
            bnfTermKind.DebugWriteLineBnfTermKind(tsParentheses, bnfTerm);

            return bnfTermKind;
        }

        public IEnumerable<Utoken> Unparse(object obj, Context context = null)
        {
            BnfTerm bnfTerm = GetBnfTerm(obj, context);
            return Unparse(obj, bnfTerm);
        }

        public IEnumerable<Utoken> Unparse(object obj, BnfTerm bnfTerm)
        {
            return UnparseRaw(new UnparsableObject(bnfTerm, obj))
                .Cook();
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject)
        {
            UnparseInfo unparseInfoUnused;
            return UnparseRawEager(unparsableObject, out unparseInfoUnused);
        }

        private IReadOnlyList<Utoken> UnparseRawEager(UnparsableObject unparsableObject, out UnparseInfo unparseInfo)
        {
            var utokens = UnparseRaw(unparsableObject).ToList();
            unparseInfo = this._unparseInfo;
            return utokens;
        }

        private IEnumerable<Utoken> UnparseRaw(UnparsableObject unparsableObject)
        {
            if (unparsableObject.bnfTerm == null)
                throw new ArgumentNullException("bnfTerm must not be null", "bnfTerm");

            Formatter.Params @params;

            formatter.BeginBnfTerm(unparsableObject.bnfTerm, out @params);

            try
            {
                /*
                 * NOTE that we cannot return the result of ConcatIfAnyMiddle because that would result in calling
                 * formatter.End immediately (before any utokens would be yielded) which is not the expected behavior.
                 * So we have to iterate through the resulted utokens and yield them one-by-one.
                 * */

                var utokens = ConcatIfAnyMiddle(
                    formatter.YieldBefore(unparsableObject.bnfTerm, @params),
                    UnparseRawMiddle(unparsableObject),
                    formatter.YieldAfter(unparsableObject.bnfTerm, @params)
                    );

                foreach (Utoken utoken in utokens)
                    yield return utoken;
            }
            finally
            {
                formatter.EndBnfTerm(unparsableObject.bnfTerm);

                if (unparsableObject.bnfTerm.IsOperator())
                    _unparseInfo = UnparseInfo.CreateOperator(registeredOperator: unparsableObject.bnfTerm);
                else if (!IsOperator(unparsableObject.bnfTerm))
                    _unparseInfo = null;
            }
        }

        private static IEnumerable<Utoken> ConcatIfAnyMiddle(IEnumerable<Utoken> utokensBefore, IEnumerable<Utoken> utokensMiddle, IEnumerable<Utoken> utokensAfter)
        {
            bool isAnyUtokenMiddle = false;

            foreach (Utoken utokenMiddle in utokensMiddle)
            {
                if (!isAnyUtokenMiddle)
                {
                    foreach (Utoken utokenBefore in utokensBefore)
                        yield return utokenBefore;
                }

                isAnyUtokenMiddle = true;

                yield return utokenMiddle;
            }

            if (isAnyUtokenMiddle)
            {
                foreach (Utoken utokenAfter in utokensAfter)
                    yield return utokenAfter;
            }
        }

        private IEnumerable<Utoken> UnparseRawMiddle(UnparsableObject unparsableObject)
        {
            BnfTerm bnfTerm = unparsableObject.bnfTerm;
            object obj = unparsableObject.obj;

            if (bnfTerm is KeyTerm)
            {
                tsUnparse.Debug("keyterm: [{0}]", ((KeyTerm)bnfTerm).Text);
                yield return Utoken.CreateText(((KeyTerm)bnfTerm).Text, obj);
            }
            else if (bnfTerm is Terminal)
            {
                tsUnparse.Debug("terminal: [\"{0}\"]", obj.ToString());
                yield return Utoken.CreateText(obj);
            }
            else if (bnfTerm is NonTerminal)
            {
                tsUnparse.Debug("nonterminal: {0}", bnfTerm.Name);
                tsUnparse.Indent();

                try
                {
                    NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                    IUnparsable unparsable = nonTerminal as IUnparsable;

                    if (unparsable == null)
                        throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' is not IUnparsable.", obj, obj.GetType().Name, nonTerminal.Name));

                    IEnumerable<Utoken> directUtokens;

                    if (unparsable.TryGetUtokensDirectly(this, obj, out directUtokens))
                    {
                        foreach (Utoken utoken in directUtokens)
                            yield return utoken;

                        tsUnparse.Debug("utokenized: [{0}]", obj != null ? string.Format("\"{0}\"", obj) : "<<NULL>>");
                    }
                    else
                    {
                        IEnumerable<UnparsableObject> chosenChildren = ChooseChildrenByPriority(obj, unparsable);

                        if (chosenChildren == null)
                        {
                            throw new UnparseException(string.Format("Cannot unparse '{0}' (type: '{1}'). BnfTerm '{2}' has no appropriate production rule.",
                                obj, obj.GetType().Name, nonTerminal.Name));
                        }

                        if (IsExpressionWithOperator(nonTerminal))
                        {
                            var utokens = new List<Utoken>();

                            BeginParenthesesContext(nonTerminal);

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

                            EndParenthesesContext(nonTerminal);

                            foreach (Utoken utoken in utokens)
                                yield return utoken;
                        }
                        else
                        {
                            foreach (UnparsableObject childValue in chosenChildren)
                                foreach (Utoken utoken in UnparseRaw(childValue))
                                    yield return utoken;
                        }
                    }
                }
                finally
                {
                    tsUnparse.Unindent();
                }
            }
            else if (bnfTerm is GrammarHint)
            {
                // GrammarHint is legal, but it does not need any unparse
            }
            else
            {
                throw new ArgumentException(string.Format("bnfTerm '{0}' with unknown type: '{1}'" + bnfTerm.Name, bnfTerm.GetType().Name));
            }
        }

        private IEnumerable<UnparsableObject> ChooseChildrenByPriority(object obj, IUnparsable unparsable)
        {
            // TODO: we should check whether any bnfTermList has an UnparseHint

            // TODO: we should avoid circularity (e.g. expression -> LEFT_PAREN + expression + RIGHT_PAREN)

            tsPriorities.Debug("{0} BEGIN priorities", unparsable.AsNonTerminal());
            tsPriorities.Indent();

            try
            {
                return GetChildBnfTermLists(unparsable.AsNonTerminal())
                    .Select(childBnfTerms =>
                        {
                            var childUnparsableObjects = unparsable.GetChildUnparsableObjects(childBnfTerms, obj);
                            return new
                            {
                                UnparsableObjects = childUnparsableObjects,
                                Priority = unparsable.GetChildBnfTermListPriority(this, obj, childUnparsableObjects)
                                    .DebugWriteLinePriority(tsPriorities, unparsable.AsNonTerminal(), obj)
                            };
                        }
                    )
                    .Where(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.HasValue)
                    .OrderByDescending(childBnfTermsWithPriority => childBnfTermsWithPriority.Priority.Value)
                    .Select(childBnfTermsWithPriority => childBnfTermsWithPriority.UnparsableObjects)
                    .FirstOrDefault();
            }
            finally
            {
                tsPriorities.Unindent();
                tsPriorities.Debug("{0} END priorities", unparsable.AsNonTerminal());
                tsPriorities.Debug("");
            }
        }

        internal static IEnumerable<BnfTermList> GetChildBnfTermLists(NonTerminal nonTerminal)
        {
            return nonTerminal.Productions.Select(production => production.RValues);
        }

        private BnfTerm GetBnfTerm(object obj, Context context)
        {
            return Grammar.Root;
            // TODO: do this by choosing by context
        }

        int? IUnparser.GetBnfTermPriority(BnfTerm bnfTerm, object obj)
        {
            if (bnfTerm is NonTerminal && bnfTerm is IUnparsable)
            {
                NonTerminal nonTerminal = (NonTerminal)bnfTerm;
                IUnparsable unparsable = (IUnparsable)bnfTerm;

                tsPriorities.Indent();

                int? priority = GetChildBnfTermLists(nonTerminal)
                    .Max(childBnfTerms => unparsable.GetChildBnfTermListPriority(this, obj, unparsable.GetChildUnparsableObjects(childBnfTerms, obj))
                        .DebugWriteLinePriority(tsPriorities, bnfTerm, obj));

                tsPriorities.Unindent();

                priority.DebugWriteLinePriority(tsPriorities, bnfTerm, obj, messageAfter: " (MAX)");

                return priority;
            }
            else
            {
                Misc.DebugWriteLinePriority(0, tsPriorities, bnfTerm, obj, messageAfter: " (for terminal)");
                return 0;
            }
        }

        IFormatProvider IUnparser.FormatProvider { get { return this.FormatProvider; } }

        protected IFormatProvider FormatProvider { get { return this.Formatting.FormatProvider; } }

        public static string ToString(IFormatProvider formatProvider, object obj)
        {
            return string.Format(formatProvider, "{0}", obj);
        }

        #region Parenthesis handling for expressions

        private IEnumerable<Utoken> LeftParenthesesIfNeeded(UnparsableObject unparsableExpression, UnparsedOperator unparsedOperator)
        {
            try
            {
                if (NeedParentheses(unparsableExpression.bnfTerm, unparsedOperator))
                    return UnparseRaw(new UnparsableObject(GetLeftSurroundingParenthesis(), unparsableExpression.obj));
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
                    return UnparseRaw(new UnparsableObject(GetRightSurroundingParenthesis(), unparsableExpression.obj));
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

        public bool IsExpressionWithOperator(BnfTerm bnfTerm)
        {
            return bnfTermToBnfTermKind[bnfTerm] == BnfTermKind.ExpressionWithOperator;
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

    public static class UnparserExtensions
    {
        public static string AsString(this IEnumerable<Utoken> utokens, Unparser unparser)
        {
            return string.Concat(utokens.Select(utoken => utoken.ToString(unparser.Formatting)));
        }

        public static async Task WriteToStreamAsync(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    await sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
        }

        public static void WriteToStream(this IEnumerable<Utoken> utokens, Stream stream, Unparser unparser)
        {
//            utokens.WriteToStreamAsync(stream, unparser).Wait();
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (Utoken utoken in utokens)
                    sw.WriteAsync(utoken.ToString(unparser.Formatting));
            }
        }

        internal static IEnumerable<Utoken> Cook(this IEnumerable<Utoken> utokens)
        {
            return Formatter.PostProcess(utokens);
        }
    }

    public class Context
    {
        public MemberInfo MemberInfo { get; private set; }

        public Context(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }
    }

    public interface IUnparsable : INonTerminal
    {
        bool TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens);
        IEnumerable<UnparsableObject> GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj);
        int? GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects);
    }

    public interface IUnparser
    {
        int? GetBnfTermPriority(BnfTerm bnfTerm, object obj);
        IFormatProvider FormatProvider { get; }
    }

    public class UnparsableObject
    {
        public readonly BnfTerm bnfTerm;
        public readonly object obj;

        public UnparsableObject(BnfTerm bnfTerm, object obj)
        {
            this.bnfTerm = bnfTerm;
            this.obj = obj;
        }
    }

    public delegate IEnumerable<Utoken> ValueUtokenizer<T>(IFormatProvider formatProvider, T obj);
}
