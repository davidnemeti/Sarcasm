using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public interface IBnfiExpression : IBnfiTerm
    {
        BnfExpression AsBnfExpression();
    }

    /// <summary>
    /// Typeless IBnfiExpression
    /// </summary>
    public interface IBnfiExpressionTL : IBnfiExpression, IBnfiTermTL
    {
    }

    /// <summary>
    /// Typesafe IBnfiExpression
    /// </summary>
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
            this.bnfExpression = bnfTerm.ToBnfExpression();
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

        internal IEnumerable<BnfTermList> GetBnfTermsList()
        {
            return this.bnfExpression.GetBnfTermsList();
        }
    }

    #endregion

    #region BnfiExpressionConversion

    public abstract class BnfiExpressionConversion : BnfiExpression
    {
        protected BnfiExpressionConversion()
        {
        }

        protected BnfiExpressionConversion(BnfExpression bnfExpression)
            : base(bnfExpression)
        {
        }

        protected BnfiExpressionConversion(BnfTerm bnfTerm)
            : base(bnfTerm)
        {
        }
    }

    #endregion

    public static class BnfExpressionHelpers
    {
        public static BnfExpression ToBnfExpression(this BnfTerm bnfTerm)
        {
            return bnfTerm as BnfExpression ?? new BnfExpression(bnfTerm);
        }

        internal static IEnumerable<BnfTermList> GetBnfTermsList(this BnfExpression bnfExpression)
        {
            return bnfExpression.Data;
        }
    }
}
