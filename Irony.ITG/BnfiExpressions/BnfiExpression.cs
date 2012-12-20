using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public interface IBnfiExpression : IBnfiTerm
    {
        BnfExpression AsBnfExpression();
    }

    public interface IBnfiExpression<out T> : IBnfiExpression, IBnfiTerm<T>
    {
    }

    #region BnfiExpressionCommon

    public abstract class BnfiExpressionCommon : IBnfiExpression
    {
        protected readonly BnfExpression bnfExpression;

        public BnfiExpressionCommon()
        {
            this.bnfExpression = new BnfExpression();
        }

        public BnfiExpressionCommon(BnfExpression bnfExpression)
        {
            this.bnfExpression = bnfExpression;
        }

        protected BnfiExpressionCommon(BnfTerm bnfTerm)
        {
            this.bnfExpression = new BnfExpression(bnfTerm);
        }

        public static implicit operator BnfExpression(BnfiExpressionCommon bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }

        //public static implicit operator BnfTerm(BnfiExpressionCommon bnfExpression)
        //{
        //    return bnfExpression.bnfExpression;
        //}

        BnfExpression IBnfiExpression.AsBnfExpression()
        {
            return bnfExpression;
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return bnfExpression;
        }
    }

    #endregion
}
