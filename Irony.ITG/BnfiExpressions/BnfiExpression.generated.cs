 
 
 
 
 
// GENERATED FILE

using System;
using System.Collections.Generic;
using System.Linq;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
	#region BnfiExpressionType definition and operators

	public interface IBnfiExpressionType : IBnfiExpression { }

	#region BnfiExpressionType definition

	public partial class BnfiExpressionType : BnfiExpressionCommon, IBnfiExpressionType
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

	#region [BnfiTermMember]: implicit conversions from BnfiExpressionType

	public partial class BnfiTermMember
	{
		public static implicit operator BnfiExpressionType(BnfiTermMember term)
		{
			return new BnfiExpressionType((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermMember '+' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfTerm term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfTerm term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember '|' operators for BnfExpression

	public partial class BnfiTermMember
	{
        public static BnfiExpressionType operator |(BnfiTermMember term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiTermMember term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator |(BnfiExpressionType term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType '+' operators for BnfExpression

	public partial class BnfiExpressionType
	{
        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfTerm term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfTerm term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiTermMember term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiTermMember term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionType operator +(BnfiExpressionType term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionType '|' operators for BnfExpression

	public partial class BnfiExpressionType
	{
        public static BnfiExpressionType operator |(BnfiExpressionType term1, BnfiExpressionType term2)
        {
            return (BnfiExpressionType)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfExpressionBoundMembers<T> definition and operators

	public interface IBnfExpressionBoundMembers<out T> : IBnfiExpression<T> { }

	#region BnfExpressionBoundMembers<T> definition

	public partial class BnfExpressionBoundMembers<T> : BnfiExpressionType, IBnfExpressionBoundMembers<T>
	{
		#region Construction

		public BnfExpressionBoundMembers()
		{
		}

		public BnfExpressionBoundMembers(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfExpressionBoundMembers(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfExpressionBoundMembers<T>(BnfExpression bnfExpression)
		{
			return new BnfExpressionBoundMembers<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermMember<TDeclaringType>]: implicit conversions from BnfExpressionBoundMembers<T>

	public partial class BnfiTermMember<TDeclaringType>
	{
		public static implicit operator BnfExpressionBoundMembers<TDeclaringType>(BnfiTermMember<TDeclaringType> term)
		{
			return new BnfExpressionBoundMembers<TDeclaringType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermMember<TDeclaringType> '+' operators for BnfExpression

	public partial class BnfiTermMember<TDeclaringType>
	{
        public static BnfExpressionBoundMembers<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfTerm term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator +(BnfTerm term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator +(BnfiTermMember<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermMember<TDeclaringType> '|' operators for BnfExpression

	public partial class BnfiTermMember<TDeclaringType>
	{
        public static BnfExpressionBoundMembers<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator |(BnfiTermMember<TDeclaringType> term1, BnfExpressionBoundMembers<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator |(BnfExpressionBoundMembers<TDeclaringType> term1, BnfiTermMember<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionBoundMembers<T> '+' operators for BnfExpression

	public partial class BnfExpressionBoundMembers<T>
	{
        public static BnfExpressionBoundMembers<T> operator +(BnfExpressionBoundMembers<T> term1, BnfTerm term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<T> operator +(BnfTerm term1, BnfExpressionBoundMembers<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<T> operator +(BnfExpressionBoundMembers<T> term1, BnfiTermMember<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<T> operator +(BnfiTermMember<T> term1, BnfExpressionBoundMembers<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<T> operator +(BnfExpressionBoundMembers<T> term1, BnfExpressionBoundMembers<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionBoundMembers<T> '|' operators for BnfExpression

	public partial class BnfExpressionBoundMembers<T>
	{
        public static BnfExpressionBoundMembers<T> operator |(BnfExpressionBoundMembers<T> term1, BnfExpressionBoundMembers<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionTransient definition and operators

	public interface IBnfiExpressionTransient : IBnfiExpression { }

	#region BnfiExpressionTransient definition

	public partial class BnfiExpressionTransient : BnfiExpressionCommon, IBnfiExpressionTransient
	{
		#region Construction

		public BnfiExpressionTransient()
		{
		}

		public BnfiExpressionTransient(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionTransient(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionTransient(BnfExpression bnfExpression)
		{
			return new BnfiExpressionTransient(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermType, BnfiTermTransient]: implicit conversions from BnfiExpressionTransient

	public partial class BnfiTermType
	{
		public static implicit operator BnfiExpressionTransient(BnfiTermType term)
		{
			return new BnfiExpressionTransient((BnfTerm)term);
		}
	}

	public partial class BnfiTermTransient
	{
		public static implicit operator BnfiExpressionTransient(BnfiTermTransient term)
		{
			return new BnfiExpressionTransient((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionTransient: implicit conversions from [BnfiExpressionCollection, BnfiExpressionValue, BnfTerm]

	public partial class BnfiExpressionTransient
	{
		public static implicit operator BnfiExpressionTransient(BnfiExpressionCollection term)
		{
			return new BnfiExpressionTransient((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionTransient(BnfiExpressionValue term)
		{
			return new BnfiExpressionTransient((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionTransient(BnfTerm term)
		{
			return new BnfiExpressionTransient((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType '+' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionTransient operator +(BnfiTermType term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiTermKeyTermPunctuation term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiTermType term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(GrammarHint term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient '+' operators for BnfExpression

	public partial class BnfiTermTransient
	{
        public static BnfiExpressionTransient operator +(BnfiTermTransient term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiTermKeyTermPunctuation term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiTermTransient term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(GrammarHint term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType '|' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient '|' operators for BnfExpression

	public partial class BnfiTermTransient
	{
        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType '|' operators for BnfExpression

	public partial class BnfiTermType
	{
        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionCollection term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionValue term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermType term1, BnfTerm term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfTerm term1, BnfiTermType term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient '|' operators for BnfExpression

	public partial class BnfiTermTransient
	{
        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionCollection term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionValue term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiTermTransient term1, BnfTerm term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfTerm term1, BnfiTermTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTransient '+' operators for BnfExpression

	public partial class BnfiExpressionTransient
	{
        public static BnfiExpressionTransient operator +(BnfiExpressionTransient term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(BnfiExpressionTransient term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator +(GrammarHint term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTransient '|' operators for BnfExpression

	public partial class BnfiExpressionTransient
	{
        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionCollection term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionValue term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfTerm term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfTerm term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient operator |(BnfiExpressionTransient term1, BnfiExpressionTransient term2)
        {
            return (BnfiExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionTransient<T> definition and operators

	public interface IBnfiExpressionTransient<out T> : IBnfiExpression<T> { }

	#region BnfiExpressionTransient<T> definition

	public partial class BnfiExpressionTransient<T> : BnfiExpressionTransient, IBnfiExpressionTransient<T>
	{
		#region Construction

		public BnfiExpressionTransient()
		{
		}

		public BnfiExpressionTransient(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionTransient(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionTransient<T>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionTransient<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermType<TType>, BnfiTermConstant<T>, BnfiTermTransient<TType>]: implicit conversions from BnfiExpressionTransient<T>

	public partial class BnfiTermType<TType>
	{
		public static implicit operator BnfiExpressionTransient<TType>(BnfiTermType<TType> term)
		{
			return new BnfiExpressionTransient<TType>((BnfTerm)term);
		}
	}

	public partial class BnfiTermConstant<T>
	{
		public static implicit operator BnfiExpressionTransient<T>(BnfiTermConstant<T> term)
		{
			return new BnfiExpressionTransient<T>((BnfTerm)term);
		}
	}

	public partial class BnfiTermTransient<TType>
	{
		public static implicit operator BnfiExpressionTransient<TType>(BnfiTermTransient<TType> term)
		{
			return new BnfiExpressionTransient<TType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiExpressionTransient<T>: implicit conversions from [BnfiExpressionCollection<TCollectionType>, BnfiExpressionValue<T>]

	public partial class BnfiExpressionTransient<T>
	{
		public static implicit operator BnfiExpressionTransient<T>(BnfiExpressionCollection<T> term)
		{
			return new BnfiExpressionTransient<T>((BnfTerm)term);
		}
		public static implicit operator BnfiExpressionTransient<T>(BnfiExpressionValue<T> term)
		{
			return new BnfiExpressionTransient<T>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermType<TType> '+' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionTransient<TType> operator +(BnfiTermType<TType> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(BnfiTermType<TType> term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(GrammarHint term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '+' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionTransient<T> operator +(BnfiTermConstant<T> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(BnfiTermConstant<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(GrammarHint term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient<TType> '+' operators for BnfExpression

	public partial class BnfiTermTransient<TType>
	{
        public static BnfiExpressionTransient<TType> operator +(BnfiTermTransient<TType> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(BnfiTermKeyTermPunctuation term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(BnfiTermTransient<TType> term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator +(GrammarHint term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType<TType> '|' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiTermConstant<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermConstant<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermTransient<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiExpressionTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionTransient<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '|' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionTransient<T> operator |(BnfiTermConstant<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiTermConstant<T> term1, BnfiTermTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiTermTransient<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiTermConstant<T> term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionTransient<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient<TType> '|' operators for BnfExpression

	public partial class BnfiTermTransient<TType>
	{
        public static BnfiExpressionTransient<TType> operator |(BnfiTermTransient<TType> term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermTransient<TType> term1, BnfiExpressionTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionTransient<TType> term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermType<TType> '|' operators for BnfExpression

	public partial class BnfiTermType<TType>
	{
        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiExpressionCollection<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionCollection<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermType<TType> term1, BnfiExpressionValue<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionValue<TType> term1, BnfiTermType<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermConstant<T> '|' operators for BnfExpression

	public partial class BnfiTermConstant<T>
	{
        public static BnfiExpressionTransient<T> operator |(BnfiTermConstant<T> term1, BnfiExpressionCollection<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionCollection<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiTermConstant<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionValue<T> term1, BnfiTermConstant<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermTransient<TType> '|' operators for BnfExpression

	public partial class BnfiTermTransient<TType>
	{
        public static BnfiExpressionTransient<TType> operator |(BnfiTermTransient<TType> term1, BnfiExpressionCollection<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionCollection<TType> term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiTermTransient<TType> term1, BnfiExpressionValue<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<TType> operator |(BnfiExpressionValue<TType> term1, BnfiTermTransient<TType> term2)
        {
            return (BnfiExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTransient<T> '+' operators for BnfExpression

	public partial class BnfiExpressionTransient<T>
	{
        public static BnfiExpressionTransient<T> operator +(BnfiExpressionTransient<T> term1, BnfiTermKeyTermPunctuation term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(BnfiTermKeyTermPunctuation term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(BnfiExpressionTransient<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator +(GrammarHint term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionTransient<T> '|' operators for BnfExpression

	public partial class BnfiExpressionTransient<T>
	{
        public static BnfiExpressionTransient<T> operator |(BnfiExpressionTransient<T> term1, BnfiExpressionCollection<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionCollection<T> term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionTransient<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionValue<T> term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionTransient<T> operator |(BnfiExpressionTransient<T> term1, BnfiExpressionTransient<T> term2)
        {
            return (BnfiExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionCollection definition and operators

	public interface IBnfiExpressionCollection : IBnfiExpression { }

	#region BnfiExpressionCollection definition

	public partial class BnfiExpressionCollection : BnfiExpressionCommon, IBnfiExpressionCollection
	{
		#region Construction

		public BnfiExpressionCollection()
		{
		}

		public BnfiExpressionCollection(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionCollection(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionCollection(BnfExpression bnfExpression)
		{
			return new BnfiExpressionCollection(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermCollection]: implicit conversions from BnfiExpressionCollection

	public partial class BnfiTermCollection
	{
		public static implicit operator BnfiExpressionCollection(BnfiTermCollection term)
		{
			return new BnfiExpressionCollection((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermCollection '+' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionCollection operator +(BnfiTermCollection term1, GrammarHint term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection operator +(GrammarHint term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection '|' operators for BnfExpression

	public partial class BnfiTermCollection
	{
        public static BnfiExpressionCollection operator |(BnfiTermCollection term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection operator |(BnfiTermCollection term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection operator |(BnfiExpressionCollection term1, BnfiTermCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionCollection '+' operators for BnfExpression

	public partial class BnfiExpressionCollection
	{
        public static BnfiExpressionCollection operator +(BnfiExpressionCollection term1, GrammarHint term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection operator +(GrammarHint term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionCollection '|' operators for BnfExpression

	public partial class BnfiExpressionCollection
	{
        public static BnfiExpressionCollection operator |(BnfiExpressionCollection term1, BnfiExpressionCollection term2)
        {
            return (BnfiExpressionCollection)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionCollection<TCollectionType> definition and operators

	public interface IBnfiExpressionCollection<out TCollectionType> : IBnfiExpression<TCollectionType> { }

	#region BnfiExpressionCollection<TCollectionType> definition

	public partial class BnfiExpressionCollection<TCollectionType> : BnfiExpressionCollection, IBnfiExpressionCollection<TCollectionType>
	{
		#region Construction

		public BnfiExpressionCollection()
		{
		}

		public BnfiExpressionCollection(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfiExpressionCollection(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfiExpressionCollection<TCollectionType>(BnfExpression bnfExpression)
		{
			return new BnfiExpressionCollection<TCollectionType>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [BnfiTermCollection<TCollectionType, TElementType>]: implicit conversions from BnfiExpressionCollection<TCollectionType>

	public partial class BnfiTermCollection<TCollectionType, TElementType>
	{
		public static implicit operator BnfiExpressionCollection<TCollectionType>(BnfiTermCollection<TCollectionType, TElementType> term)
		{
			return new BnfiExpressionCollection<TCollectionType>((BnfTerm)term);
		}
	}

	#endregion

	#region BnfiTermCollection<TCollectionType, TElementType> '+' operators for BnfExpression

	public partial class BnfiTermCollection<TCollectionType, TElementType>
	{
        public static BnfiExpressionCollection<TCollectionType> operator +(BnfiTermCollection<TCollectionType, TElementType> term1, GrammarHint term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection<TCollectionType> operator +(GrammarHint term1, BnfiTermCollection<TCollectionType, TElementType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermCollection<TCollectionType, TElementType> '|' operators for BnfExpression

	public partial class BnfiTermCollection<TCollectionType, TElementType>
	{
        public static BnfiExpressionCollection<TCollectionType> operator |(BnfiTermCollection<TCollectionType, TElementType> term1, BnfiTermCollection<TCollectionType, TElementType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection<TCollectionType> operator |(BnfiTermCollection<TCollectionType, TElementType> term1, BnfiExpressionCollection<TCollectionType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection<TCollectionType> operator |(BnfiExpressionCollection<TCollectionType> term1, BnfiTermCollection<TCollectionType, TElementType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionCollection<TCollectionType> '+' operators for BnfExpression

	public partial class BnfiExpressionCollection<TCollectionType>
	{
        public static BnfiExpressionCollection<TCollectionType> operator +(BnfiExpressionCollection<TCollectionType> term1, GrammarHint term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionCollection<TCollectionType> operator +(GrammarHint term1, BnfiExpressionCollection<TCollectionType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionCollection<TCollectionType> '|' operators for BnfExpression

	public partial class BnfiExpressionCollection<TCollectionType>
	{
        public static BnfiExpressionCollection<TCollectionType> operator |(BnfiExpressionCollection<TCollectionType> term1, BnfiExpressionCollection<TCollectionType> term2)
        {
            return (BnfiExpressionCollection<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfiExpressionValue definition and operators

	public interface IBnfiExpressionValue : IBnfiExpression { }

	#region BnfiExpressionValue definition

	public partial class BnfiExpressionValue : BnfiExpressionCommon, IBnfiExpressionValue
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
        public static BnfiExpressionValue operator +(BnfiTermValue term1, KeyTerm term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(KeyTerm term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiTermValue term1, GrammarHint term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(GrammarHint term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermValue '|' operators for BnfExpression

	public partial class BnfiTermValue
	{
        public static BnfiExpressionValue operator |(BnfiTermValue term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator |(BnfiTermValue term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator |(BnfiExpressionValue term1, BnfiTermValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue '+' operators for BnfExpression

	public partial class BnfiExpressionValue
	{
        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, KeyTerm term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(KeyTerm term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(BnfiExpressionValue term1, GrammarHint term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue operator +(GrammarHint term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue '|' operators for BnfExpression

	public partial class BnfiExpressionValue
	{
        public static BnfiExpressionValue operator |(BnfiExpressionValue term1, BnfiExpressionValue term2)
        {
            return (BnfiExpressionValue)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
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
        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, KeyTerm term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(KeyTerm term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiTermValue<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(GrammarHint term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiTermValue<T> '|' operators for BnfExpression

	public partial class BnfiTermValue<T>
	{
        public static BnfiExpressionValue<T> operator |(BnfiTermValue<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator |(BnfiTermValue<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator |(BnfiExpressionValue<T> term1, BnfiTermValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue<T> '+' operators for BnfExpression

	public partial class BnfiExpressionValue<T>
	{
        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, KeyTerm term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(KeyTerm term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(BnfiExpressionValue<T> term1, GrammarHint term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfiExpressionValue<T> operator +(GrammarHint term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfiExpressionValue<T> '|' operators for BnfExpression

	public partial class BnfiExpressionValue<T>
	{
        public static BnfiExpressionValue<T> operator |(BnfiExpressionValue<T> term1, BnfiExpressionValue<T> term2)
        {
            return (BnfiExpressionValue<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

}
