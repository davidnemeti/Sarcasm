 
 
 
 
 
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

	public interface IBnfiExpressionTerminals : IBnfiExpressionTL { }

	#region BnfiExpressionTerminals definition

	public partial class BnfiExpressionTerminals : BnfiExpression, IBnfiExpressionTerminals
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

	public interface IBnfiExpressionKeyTermPunctuations : IBnfiExpressionTL { }

	#region BnfiExpressionKeyTermPunctuations definition

	public partial class BnfiExpressionKeyTermPunctuations : BnfiExpression, IBnfiExpressionKeyTermPunctuations
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

	#region BnfiExpressionType definition and operators

	public interface IBnfiExpressionType : IBnfiExpressionTL { }

	#region BnfiExpressionType definition

	public partial class BnfiExpressionType : BnfiExpression, IBnfiExpressionType
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

		public static explicit operator BnfiExpressionType(BnfExpression bnfExpression)
		{
			return new BnfiExpressionType(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermMember, BnfiTermCopyable]: implicit conversions from BnfiExpressionType

	public partial class BnfiTermMember
	{
		public static implicit operator BnfiExpressionType(BnfiTermMember term)
		{
			return new BnfiExpressionType((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopyable
	{
		public static implicit operator BnfiExpressionType(BnfiTermCopyable term)
		{
			return new BnfiExpressionType((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionType: implicit conversions from [IBnfiTermCopyable]

	public partial class BnfiExpressionType
	{
	}

	#endregion

	#region BnfiTermMember '+' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermNoAst term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermKeyTerm term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, Terminal term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(Terminal term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, GrammarHint term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(GrammarHint term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionTerminals term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(IBnfiTermCopyable term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyable '+' operators for BnfExpression

	public partial class BnfiTermCopyable
	{
        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermNoAst term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermKeyTerm term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, Terminal term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(Terminal term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, GrammarHint term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(GrammarHint term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionTerminals term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(IBnfiTermCopyable term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember '|' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionType operator |(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiTermMember term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiTermCopyable term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiTermMember term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiExpressionType term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyable '|' operators for BnfExpression

	public partial class BnfiTermCopyable
	{
        public static BnfiExpressionType operator |(BnfiTermCopyable term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiTermCopyable term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiExpressionType term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember '|' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionType operator |(BnfiTermMember term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(IBnfiTermCopyable term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyable '|' operators for BnfExpression

	public partial class BnfiTermCopyable
	{
        public static BnfiExpressionType operator |(BnfiTermCopyable term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(IBnfiTermCopyable term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType '+' operators for BnfExpression

	public partial class BnfiExpressionType
	{
        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermNoAst term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermKeyTerm term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, Terminal term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(Terminal term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, GrammarHint term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(GrammarHint term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionTerminals term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermCopyable term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(IBnfiTermCopyable term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType '|' operators for BnfExpression

	public partial class BnfiExpressionType
	{
        public static BnfiExpressionType operator |(BnfiExpressionType term1, IBnfiTermCopyable term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(IBnfiTermCopyable term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiExpressionType term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionType<T> definition and operators

	public interface IBnfiExpressionType<out T> : IBnfiExpression<T> { }

	#region BnfiExpressionType<T> definition

	public partial class BnfiExpressionType<T> : BnfiExpressionType, IBnfiExpressionType<T>
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

	#region [BnfiTermMember<TDeclaringType>, BnfiTermCopyable<T>]: implicit conversions from BnfiExpressionType<T>

	public partial class BnfiTermMember<TDeclaringType>
	{
		public static implicit operator BnfiExpressionType<TDeclaringType>(BnfiTermMember<TDeclaringType> term)
		{
			return new BnfiExpressionType<TDeclaringType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermCopyable<T>
	{
		public static implicit operator BnfiExpressionType<T>(BnfiTermCopyable<T> term)
		{
			return new BnfiExpressionType<T>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionType<T>: implicit conversions from [IBnfiTermCopyable<T>]

	public partial class BnfiExpressionType<T>
	{
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

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermCopyable<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermCopyable<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, IBnfiTermCopyable<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator +(IBnfiTermCopyable<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyable<T> '+' operators for BnfExpression

	public partial class BnfiTermCopyable<T>
	{
        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermNoAst term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermKeyTerm term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, Terminal term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(Terminal term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(GrammarHint term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionTerminals term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, IBnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(IBnfiTermCopyable<T> term1, BnfiTermCopyable<T> term2)
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

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfiTermCopyable<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermCopyable<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
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

	#region BnfiTermCopyable<T> '|' operators for BnfExpression

	public partial class BnfiTermCopyable<T>
	{
        public static BnfiExpressionType<T> operator |(BnfiTermCopyable<T> term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(BnfiTermCopyable<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(BnfiExpressionType<T> term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember<TDeclaringType> '|' operators for BnfExpression

	public partial class BnfiTermMember<TDeclaringType>
	{
        public static BnfiExpressionType<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, IBnfiTermCopyable<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<TDeclaringType> operator |(IBnfiTermCopyable<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfiExpressionType<TDeclaringType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCopyable<T> '|' operators for BnfExpression

	public partial class BnfiTermCopyable<T>
	{
        public static BnfiExpressionType<T> operator |(BnfiTermCopyable<T> term1, IBnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(IBnfiTermCopyable<T> term1, BnfiTermCopyable<T> term2)
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

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, BnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiTermCopyable<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(BnfiExpressionType<T> term1, IBnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator +(IBnfiTermCopyable<T> term1, BnfiExpressionType<T> term2)
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
        public static BnfiExpressionType<T> operator |(BnfiExpressionType<T> term1, IBnfiTermCopyable<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(IBnfiTermCopyable<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType<T> operator |(BnfiExpressionType<T> term1, BnfiExpressionType<T> term2)
        {
            return (BnfiExpressionType<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionValue definition and operators

	public interface IBnfiExpressionValue : IBnfiExpressionTL { }

	#region BnfiExpressionValue definition

	public partial class BnfiExpressionValue : BnfiExpression, IBnfiExpressionValue
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

		public static explicit operator BnfiExpressionValue(BnfExpression bnfExpression)
		{
			return new BnfiExpressionValue(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermValue]: implicit conversions from BnfiExpressionValue

	public partial class BnfiTermValue
	{
		public static implicit operator BnfiExpressionValue(BnfiTermValue term)
		{
			return new BnfiExpressionValue((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermValue '+' operators for BnfExpression

	public partial class BnfiTermValue
	{
        public static BnfiExpressionValue operator +(BnfiTermValue term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermNoAst term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermValue term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermKeyTerm term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermValue term1, Terminal term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(Terminal term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermValue term1, GrammarHint term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(GrammarHint term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermValue term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionTerminals term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermValue '|' operators for BnfExpression

	public partial class BnfiTermValue
	{
        public static BnfiExpressionValue operator |(BnfiTermValue term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator |(BnfiTermValue term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator |(BnfiExpressionValue term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue '+' operators for BnfExpression

	public partial class BnfiExpressionValue
	{
        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, BnfiTermNoAst term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermNoAst term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, BnfiTermKeyTerm term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermKeyTerm term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, Terminal term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(Terminal term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, GrammarHint term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(GrammarHint term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, BnfiExpressionTerminals term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionTerminals term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue '|' operators for BnfExpression

	public partial class BnfiExpressionValue
	{
        public static BnfiExpressionValue operator |(BnfiExpressionValue term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionValue<T> definition and operators

	public interface IBnfiExpressionValue<out T> : IBnfiExpression<T> { }

	#region BnfiExpressionValue<T> definition

	public partial class BnfiExpressionValue<T> : BnfiExpressionValue, IBnfiExpressionValue<T>
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

	#region BnfiExpressionChoice definition and operators

	public interface IBnfiExpressionChoice : IBnfiExpressionTL { }

	#region BnfiExpressionChoice definition

	public partial class BnfiExpressionChoice : BnfiExpression, IBnfiExpressionChoice
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

		public static explicit operator BnfiExpressionChoice(BnfExpression bnfExpression)
		{
			return new BnfiExpressionChoice(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermType, BnfiTermChoice, BnfiTermCollection]: implicit conversions from BnfiExpressionChoice

	public partial class BnfiTermType
	{
		public static implicit operator BnfiExpressionChoice(BnfiTermType term)
		{
			return new BnfiExpressionChoice((BnfTerm)term);
		}
	}

	public partial class BnfiTermChoice
	{
		public static implicit operator BnfiExpressionChoice(BnfiTermChoice term)
		{
			return new BnfiExpressionChoice((BnfTerm)term);
		}
	}

	public partial class BnfiTermCollection
	{
		public static implicit operator BnfiExpressionChoice(BnfiTermCollection term)
		{
			return new BnfiExpressionChoice((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionChoice: implicit conversions from [BnfiTermValue, BnfiExpressionValue, IBnfiTermCollectionTL]

	public partial class BnfiExpressionChoice
	{
		public static implicit operator BnfiExpressionChoice(BnfiTermValue term)
		{
			return new BnfiExpressionChoice((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionChoice(BnfiExpressionValue term)
		{
			return new BnfiExpressionChoice((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType '+' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionChoice operator +(BnfiTermType term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermKeyTermPunctuation term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermType term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(GrammarHint term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermType term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '+' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoice operator +(BnfiTermChoice term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermKeyTermPunctuation term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermChoice term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(GrammarHint term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermChoice term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '+' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoice operator +(BnfiTermCollection term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermKeyTermPunctuation term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermCollection term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(GrammarHint term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermCollection term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType '|' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '|' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '|' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType '|' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermValue term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermType term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionValue term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermType term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(IBnfiTermCollectionTL term1, BnfiTermType term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermChoice '|' operators for BnfExpression

	public partial class BnfiTermChoice
	{
        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermValue term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionValue term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermChoice term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(IBnfiTermCollectionTL term1, BnfiTermChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '|' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermValue term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionValue term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermCollection term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(IBnfiTermCollectionTL term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice '+' operators for BnfExpression

	public partial class BnfiExpressionChoice
	{
        public static BnfiExpressionChoice operator +(BnfiExpressionChoice term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionChoice term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(GrammarHint term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionChoice term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator +(BnfiExpressionKeyTermPunctuations term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionChoice '|' operators for BnfExpression

	public partial class BnfiExpressionChoice
	{
        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiTermValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiTermValue term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionValue term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, IBnfiTermCollectionTL term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(IBnfiTermCollectionTL term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice operator |(BnfiExpressionChoice term1, BnfiExpressionChoice term2)
        {
            return (BnfiExpressionChoice)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionChoice<T> definition and operators

	public interface IBnfiExpressionChoice<out T> : IBnfiExpression<T> { }

	#region BnfiExpressionChoice<T> definition

	public partial class BnfiExpressionChoice<T> : BnfiExpressionChoice, IBnfiExpressionChoice<T>
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

	#region [BnfiTermType<TType>, BnfiTermConstant<T>, BnfiTermChoice<TType>, BnfiTermCollection<TCollectionType>]: implicit conversions from BnfiExpressionChoice<T>

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

	public partial class BnfiTermCollection<TCollectionType>
	{
		public static implicit operator BnfiExpressionChoice<TCollectionType>(BnfiTermCollection<TCollectionType> term)
		{
			return new BnfiExpressionChoice<TCollectionType>((BnfTerm)term);
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

	#region BnfiTermCollection<TCollectionType> '+' operators for BnfExpression

	public partial class BnfiTermCollection<TCollectionType>
	{
        public static BnfiExpressionChoice<TCollectionType> operator +(BnfiTermCollection<TCollectionType> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator +(BnfiTermCollection<TCollectionType> term1, GrammarHint term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator +(GrammarHint term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator +(BnfiTermCollection<TCollectionType> term1, BnfiExpressionKeyTermPunctuations term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator +(BnfiExpressionKeyTermPunctuations term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType<TType> '|' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
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

        public static BnfiExpressionChoice<TType> operator |(BnfiTermType<TType> term1, BnfiTermCollection<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermCollection<TType> term1, BnfiTermType<TType> term2)
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

        public static BnfiExpressionChoice<T> operator |(BnfiTermConstant<T> term1, BnfiTermCollection<T> term2)
        {
            return (BnfiExpressionChoice<T>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<T> operator |(BnfiTermCollection<T> term1, BnfiTermConstant<T> term2)
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
        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiTermChoice<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermChoice<TType> term1, BnfiTermCollection<TType> term2)
        {
            return (BnfiExpressionChoice<TType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TType> operator |(BnfiTermCollection<TType> term1, BnfiTermChoice<TType> term2)
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

	#region BnfiTermCollection<TCollectionType> '|' operators for BnfExpression

	public partial class BnfiTermCollection<TCollectionType>
	{
        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermCollection<TCollectionType> term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermCollection<TCollectionType> term1, BnfiExpressionChoice<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiExpressionChoice<TCollectionType> term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
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

	}

	#endregion

	#region BnfiTermCollection<TCollectionType> '|' operators for BnfExpression

	public partial class BnfiTermCollection<TCollectionType>
	{
        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermCollection<TCollectionType> term1, BnfiTermValue<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermValue<TCollectionType> term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermCollection<TCollectionType> term1, BnfiExpressionValue<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiExpressionValue<TCollectionType> term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(BnfiTermCollection<TCollectionType> term1, IBnfiTermCollectionWithCollectionType<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionChoice<TCollectionType> operator |(IBnfiTermCollectionWithCollectionType<TCollectionType> term1, BnfiTermCollection<TCollectionType> term2)
        {
            return (BnfiExpressionChoice<TCollectionType>)BnfiExpression.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
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

}
