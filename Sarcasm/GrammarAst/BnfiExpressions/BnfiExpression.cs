#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;

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
    public interface IBnfiExpression<out TD> : IBnfiExpression, IBnfiTerm<TD>
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

        Type IBnfiTerm.DomainType
        {
            get { return null; }
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
