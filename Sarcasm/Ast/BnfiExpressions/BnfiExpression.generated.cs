 
 
 
 
 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.Ast
{
	#region BnfiExpressionTerminals definition and operators

	#region BnfiExpressionTerminals definition

	public partial class BnfiExpressionTerminals : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionTerminals()
		{
		}

		public BnfiExpressionTerminals(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionTerminals(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionTerminals(BnfExpression bnfExpression)
		{
			return new BnfiExpressionTerminals(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermNoAst, BnfiTermKeyTerm]: implicit conversions from BnfiExpressionTerminals

	public partial class BnfiTermNoAst
	{
		public static implicit operator BnfiExpressionTerminals(BnfiTermNoAst term)
		{
			return new BnfiExpressionTerminals((BnfTerm)term);
		}
	}

	public partial class BnfiTermKeyTerm
	{
		public static implicit operator BnfiExpressionTerminals(BnfiTermKeyTerm term)
		{
			return new BnfiExpressionTerminals((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionTerminals: implicit conversions from [Terminal, GrammarHint]

	public partial class BnfiExpressionTerminals
	{
		public static implicit operator BnfiExpressionTerminals(Terminal term)
		{
			return new BnfiExpressionTerminals((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionTerminals(GrammarHint term)
		{
			return new BnfiExpressionTerminals((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermNoAst '+' operators for BnfExpression

	public partial class BnfiTermNoAst
	{
        public static BnfiExpressionTerminals operator +(BnfiTermNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermKeyTerm term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermNoAst term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(Terminal term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(GrammarHint term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermKeyTerm '+' operators for BnfExpression

	public partial class BnfiTermKeyTerm
	{
        public static BnfiExpressionTerminals operator +(BnfiTermKeyTerm term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermKeyTerm term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(Terminal term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermKeyTerm term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(GrammarHint term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermNoAst '|' operators for BnfExpression

	public partial class BnfiTermNoAst
	{
        public static BnfiExpressionTerminals operator |(BnfiTermNoAst term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(Terminal term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermNoAst term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(GrammarHint term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermNoAst term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermNoAst term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermKeyTerm term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermNoAst term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiExpressionTerminals term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermKeyTerm '|' operators for BnfExpression

	public partial class BnfiTermKeyTerm
	{
        public static BnfiExpressionTerminals operator |(BnfiTermKeyTerm term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(Terminal term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermKeyTerm term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(GrammarHint term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermKeyTerm term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiTermKeyTerm term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiExpressionTerminals term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTerminals '+' operators for BnfExpression

	public partial class BnfiExpressionTerminals
	{
        public static BnfiExpressionTerminals operator +(BnfiExpressionTerminals term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermNoAst term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiExpressionTerminals term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiTermKeyTerm term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiExpressionTerminals term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(Terminal term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiExpressionTerminals term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(GrammarHint term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator +(BnfiExpressionTerminals term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTerminals '|' operators for BnfExpression

	public partial class BnfiExpressionTerminals
	{
        public static BnfiExpressionTerminals operator |(BnfiExpressionTerminals term1, Terminal term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(Terminal term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiExpressionTerminals term1, GrammarHint term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(GrammarHint term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTerminals operator |(BnfiExpressionTerminals term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTerminals)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionKeyTermPunctuations definition and operators

	#region BnfiExpressionKeyTermPunctuations definition

	public partial class BnfiExpressionKeyTermPunctuations : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionKeyTermPunctuations()
		{
		}

		public BnfiExpressionKeyTermPunctuations(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionKeyTermPunctuations(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionKeyTermPunctuations(BnfExpression bnfExpression)
		{
			return new BnfiExpressionKeyTermPunctuations(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermKeyTermPunctuation]: implicit conversions from BnfiExpressionKeyTermPunctuations

	public partial class BnfiTermKeyTermPunctuation
	{
		public static implicit operator BnfiExpressionKeyTermPunctuations(BnfiTermKeyTermPunctuation term)
		{
			return new BnfiExpressionKeyTermPunctuations((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionKeyTermPunctuations: implicit conversions from [GrammarHint]

	public partial class BnfiExpressionKeyTermPunctuations
	{
		public static implicit operator BnfiExpressionKeyTermPunctuations(GrammarHint term)
		{
			return new BnfiExpressionKeyTermPunctuations((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermKeyTermPunctuation '+' operators for BnfExpression

	public partial class BnfiTermKeyTermPunctuation
	{
        public static BnfiExpressionKeyTermPunctuations operator +(BnfiTermKeyTermPunctuation term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(BnfiTermKeyTermPunctuation term1, GrammarHint term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(GrammarHint term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermKeyTermPunctuation '|' operators for BnfExpression

	public partial class BnfiTermKeyTermPunctuation
	{
        public static BnfiExpressionKeyTermPunctuations operator |(BnfiTermKeyTermPunctuation term1, GrammarHint term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(GrammarHint term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(BnfiTermKeyTermPunctuation term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(BnfiTermKeyTermPunctuation term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(BnfiExpressionKeyTermPunctuations term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionKeyTermPunctuations '+' operators for BnfExpression

	public partial class BnfiExpressionKeyTermPunctuations
	{
        public static BnfiExpressionKeyTermPunctuations operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(BnfiExpressionKeyTermPunctuations term1, GrammarHint term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(GrammarHint term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator +(BnfiExpressionKeyTermPunctuations term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionKeyTermPunctuations '|' operators for BnfExpression

	public partial class BnfiExpressionKeyTermPunctuations
	{
        public static BnfiExpressionKeyTermPunctuations operator |(BnfiExpressionKeyTermPunctuations term1, GrammarHint term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(GrammarHint term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionKeyTermPunctuations operator |(BnfiExpressionKeyTermPunctuations term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionKeyTermPunctuations)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionTypeTL definition and operators

	#region BnfiExpressionTypeTL definition

	public partial class BnfiExpressionTypeTL : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionTypeTL()
		{
		}

		public BnfiExpressionTypeTL(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionTypeTL(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionTypeTL(BnfExpression bnfExpression)
		{
			return new BnfiExpressionTypeTL(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermMember, BnfiTermCopyTL]: implicit conversions from BnfiExpressionTypeTL

	public partial class BnfiTermMember
	{
		public static implicit operator BnfiExpressionTypeTL(BnfiTermMember term)
		{
			return new BnfiExpressionTypeTL((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopyTL
	{
		public static implicit operator BnfiExpressionTypeTL(BnfiTermCopyTL term)
		{
			return new BnfiExpressionTypeTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermMember '+' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermNoAst term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermKeyTerm term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, Terminal term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(Terminal term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, GrammarHint term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(GrammarHint term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTerminals term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyTL '+' operators for BnfExpression

	public partial class BnfiTermCopyTL
	{
        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermNoAst term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermKeyTerm term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, Terminal term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(Terminal term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, GrammarHint term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(GrammarHint term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTerminals term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember '|' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionTypeTL operator |(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiTermMember term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiTermCopyTL term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiTermMember term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiExpressionTypeTL term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyTL '|' operators for BnfExpression

	public partial class BnfiTermCopyTL
	{
        public static BnfiExpressionTypeTL operator |(BnfiTermCopyTL term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiTermCopyTL term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator |(BnfiExpressionTypeTL term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTypeTL '+' operators for BnfExpression

	public partial class BnfiExpressionTypeTL
	{
        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermNoAst term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermKeyTerm term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, Terminal term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(Terminal term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, GrammarHint term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(GrammarHint term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTerminals term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiTermMember term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermMember term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiTermCopyTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiTermCopyTL term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTypeTL operator +(BnfiExpressionTypeTL term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTypeTL '|' operators for BnfExpression

	public partial class BnfiExpressionTypeTL
	{
        public static BnfiExpressionTypeTL operator |(BnfiExpressionTypeTL term1, BnfiExpressionTypeTL term2)
        {
            return (BnfiExpressionTypeTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionType<T> definition and operators

	#region BnfiExpressionType<T> definition

	public partial class BnfiExpressionType<T> : BnfiExpression, IBnfiExpression<T>, IBnfiTermPlusAbleForType<T>
	{
		#region Construction

		public BnfiExpressionType()
		{
		}

		public BnfiExpressionType(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionType(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionType<T>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionType<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermMember<TDeclaringType>, BnfiTermCopy<T>]: implicit conversions from BnfiExpressionType<T>

	public partial class BnfiTermMember<TDeclaringType>
	{
		public static implicit operator BnfiExpressionType<TDeclaringType>(BnfiTermMember<TDeclaringType> term)
		{
			return new BnfiExpressionType<TDeclaringType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopy<T>
	{
		public static implicit operator BnfiExpressionType<T>(BnfiTermCopy<T> term)
		{
			return new BnfiExpressionType<T>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermMember<TDeclaringType> '+' operators for BnfExpression

	public partial class BnfiTermMember<TDeclaringType>
	{
        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermNoAst term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermKeyTerm term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, Terminal term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(Terminal term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, GrammarHint term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(GrammarHint term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiExpressionTerminals term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermCopy<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermCopy<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy<T> '+' operators for BnfExpression

	public partial class BnfiTermCopy<T>
	{
        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermNoAst term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermKeyTerm term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, Terminal term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(Terminal term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(GrammarHint term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionTerminals term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember<TDeclaringType> '|' operators for BnfExpression

	public partial class BnfiTermMember<TDeclaringType>
	{
        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfiTermCopy<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermCopy<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfiExpressionType<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiExpressionType<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopy<T> '|' operators for BnfExpression

	public partial class BnfiTermCopy<T>
	{
        public static BnfiExpressionType<T> operator |(BnfiTermCopy<T> term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(BnfiTermCopy<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(BnfiExpressionType<T> term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType<T> '+' operators for BnfExpression

	public partial class BnfiExpressionType<T>
	{
        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermNoAst term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermKeyTerm term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, Terminal term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(Terminal term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(GrammarHint term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionTerminals term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiTermMember<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermMember<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiTermCopy<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopy<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType<T> '|' operators for BnfExpression

	public partial class BnfiExpressionType<T>
	{
        public static BnfiExpressionType<T> operator |(BnfiExpressionType<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionValueTL definition and operators

	#region BnfiExpressionValueTL definition

	public partial class BnfiExpressionValueTL : BnfiExpression, IBnfiExpressionTL
	{
		#region Construction

		public BnfiExpressionValueTL()
		{
		}

		public BnfiExpressionValueTL(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionValueTL(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionValueTL(BnfExpression bnfExpression)
		{
			return new BnfiExpressionValueTL(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermValue]: implicit conversions from BnfiExpressionValueTL

	public partial class BnfiTermValue
	{
		public static implicit operator BnfiExpressionValueTL(BnfiTermValue term)
		{
			return new BnfiExpressionValueTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermValue '+' operators for BnfExpression

	public partial class BnfiTermValue
	{
        public static BnfiExpressionValueTL operator +(BnfiTermValue term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermNoAst term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermValue term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermKeyTerm term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermValue term1, Terminal term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(Terminal term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermValue term1, GrammarHint term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(GrammarHint term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermValue term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionTerminals term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermValue '|' operators for BnfExpression

	public partial class BnfiTermValue
	{
        public static BnfiExpressionValueTL operator |(BnfiTermValue term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator |(BnfiTermValue term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator |(BnfiExpressionValueTL term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValueTL '+' operators for BnfExpression

	public partial class BnfiExpressionValueTL
	{
        public static BnfiExpressionValueTL operator +(BnfiExpressionValueTL term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermNoAst term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionValueTL term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiTermKeyTerm term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionValueTL term1, Terminal term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(Terminal term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionValueTL term1, GrammarHint term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(GrammarHint term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionValueTL term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValueTL operator +(BnfiExpressionTerminals term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValueTL '|' operators for BnfExpression

	public partial class BnfiExpressionValueTL
	{
        public static BnfiExpressionValueTL operator |(BnfiExpressionValueTL term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionValueTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionValue<T> definition and operators

	#region BnfiExpressionValue<T> definition

	public partial class BnfiExpressionValue<T> : BnfiExpression, IBnfiExpression<T>, IBnfiTermOrAbleForChoice<T>
	{
		#region Construction

		public BnfiExpressionValue()
		{
		}

		public BnfiExpressionValue(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionValue(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionValue<T>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionValue<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermValue<T>]: implicit conversions from BnfiExpressionValue<T>

	public partial class BnfiTermValue<T>
	{
		public static implicit operator BnfiExpressionValue<T>(BnfiTermValue<T> term)
		{
			return new BnfiExpressionValue<T>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermValue<T> '+' operators for BnfExpression

	public partial class BnfiTermValue<T>
	{
        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermNoAst term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermKeyTerm term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, Terminal term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(Terminal term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(GrammarHint term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionTerminals term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermValue<T> '|' operators for BnfExpression

	public partial class BnfiTermValue<T>
	{
        public static BnfiExpressionValue<T> operator |(BnfiTermValue<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator |(BnfiTermValue<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator |(BnfiExpressionValue<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue<T> '+' operators for BnfExpression

	public partial class BnfiExpressionValue<T>
	{
        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermNoAst term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermKeyTerm term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, Terminal term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(Terminal term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(GrammarHint term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionTerminals term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue<T> '|' operators for BnfExpression

	public partial class BnfiExpressionValue<T>
	{
        public static BnfiExpressionValue<T> operator |(BnfiExpressionValue<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
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

	#region [BnfiTermType, BnfiTermChoice, BnfiTermCollection]: implicit conversions from BnfiExpressionChoiceTL

	public partial class BnfiTermType
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermType term)
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

	#region BnfiExpressionChoiceTL: implicit conversions from [BnfiTermValue, BnfiExpressionValueTL, IBnfiTermCollectionTL]

	public partial class BnfiExpressionChoiceTL
	{
		public static implicit operator BnfiExpressionChoiceTL(BnfiTermValue term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoiceTL(BnfiExpressionValueTL term)
		{
			return new BnfiExpressionChoiceTL((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType '+' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermType term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTermPunctuation term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermType term1, GrammarHint term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(GrammarHint term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermType term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '+' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermChoice term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTermPunctuation term1, BnfiTermChoice term2)
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

        public static BnfiExpressionChoiceTL operator +(BnfiTermChoice term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '+' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoiceTL operator +(BnfiTermCollection term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTermPunctuation term1, BnfiTermCollection term2)
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

        public static BnfiExpressionChoiceTL operator +(BnfiTermCollection term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType '|' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermValue term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionValueTL term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(IBnfiTermCollectionTL term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermType term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '|' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermValue term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionValueTL term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermChoice term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(IBnfiTermCollectionTL term1, BnfiTermChoice term2)
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
        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermValue term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionValueTL term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermCollection term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(IBnfiTermCollectionTL term1, BnfiTermCollection term2)
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
        public static BnfiExpressionChoiceTL operator +(BnfiExpressionChoiceTL term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionChoiceTL term2)
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

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionChoiceTL term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator +(BnfiExpressionKeyTermPunctuations term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoiceTL '|' operators for BnfExpression

	public partial class BnfiExpressionChoiceTL
	{
        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiTermValue term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, BnfiExpressionValueTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionValueTL term1, BnfiExpressionChoiceTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(BnfiExpressionChoiceTL term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoiceTL)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoiceTL operator |(IBnfiTermCollectionTL term1, BnfiExpressionChoiceTL term2)
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

	#region BnfiExpressionChoice<T> definition and operators

	#region BnfiExpressionChoice<T> definition

	public partial class BnfiExpressionChoice<T> : BnfiExpression, IBnfiExpression<T>, IBnfiTermOrAbleForChoice<T>
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

		public static explicit operator BnfiExpressionChoice<T>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionChoice<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermType<TType>, BnfiTermConstant<T>, BnfiTermChoice<TType>]: implicit conversions from BnfiExpressionChoice<T>

	public partial class BnfiTermType<TType>
	{
		public static implicit operator BnfiExpressionChoice<TType>(BnfiTermType<TType> term)
		{
			return new BnfiExpressionChoice<TType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant<T>
	{
		public static implicit operator BnfiExpressionChoice<T>(BnfiTermConstant<T> term)
		{
			return new BnfiExpressionChoice<T>((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice<TType>
	{
		public static implicit operator BnfiExpressionChoice<TType>(BnfiTermChoice<TType> term)
		{
			return new BnfiExpressionChoice<TType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionChoice<T>: implicit conversions from [BnfiTermValue<T>, BnfiExpressionValue<T>, IBnfiTermCollectionWithCollectionType<TCollectionType>]

	public partial class BnfiExpressionChoice<T>
	{
		public static implicit operator BnfiExpressionChoice<T>(BnfiTermValue<T> term)
		{
			return new BnfiExpressionChoice<T>((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoice<T>(BnfiExpressionValue<T> term)
		{
			return new BnfiExpressionChoice<T>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType<TType> '+' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionChoice<TType> operator +(BnfiTermType<TType> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermType<TType> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(GrammarHint term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermType<TType> term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '+' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionChoice<T> operator +(BnfiTermConstant<T> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiTermConstant<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(GrammarHint term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiTermConstant<T> term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TType> '+' operators for BnfExpression

	public partial class BnfiTermChoice<TType>
	{
        public static BnfiExpressionChoice<TType> operator +(BnfiTermChoice<TType> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermChoice<TType> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(GrammarHint term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiTermChoice<TType> term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType<TType> '|' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiTermValue<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermValue<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiExpressionValue<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiExpressionValue<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, IBnfiTermCollectionWithCollectionType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(IBnfiTermCollectionWithCollectionType<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiTermConstant<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermConstant<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiExpressionChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiExpressionChoice<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '|' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermValue<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionValue<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, IBnfiTermCollectionWithCollectionType<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(IBnfiTermCollectionWithCollectionType<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiTermChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermChoice<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionChoice<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TType> '|' operators for BnfExpression

	public partial class BnfiTermChoice<TType>
	{
        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiTermValue<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermValue<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiExpressionValue<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiExpressionValue<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, IBnfiTermCollectionWithCollectionType<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(IBnfiTermCollectionWithCollectionType<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiExpressionChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiExpressionChoice<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice<T> '+' operators for BnfExpression

	public partial class BnfiExpressionChoice<T>
	{
        public static BnfiExpressionChoice<T> operator +(BnfiExpressionChoice<T> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiExpressionChoice<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(GrammarHint term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiExpressionChoice<T> term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator +(BnfiExpressionKeyTermPunctuations term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice<T> '|' operators for BnfExpression

	public partial class BnfiExpressionChoice<T>
	{
        public static BnfiExpressionChoice<T> operator |(BnfiExpressionChoice<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermValue<T> term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionChoice<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionValue<T> term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionChoice<T> term1, IBnfiTermCollectionWithCollectionType<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(IBnfiTermCollectionWithCollectionType<T> term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiExpressionChoice<T> term1, BnfiExpressionChoice<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionGeneral<T> definition and operators

	#region BnfiExpressionGeneral<T> definition

	public partial class BnfiExpressionGeneral<T> : BnfiExpression, IBnfiExpression<T>
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

		public static explicit operator BnfiExpressionGeneral<T>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionGeneral<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermType<TType>, BnfiTermConstant<T>, BnfiTermChoice<TType>]: implicit conversions from BnfiExpressionGeneral<T>

	public partial class BnfiTermType<TType>
	{
		public static implicit operator BnfiExpressionGeneral<TType>(BnfiTermType<TType> term)
		{
			return new BnfiExpressionGeneral<TType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant<T>
	{
		public static implicit operator BnfiExpressionGeneral<T>(BnfiTermConstant<T> term)
		{
			return new BnfiExpressionGeneral<T>((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice<TType>
	{
		public static implicit operator BnfiExpressionGeneral<TType>(BnfiTermChoice<TType> term)
		{
			return new BnfiExpressionGeneral<TType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType<TType> '+' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionGeneral<TType> operator +(BnfiTermType<TType> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermNoAst term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermType<TType> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermKeyTerm term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '+' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionGeneral<T> operator +(BnfiTermConstant<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiTermNoAst term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiTermConstant<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiTermKeyTerm term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice<TType> '+' operators for BnfExpression

	public partial class BnfiTermChoice<TType>
	{
        public static BnfiExpressionGeneral<TType> operator +(BnfiTermChoice<TType> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermNoAst term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermChoice<TType> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<TType> operator +(BnfiTermKeyTerm term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionGeneral<TType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionGeneral<T> '+' operators for BnfExpression

	public partial class BnfiExpressionGeneral<T>
	{
        public static BnfiExpressionGeneral<T> operator +(BnfiExpressionGeneral<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiTermNoAst term1, BnfiExpressionGeneral<T> term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiExpressionGeneral<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionGeneral<T> operator +(BnfiTermKeyTerm term1, BnfiExpressionGeneral<T> term2)
        {
            return (BnfiExpressionGeneral<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

}
