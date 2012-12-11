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
    }

    public class BnfExpressionWithMemberBoundToBnfTerm : BnfExpression
    {
        public BnfExpressionWithMemberBoundToBnfTerm(BnfTerm bnfTerm)
            : base(bnfTerm)
        {
        }
    }

    public interface ITypeForWithSingleTypesafeRule<in T>
    {
        BnfExpression RuleTL { get; set; }

        IBnfTerm<T> Rule { set; }
    }

    public interface ITypeForWithMultipleTypesafeRule<in T> : ITypeForWithSingleTypesafeRule<T>
    {
        void SetRule(params IBnfTerm<T>[] bnfExpressions);
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

        protected static BnfExpression<T> Op_Pipe<T>(BnfTerm bnfTerm1, BnfTerm bnfTerm2)
        {
            return new BnfExpression<T>(BnfTerm.Op_Pipe(bnfTerm1, bnfTerm2));
        }

        internal const string typelessQErrorMessage = "Use the typesafe QVal or QRef extension methods combined with CreateValue or ConvertValue extension methods instead";
        internal const string typelessMemberBoundErrorMessage = "Typeless MemberBoundToBnfTerm should not mix with typesafe MemberBoundToBnfTerm<TDeclaringType>";
        internal const string invalidUseOfNonExistingTypesafePipeOperatorErrorMessage = "There is no typesafe pipe operator for different types. Use SetRuleOr method instead.";
    }
}
