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
    public class TypeForTransient : TypeForNonTerminal
    {
        protected TypeForTransient(Type type, string errorAlias)
            : base(type, errorAlias)
        {
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;      // the child node already contains the created ast node
        }

        public static TypeForTransient<TType> Of<TType>(string errorAlias = null)
        {
            return new TypeForTransient<TType>(errorAlias);
        }

        public static TypeForTransient Of(Type type, string errorAlias = null)
        {
            return new TypeForTransient(type, errorAlias);
        }
    }

    public class TypeForTransient<TType> : TypeForTransient, IBnfTerm<TType>, INonTerminalWithMultipleTypesafeRule<TType>
    {
        internal TypeForTransient(string errorAlias)
            : base(typeof(TType), errorAlias)
        {
        }

        BnfTerm IBnfTerm<TType>.AsTypeless()
        {
            return this;
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public new IBnfTerm<TType> Rule { set { this.SetRule(value); } }

        public BnfExpression RuleTL { get { return base.Rule; } set { base.Rule = value; } }

        public static BnfExpressionTransient<TType> operator |(TypeForTransient<TType> term1, TypeForTransient<TType> term2)
        {
            return BnfExpressionTransient<TType>.Op_Pipe(term1, term2);
        }

        public static BnfExpressionTransient<TType> operator +(TypeForTransient<TType> term1, KeyTermPunctuation term2)
        {
            return BnfExpressionTransient<TType>.Op_Plus(term1, term2);
        }

        public static BnfExpressionTransient<TType> operator +(KeyTermPunctuation term1, TypeForTransient<TType> term2)
        {
            return BnfExpressionTransient<TType>.Op_Plus(term1, term2);
        }

        /*
         * public static BnfExpressionTransient<T> operator +(TypeForTransient<T> term1, TypeForTransient<T> term2)
         * is not defined, because in a BnfExpressionTransient there can be only one TypeForTransient term,
         * and the resulting BnfExpressionTransient would contain two of them
         * */
    }
}
