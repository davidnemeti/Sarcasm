using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public interface IBnfExpression : IBnfTerm
    {
        BnfExpression AsBnfExpression();
    }

    public interface IBnfExpression<out T> : IBnfExpression, IBnfTerm<T>
    {
    }

    #region BnfExpressionCommon

    public abstract class BnfExpressionCommon : IBnfExpression
    {
        protected readonly BnfExpression bnfExpression;

        public BnfExpressionCommon()
        {
            this.bnfExpression = new BnfExpression();
        }

        public BnfExpressionCommon(BnfExpression bnfExpression)
        {
            this.bnfExpression = bnfExpression;
        }

        protected BnfExpressionCommon(BnfTerm bnfTerm)
        {
            this.bnfExpression = new BnfExpression(bnfTerm);
        }

        public static implicit operator BnfExpression(BnfExpressionCommon bnfExpression)
        {
            return bnfExpression.bnfExpression;
        }

        //public static implicit operator BnfTerm(BnfExpressionCommon bnfExpression)
        //{
        //    return bnfExpression.bnfExpression;
        //}

        BnfExpression IBnfExpression.AsBnfExpression()
        {
            return bnfExpression;
        }

        BnfTerm IBnfTerm.AsBnfTerm()
        {
            return bnfExpression;
        }
    }

    #endregion
}
