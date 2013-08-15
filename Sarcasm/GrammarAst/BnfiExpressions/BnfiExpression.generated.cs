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

 
 
 
 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
	#region BnfiExpressionNoAst definition and operators

	#region BnfiExpressionNoAst definition

	public partial class BnfiExpressionNoAst : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionNoAst()
		{
		}

		public BnfiExpressionNoAst(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionNoAst(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionNoAst(BnfExpression bnfExpression)
		{
			return new BnfiExpressionNoAst(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermNoAst, BnfiTermKeyTerm]: implicit conversions from BnfiExpressionNoAst

	public partial class BnfiTermNoAst
	{
		public static implicit operator BnfiExpressionNoAst(BnfiTermNoAst term)
		{
			return new BnfiExpressionNoAst((BnfTerm)term);
		}
	}

	public partial class BnfiTermKeyTerm
	{
		public static implicit operator BnfiExpressionNoAst(BnfiTermKeyTerm term)
		{
			return new BnfiExpressionNoAst((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionNoAst: implicit conversions from [GrammarHint]

	public partial class BnfiExpressionNoAst
	{
		public static implicit operator BnfiExpressionNoAst(GrammarHint term)
		{
			return new BnfiExpressionNoAst((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermNoAst '+' operators for BnfExpression

	public partial class BnfiTermNoAst
	{
        public static BnfiExpressionNoAst operator +(BnfiTermNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermKeyTerm term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(GrammarHint term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermKeyTerm '+' operators for BnfExpression

	public partial class BnfiTermKeyTerm
	{
        public static BnfiExpressionNoAst operator +(BnfiTermKeyTerm term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermKeyTerm term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(GrammarHint term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermNoAst '|' operators for BnfExpression

	public partial class BnfiTermNoAst
	{
        public static BnfiExpressionNoAst operator |(BnfiTermNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(GrammarHint term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermKeyTerm term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermNoAst term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiExpressionNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermKeyTerm '|' operators for BnfExpression

	public partial class BnfiTermKeyTerm
	{
        public static BnfiExpressionNoAst operator |(BnfiTermKeyTerm term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(GrammarHint term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermKeyTerm term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiTermKeyTerm term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiExpressionNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionNoAst '+' operators for BnfExpression

	public partial class BnfiExpressionNoAst
	{
        public static BnfiExpressionNoAst operator +(BnfiExpressionNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermNoAst term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiExpressionNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiTermKeyTerm term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiExpressionNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(GrammarHint term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator +(BnfiExpressionNoAst term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionNoAst '|' operators for BnfExpression

	public partial class BnfiExpressionNoAst
	{
        public static BnfiExpressionNoAst operator |(BnfiExpressionNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(GrammarHint term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionNoAst operator |(BnfiExpressionNoAst term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionNoAst)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionRecordTL definition and operators

	#region BnfiExpressionRecordTL definition

	public partial class BnfiExpressionRecordTL : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionRecordTL()
		{
		}

		public BnfiExpressionRecordTL(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionRecordTL(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionRecordTL(BnfExpression bnfExpression)
		{
			return new BnfiExpressionRecordTL(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [Member, BnfiTermCopy]: implicit conversions from BnfiExpressionRecordTL

	public partial class Member
	{
		public static implicit operator BnfiExpressionRecordTL(Member term)
		{
			return new BnfiExpressionRecordTL((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopy
	{
		public static implicit operator BnfiExpressionRecordTL(BnfiTermCopy term)
		{
			return new BnfiExpressionRecordTL((BnfTerm)term);
		}
	}

	#endregion

	#region Member '+' operators for BnfExpression

	public partial class Member
	{
        public static BnfiExpressionRecordTL operator +(Member term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermNoAst term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermKeyTerm term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, GrammarHint term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(GrammarHint term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionNoAst term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy '+' operators for BnfExpression

	public partial class BnfiTermCopy
	{
        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermNoAst term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermKeyTerm term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, GrammarHint term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(GrammarHint term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionNoAst term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region Member '|' operators for BnfExpression

	public partial class Member
	{
        public static BnfiExpressionRecordTL operator |(Member term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(Member term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(BnfiTermCopy term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(Member term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(BnfiExpressionRecordTL term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy '|' operators for BnfExpression

	public partial class BnfiTermCopy
	{
        public static BnfiExpressionRecordTL operator |(BnfiTermCopy term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(BnfiTermCopy term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator |(BnfiExpressionRecordTL term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionRecordTL '+' operators for BnfExpression

	public partial class BnfiExpressionRecordTL
	{
        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermNoAst term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermKeyTerm term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, GrammarHint term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(GrammarHint term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionNoAst term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, Member term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(Member term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, BnfiTermCopy term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiTermCopy term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecordTL operator +(BnfiExpressionRecordTL term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionRecordTL '|' operators for BnfExpression

	public partial class BnfiExpressionRecordTL
	{
        public static BnfiExpressionRecordTL operator |(BnfiExpressionRecordTL term1, BnfiExpressionRecordTL term2)
        {
            return (BnfiExpressionRecordTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionRecord<TD> definition and operators

	#region BnfiExpressionRecord<TD> definition

	public partial class BnfiExpressionRecord<TD> : BnfiExpression, IBnfiExpression<TD>, IBnfiTermPlusAbleForType<TD>
	{
		#region Construction

		public BnfiExpressionRecord()
		{
		}

		public BnfiExpressionRecord(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionRecord(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionRecord<TD>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionRecord<TD>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [Member<TDDeclaringType>, BnfiTermCopy<TD>]: implicit conversions from BnfiExpressionRecord<TD>

	public partial class Member<TDDeclaringType>
	{
		public static implicit operator BnfiExpressionRecord<TDDeclaringType>(Member<TDDeclaringType> term)
		{
			return new BnfiExpressionRecord<TDDeclaringType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopy<TD>
	{
		public static implicit operator BnfiExpressionRecord<TD>(BnfiTermCopy<TD> term)
		{
			return new BnfiExpressionRecord<TD>((BnfTerm)term);
		}
	}

	#endregion

	#region Member<TDDeclaringType> '+' operators for BnfExpression

	public partial class Member<TDDeclaringType>
	{
        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(BnfiTermNoAst term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(BnfiTermKeyTerm term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, GrammarHint term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(GrammarHint term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(BnfiExpressionNoAst term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(Member<TDDeclaringType> term1, BnfiTermCopy<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator +(BnfiTermCopy<TDDeclaringType> term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy<TD> '+' operators for BnfExpression

	public partial class BnfiTermCopy<TD>
	{
        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermNoAst term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermKeyTerm term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(GrammarHint term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionNoAst term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region Member<TDDeclaringType> '|' operators for BnfExpression

	public partial class Member<TDDeclaringType>
	{
        public static BnfiExpressionRecord<TDDeclaringType> operator |(Member<TDDeclaringType> term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator |(Member<TDDeclaringType> term1, BnfiTermCopy<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator |(BnfiTermCopy<TDDeclaringType> term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator |(Member<TDDeclaringType> term1, BnfiExpressionRecord<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TDDeclaringType> operator |(BnfiExpressionRecord<TDDeclaringType> term1, Member<TDDeclaringType> term2)
        {
            return (BnfiExpressionRecord<TDDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy<TD> '|' operators for BnfExpression

	public partial class BnfiTermCopy<TD>
	{
        public static BnfiExpressionRecord<TD> operator |(BnfiTermCopy<TD> term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator |(BnfiTermCopy<TD> term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator |(BnfiExpressionRecord<TD> term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionRecord<TD> '+' operators for BnfExpression

	public partial class BnfiExpressionRecord<TD>
	{
        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermNoAst term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermKeyTerm term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(GrammarHint term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionNoAst term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, Member<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(Member<TD> term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, BnfiTermCopy<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiTermCopy<TD> term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionRecord<TD> operator +(BnfiExpressionRecord<TD> term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionRecord<TD> '|' operators for BnfExpression

	public partial class BnfiExpressionRecord<TD>
	{
        public static BnfiExpressionRecord<TD> operator |(BnfiExpressionRecord<TD> term1, BnfiExpressionRecord<TD> term2)
        {
            return (BnfiExpressionRecord<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionConversionTL definition and operators

	#region BnfiExpressionConversionTL definition

	public partial class BnfiExpressionConversionTL : BnfiExpressionConversion, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionConversionTL()
		{
		}

		public BnfiExpressionConversionTL(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionConversionTL(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionConversionTL(BnfExpression bnfExpression)
		{
			return new BnfiExpressionConversionTL(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermConversion]: implicit conversions from BnfiExpressionConversionTL

	public partial class BnfiTermConversion
	{
		public static implicit operator BnfiExpressionConversionTL(BnfiTermConversion term)
		{
			return new BnfiExpressionConversionTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermConversion '+' operators for BnfExpression

	public partial class BnfiTermConversion
	{
        public static BnfiExpressionConversionTL operator +(BnfiTermConversion term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermNoAst term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermConversion term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermKeyTerm term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermConversion term1, GrammarHint term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(GrammarHint term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermConversion term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiExpressionNoAst term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConversion '|' operators for BnfExpression

	public partial class BnfiTermConversion
	{
        public static BnfiExpressionConversionTL operator |(BnfiTermConversion term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator |(BnfiTermConversion term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator |(BnfiExpressionConversionTL term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionConversionTL '+' operators for BnfExpression

	public partial class BnfiExpressionConversionTL
	{
        public static BnfiExpressionConversionTL operator +(BnfiExpressionConversionTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermNoAst term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiExpressionConversionTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiTermKeyTerm term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiExpressionConversionTL term1, GrammarHint term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(GrammarHint term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiExpressionConversionTL term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversionTL operator +(BnfiExpressionNoAst term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionConversionTL '|' operators for BnfExpression

	public partial class BnfiExpressionConversionTL
	{
        public static BnfiExpressionConversionTL operator |(BnfiExpressionConversionTL term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionConversionTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionConversion<TD> definition and operators

	#region BnfiExpressionConversion<TD> definition

	public partial class BnfiExpressionConversion<TD> : BnfiExpressionConversion, IBnfiExpression<TD>, IBnfiTermOrAbleForChoice<TD>
	{
		#region Construction

		public BnfiExpressionConversion()
		{
		}

		public BnfiExpressionConversion(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionConversion(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionConversion<TD>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionConversion<TD>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermConversion<TD>]: implicit conversions from BnfiExpressionConversion<TD>

	public partial class BnfiTermConversion<TD>
	{
		public static implicit operator BnfiExpressionConversion<TD>(BnfiTermConversion<TD> term)
		{
			return new BnfiExpressionConversion<TD>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermConversion<TD> '+' operators for BnfExpression

	public partial class BnfiTermConversion<TD>
	{
        public static BnfiExpressionConversion<TD> operator +(BnfiTermConversion<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermNoAst term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermConversion<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermKeyTerm term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermConversion<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(GrammarHint term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermConversion<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionNoAst term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConversion<TD> '|' operators for BnfExpression

	public partial class BnfiTermConversion<TD>
	{
        public static BnfiExpressionConversion<TD> operator |(BnfiTermConversion<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator |(BnfiTermConversion<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionConversion<TD> '+' operators for BnfExpression

	public partial class BnfiExpressionConversion<TD>
	{
        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionConversion<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermNoAst term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionConversion<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiTermKeyTerm term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionConversion<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(GrammarHint term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionConversion<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionConversion<TD> operator +(BnfiExpressionNoAst term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionConversion<TD> '|' operators for BnfExpression

	public partial class BnfiExpressionConversion<TD>
	{
        public static BnfiExpressionConversion<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionConversion<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionChoiceTL definition and operators

	#region BnfiExpressionChoiceTL definition

	public partial class BnfiExpressionChoiceTL : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionChoiceTL()
		{
		}

		public BnfiExpressionChoiceTL(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionChoiceTL(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionChoiceTL(BnfExpression bnfExpression)
		{
			return new BnfiExpressionChoiceTL(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermRecord, BnfiTermConstant, BnfiTermChoice, BnfiTermCollection]: implicit conversions from BnfiExpressionChoiceTL

	public partial class BnfiTermRecord
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermRecord term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermConstant term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermChoice term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	public partial class BnfiTermCollection
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermCollection term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionChoiceTL: implicit conversions from [BnfiTermConversion, BnfiExpressionConversionTL, BnfiExpressionConversion]

	public partial class BnfiExpressionChoiceTL
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermConversion term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoiceTL(BnfiExpressionConversionTL term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoiceTL(BnfiExpressionConversion term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermRecord '+' operators for BnfExpression

	public partial class BnfiTermRecord
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermRecord term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermNoAst term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermRecord term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTerm term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermRecord term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant '+' operators for BnfExpression

	public partial class BnfiTermConstant
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermConstant term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermNoAst term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermConstant term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTerm term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermConstant term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '+' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermChoice term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermNoAst term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermChoice term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTerm term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermChoice term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '+' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermCollection term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermNoAst term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermCollection term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTerm term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermCollection term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermRecord '|' operators for BnfExpression

	public partial class BnfiTermRecord
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConversion term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversionTL term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiExpressionConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversion term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermRecord term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermRecord term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant '|' operators for BnfExpression

	public partial class BnfiTermConstant
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConversion term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversionTL term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiExpressionConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversion term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConstant term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermConstant term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '|' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConversion term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversionTL term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiExpressionConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversion term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '|' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConversion term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversionTL term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiExpressionConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversion term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoiceTL '+' operators for BnfExpression

	public partial class BnfiExpressionChoiceTL
	{
        public static BnfiExpressionChoiceTL operator +(BnfiExpressionChoiceTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermNoAst term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionChoiceTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTerm term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionChoiceTL term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoiceTL '|' operators for BnfExpression

	public partial class BnfiExpressionChoiceTL
	{
        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermConversion term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiExpressionConversionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversionTL term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiExpressionConversion term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionConversion term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionChoice<TD> definition and operators

	#region BnfiExpressionChoice<TD> definition

	public partial class BnfiExpressionChoice<TD> : BnfiExpression, IBnfiExpression<TD>, IBnfiTermOrAbleForChoice<TD>
	{
		#region Construction

		public BnfiExpressionChoice()
		{
		}

		public BnfiExpressionChoice(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionChoice(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionChoice<TD>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionChoice<TD>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermRecord<TD>, BnfiTermConstant<TD>, BnfiTermChoice<TD>, BnfiTermCollectionWithCollectionType<TDCollectionType>]: implicit conversions from BnfiExpressionChoice<TD>

	public partial class BnfiTermRecord<TD>
	{
		public static implicit operator BnfiExpressionChoice<TD>(BnfiTermRecord<TD> term)
		{
			return new BnfiExpressionChoice<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant<TD>
	{
		public static implicit operator BnfiExpressionChoice<TD>(BnfiTermConstant<TD> term)
		{
			return new BnfiExpressionChoice<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice<TD>
	{
		public static implicit operator BnfiExpressionChoice<TD>(BnfiTermChoice<TD> term)
		{
			return new BnfiExpressionChoice<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermCollectionWithCollectionType<TDCollectionType>
	{
		public static implicit operator BnfiExpressionChoice<TDCollectionType>(BnfiTermCollectionWithCollectionType<TDCollectionType> term)
		{
			return new BnfiExpressionChoice<TDCollectionType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionChoice<TD>: implicit conversions from [BnfiTermConversion<TD>, BnfiExpressionConversion<TD>]

	public partial class BnfiExpressionChoice<TD>
	{
		public static implicit operator BnfiExpressionChoice<TD>(BnfiTermConversion<TD> term)
		{
			return new BnfiExpressionChoice<TD>((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoice<TD>(BnfiExpressionConversion<TD> term)
		{
			return new BnfiExpressionChoice<TD>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermRecord<TD> '+' operators for BnfExpression

	public partial class BnfiTermRecord<TD>
	{
        public static BnfiExpressionChoice<TD> operator +(BnfiTermRecord<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermNoAst term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermRecord<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermKeyTerm term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermRecord<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(GrammarHint term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<TD> '+' operators for BnfExpression

	public partial class BnfiTermConstant<TD>
	{
        public static BnfiExpressionChoice<TD> operator +(BnfiTermConstant<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermNoAst term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermConstant<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermKeyTerm term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermConstant<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(GrammarHint term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TD> '+' operators for BnfExpression

	public partial class BnfiTermChoice<TD>
	{
        public static BnfiExpressionChoice<TD> operator +(BnfiTermChoice<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermNoAst term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermChoice<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermKeyTerm term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermChoice<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(GrammarHint term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollectionWithCollectionType<TDCollectionType> '+' operators for BnfExpression

	public partial class BnfiTermCollectionWithCollectionType<TDCollectionType>
	{
        public static BnfiExpressionChoice<TDCollectionType> operator +(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator +(BnfiTermNoAst term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator +(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator +(BnfiTermKeyTerm term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator +(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator +(GrammarHint term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermRecord<TD> '|' operators for BnfExpression

	public partial class BnfiTermRecord<TD>
	{
        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConversion<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiTermCollectionWithCollectionType<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermCollectionWithCollectionType<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermRecord<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<TD> '|' operators for BnfExpression

	public partial class BnfiTermConstant<TD>
	{
        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConversion<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiTermCollectionWithCollectionType<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermCollectionWithCollectionType<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConstant<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TD> '|' operators for BnfExpression

	public partial class BnfiTermChoice<TD>
	{
        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConversion<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiTermCollectionWithCollectionType<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermCollectionWithCollectionType<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermChoice<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollectionWithCollectionType<TDCollectionType> '|' operators for BnfExpression

	public partial class BnfiTermCollectionWithCollectionType<TDCollectionType>
	{
        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiTermConversion<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiTermConversion<TDCollectionType> term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiExpressionConversion<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiExpressionConversion<TDCollectionType> term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiExpressionChoice<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TDCollectionType> operator |(BnfiExpressionChoice<TDCollectionType> term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionChoice<TDCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice<TD> '+' operators for BnfExpression

	public partial class BnfiExpressionChoice<TD>
	{
        public static BnfiExpressionChoice<TD> operator +(BnfiExpressionChoice<TD> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermNoAst term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiExpressionChoice<TD> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiTermKeyTerm term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(BnfiExpressionChoice<TD> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator +(GrammarHint term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice<TD> '|' operators for BnfExpression

	public partial class BnfiExpressionChoice<TD>
	{
        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiTermConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiTermConversion<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiExpressionConversion<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionConversion<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TD> operator |(BnfiExpressionChoice<TD> term1, BnfiExpressionChoice<TD> term2)
        {
            return (BnfiExpressionChoice<TD>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionGeneral<TD> definition and operators

	#region BnfiExpressionGeneral<TD> definition

	public partial class BnfiExpressionGeneral<TD> : BnfiExpression, IBnfiExpression<TD>
	{
		#region Construction

		public BnfiExpressionGeneral()
		{
		}

		public BnfiExpressionGeneral(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionGeneral(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionGeneral<TD>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionGeneral<TD>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermRecord<TD>, BnfiTermConstant<TD>, BnfiTermChoice<TD>, BnfiTermCollectionWithCollectionType<TDCollectionType>]: implicit conversions from BnfiExpressionGeneral<TD>

	public partial class BnfiTermRecord<TD>
	{
		public static implicit operator BnfiExpressionGeneral<TD>(BnfiTermRecord<TD> term)
		{
			return new BnfiExpressionGeneral<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant<TD>
	{
		public static implicit operator BnfiExpressionGeneral<TD>(BnfiTermConstant<TD> term)
		{
			return new BnfiExpressionGeneral<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice<TD>
	{
		public static implicit operator BnfiExpressionGeneral<TD>(BnfiTermChoice<TD> term)
		{
			return new BnfiExpressionGeneral<TD>((BnfTerm)term);
		}
	}

	public partial class BnfiTermCollectionWithCollectionType<TDCollectionType>
	{
		public static implicit operator BnfiExpressionGeneral<TDCollectionType>(BnfiTermCollectionWithCollectionType<TDCollectionType> term)
		{
			return new BnfiExpressionGeneral<TDCollectionType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermRecord<TD> '+' operators for BnfExpression

	public partial class BnfiTermRecord<TD>
	{
        public static BnfiExpressionGeneral<TD> operator +(BnfiTermRecord<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TD> operator +(BnfiExpressionNoAst term1, BnfiTermRecord<TD> term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<TD> '+' operators for BnfExpression

	public partial class BnfiTermConstant<TD>
	{
        public static BnfiExpressionGeneral<TD> operator +(BnfiTermConstant<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TD> operator +(BnfiExpressionNoAst term1, BnfiTermConstant<TD> term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TD> '+' operators for BnfExpression

	public partial class BnfiTermChoice<TD>
	{
        public static BnfiExpressionGeneral<TD> operator +(BnfiTermChoice<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TD> operator +(BnfiExpressionNoAst term1, BnfiTermChoice<TD> term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollectionWithCollectionType<TDCollectionType> '+' operators for BnfExpression

	public partial class BnfiTermCollectionWithCollectionType<TDCollectionType>
	{
        public static BnfiExpressionGeneral<TDCollectionType> operator +(BnfiTermCollectionWithCollectionType<TDCollectionType> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionGeneral<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TDCollectionType> operator +(BnfiExpressionNoAst term1, BnfiTermCollectionWithCollectionType<TDCollectionType> term2)
        {
            return (BnfiExpressionGeneral<TDCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionGeneral<TD> '+' operators for BnfExpression

	public partial class BnfiExpressionGeneral<TD>
	{
        public static BnfiExpressionGeneral<TD> operator +(BnfiExpressionGeneral<TD> term1, BnfiExpressionNoAst term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TD> operator +(BnfiExpressionNoAst term1, BnfiExpressionGeneral<TD> term2)
        {
            return (BnfiExpressionGeneral<TD>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

}
