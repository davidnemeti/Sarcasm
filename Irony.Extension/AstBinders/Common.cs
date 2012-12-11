using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Extension.AstBinders
{
    public delegate T AstValueCreator<T>(AstContext context, ParseTreeNodeWithOutAst parseNode);
    public delegate TOut ValueConverter<TIn, TOut>(TIn inputObject);

    public interface IBnfTerm<out T>
    {
        BnfTerm AsTypeless();
    }

    public class BnfExpression<T> : IBnfTerm<T>
    {
        private readonly BnfExpression bnfExpression;

        public BnfExpression(BnfTerm bnfTerm)
        {
            this.bnfExpression = new BnfExpression(bnfTerm);
        }

        BnfTerm IBnfTerm<T>.AsTypeless()
        {
            return this.bnfExpression;
        }

        public static implicit operator BnfExpression(BnfExpression<T> bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }

        public static BnfExpression<T> operator |(BnfExpression<T> term1, BnfExpression<T> term2)
        {
            return GrammarHelper.Op_Pipe<T>(term1, term2);
        }

        public static BnfExpression<T> operator |(BnfExpression<T> term1, IBnfTerm<T> term2)
        {
            return GrammarHelper.Op_Pipe<T>(term1, term2.AsTypeless());
        }

        public static BnfExpression<T> operator |(IBnfTerm<T> term1, BnfExpression<T> term2)
        {
            return GrammarHelper.Op_Pipe<T>(term1.AsTypeless(), term2);
        }

        public static BnfExpression<T> operator +(BnfExpression<T> term1, BnfExpression<T> term2)
        {
            return GrammarHelper.Op_Plus<T>(term1, term2);
        }

        public static BnfExpression<T> operator +(BnfExpression<T> term1, BnfTerm term2)
        {
            return GrammarHelper.Op_Plus<T>(term1, term2);
        }

        public static BnfExpression<T> operator +(BnfTerm term1, BnfExpression<T> term2)
        {
            return GrammarHelper.Op_Plus<T>(term1, term2);
        }
    }

    public class BnfExpressionWithMemberBoundToBnfTerm : BnfExpression
    {
        public BnfExpressionWithMemberBoundToBnfTerm(BnfTerm bnfTerm)
            : base(bnfTerm)
        {
        }
    }

    public interface INonTerminalWithSingleTypesafeRule<in T>
    {
        BnfExpression RuleTL { get; set; }
        IBnfTerm<T> Rule { set; }
    }

    public interface INonTerminalWithMultipleTypesafeRule<in T> : INonTerminalWithSingleTypesafeRule<T>
    {
    }

    public abstract class TypeForNonTerminal : NonTerminal
    {
        protected Type type { get; private set; }

        protected TypeForNonTerminal(Type type, string errorAlias)
            : base(GrammarHelper.TypeNameWithDeclaringTypes(type), errorAlias)
        {
            this.type = type;
        }

        public new virtual BnfExpression Rule
        {
            get { return base.Rule; }
            set { base.Rule = value; }
        }

        protected static BnfExpression GetRuleWithOrBetweenTypesafeExpressions<T>(params IBnfTerm<T>[] bnfExpressions)
        {
            return bnfExpressions.Cast<BnfExpression>().Aggregate(
                (BnfExpression)bnfExpressions[0],
                (bnfExpressionProcessed, bnfExpressionToBeProcess) => bnfExpressionProcessed | bnfExpressionToBeProcess
                );
        }

        internal const string typelessQErrorMessage = "Use the typesafe QVal or QRef extension methods combined with CreateValue or ConvertValue extension methods instead";
        internal const string typelessMemberBoundErrorMessage = "Typeless MemberBoundToBnfTerm should not mix with typesafe MemberBoundToBnfTerm<TDeclaringType>";
        internal const string invalidUseOfNonExistingTypesafePipeOperatorErrorMessage = "There is no typesafe pipe operator for different types. Use SetRuleOr method instead.";
    }
}
