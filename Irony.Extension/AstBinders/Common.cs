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

    public class BnfExpression<TDeclaringType> : IBnfTerm<TDeclaringType>
    {
        private readonly BnfExpression bnfExpression;

        public BnfExpression(BnfTerm bnfTerm)
        {
            this.bnfExpression = new BnfExpression(bnfTerm);
        }

        BnfTerm IBnfTerm<TDeclaringType>.AsTypeless()
        {
            return this.bnfExpression;
        }

        public static explicit operator BnfExpression(BnfExpression<TDeclaringType> bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }
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

        internal const string obsoleteQErrorMessage = "Use the typesafe QVal or QRef extension method instead";
    }
}
