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
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
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

        public static BnfExpression<TType> operator |(TypeForTransient<TType> term1, TypeForTransient<TType> term2)
        {
            return GrammarHelper.Op_Pipe<TType>(term1, term2);
        }

        public static BnfExpression<TType> operator +(TypeForTransient<TType> bnfTerm1, BnfExpression bnfTerm2)
        {
            return GrammarHelper.Op_Plus<TType>(bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TType> operator +(BnfExpression bnfTerm1, TypeForTransient<TType> bnfTerm2)
        {
            return GrammarHelper.Op_Plus<TType>(bnfTerm1, bnfTerm2);
        }
    }
}
