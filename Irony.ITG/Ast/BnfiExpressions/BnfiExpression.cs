using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG.Ast
{
    public interface IBnfiExpression : IBnfiTerm
    {
        BnfExpression AsBnfExpression();
    }

    public interface IBnfiExpression<out T> : IBnfiExpression, IBnfiTerm<T>
    {
    }

    #region BnfiExpression

    public abstract class BnfiExpression : IBnfiExpression
    {
        protected readonly BnfExpression bnfExpression;

        protected BnfiExpression()
        {
            this.bnfExpression = new BnfExpression();
        }

        protected BnfiExpression(BnfExpression bnfExpression)
        {
            this.bnfExpression = bnfExpression;
        }

        protected BnfiExpression(BnfTerm bnfTerm)
        {
            this.bnfExpression = bnfTerm is BnfExpression
                ? (BnfExpression)bnfTerm
                : new BnfExpression(bnfTerm);
        }

        public static implicit operator BnfExpression(BnfiExpression bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }

        public static implicit operator BnfTerm(BnfiExpression bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }

        BnfExpression IBnfiExpression.AsBnfExpression()
        {
            return bnfExpression;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return bnfExpression;
        }

        internal static BnfExpression Op_Plus(BnfTerm term1, BnfTerm term2)
        {
            return term1 + term2;
        }

        internal static BnfExpression Op_Pipe(BnfTerm term1, BnfTerm term2)
        {
            return term1 | term2;
        }
    }

    #endregion
}
