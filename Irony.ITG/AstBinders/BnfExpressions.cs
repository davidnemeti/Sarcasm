 
 
 
 
 
// GENERATED FILE

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

	#region BnfExpressionBoundMembers definition and operators

	public interface IBnfExpressionBoundMembers : IBnfExpression { }

	#region BnfExpressionBoundMembers definition

	public partial class BnfExpressionBoundMembers : BnfExpressionCommon, IBnfExpressionBoundMembers
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

		public static explicit operator BnfExpressionBoundMembers(BnfExpression bnfExpression)
		{
			return new BnfExpressionBoundMembers(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [MemberBoundToBnfTerm]: implicit conversions from BnfExpressionBoundMembers

	public partial class MemberBoundToBnfTerm
	{
		public static implicit operator BnfExpressionBoundMembers(MemberBoundToBnfTerm term)
		{
			return new BnfExpressionBoundMembers(term.AsBnfTerm());
		}
	}

	#endregion

	#region MemberBoundToBnfTerm '+' operators for BnfExpression

	public partial class MemberBoundToBnfTerm
	{
        public static BnfExpressionBoundMembers operator +(MemberBoundToBnfTerm term1, BnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(BnfTerm term1, MemberBoundToBnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(MemberBoundToBnfTerm term1, MemberBoundToBnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region MemberBoundToBnfTerm '|' operators for BnfExpression

	public partial class MemberBoundToBnfTerm
	{
        public static BnfExpressionBoundMembers operator |(MemberBoundToBnfTerm term1, MemberBoundToBnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator |(MemberBoundToBnfTerm term1, BnfExpressionBoundMembers term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator |(BnfExpressionBoundMembers term1, MemberBoundToBnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionBoundMembers '+' operators for BnfExpression

	public partial class BnfExpressionBoundMembers
	{
        public static BnfExpressionBoundMembers operator +(BnfExpressionBoundMembers term1, BnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(BnfTerm term1, BnfExpressionBoundMembers term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(BnfExpressionBoundMembers term1, MemberBoundToBnfTerm term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(MemberBoundToBnfTerm term1, BnfExpressionBoundMembers term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers operator +(BnfExpressionBoundMembers term1, BnfExpressionBoundMembers term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionBoundMembers '|' operators for BnfExpression

	public partial class BnfExpressionBoundMembers
	{
        public static BnfExpressionBoundMembers operator |(BnfExpressionBoundMembers term1, BnfExpressionBoundMembers term2)
        {
            return (BnfExpressionBoundMembers)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfExpressionBoundMembers<T> definition and operators

	public interface IBnfExpressionBoundMembers<out T> : IBnfExpression<T> { }

	#region BnfExpressionBoundMembers<T> definition

	public partial class BnfExpressionBoundMembers<T> : BnfExpressionBoundMembers, IBnfExpressionBoundMembers<T>
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

	#region [MemberBoundToBnfTerm<TDeclaringType>]: implicit conversions from BnfExpressionBoundMembers<T>

	public partial class MemberBoundToBnfTerm<TDeclaringType>
	{
		public static implicit operator BnfExpressionBoundMembers<TDeclaringType>(MemberBoundToBnfTerm<TDeclaringType> term)
		{
			return new BnfExpressionBoundMembers<TDeclaringType>(term.AsBnfTerm());
		}
	}

	#endregion

	#region MemberBoundToBnfTerm<TDeclaringType> '+' operators for BnfExpression

	public partial class MemberBoundToBnfTerm<TDeclaringType>
	{
        public static BnfExpressionBoundMembers<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> term1, BnfTerm term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator +(BnfTerm term1, MemberBoundToBnfTerm<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> term1, MemberBoundToBnfTerm<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region MemberBoundToBnfTerm<TDeclaringType> '|' operators for BnfExpression

	public partial class MemberBoundToBnfTerm<TDeclaringType>
	{
        public static BnfExpressionBoundMembers<TDeclaringType> operator |(MemberBoundToBnfTerm<TDeclaringType> term1, MemberBoundToBnfTerm<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator |(MemberBoundToBnfTerm<TDeclaringType> term1, BnfExpressionBoundMembers<TDeclaringType> term2)
        {
            return (BnfExpressionBoundMembers<TDeclaringType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<TDeclaringType> operator |(BnfExpressionBoundMembers<TDeclaringType> term1, MemberBoundToBnfTerm<TDeclaringType> term2)
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

        public static BnfExpressionBoundMembers<T> operator +(BnfExpressionBoundMembers<T> term1, MemberBoundToBnfTerm<T> term2)
        {
            return (BnfExpressionBoundMembers<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionBoundMembers<T> operator +(MemberBoundToBnfTerm<T> term1, BnfExpressionBoundMembers<T> term2)
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

	#region BnfExpressionTransient definition and operators

	public interface IBnfExpressionTransient : IBnfExpression { }

	#region BnfExpressionTransient definition

	public partial class BnfExpressionTransient : BnfExpressionCommon, IBnfExpressionTransient
	{
		#region Construction

		public BnfExpressionTransient()
		{
		}

		public BnfExpressionTransient(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfExpressionTransient(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfExpressionTransient(BnfExpression bnfExpression)
		{
			return new BnfExpressionTransient(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [TypeForCollection, TypeForBoundMembers, TypeForTransient, TypeForValue]: implicit conversions from BnfExpressionTransient

	public partial class TypeForCollection
	{
		public static implicit operator BnfExpressionTransient(TypeForCollection term)
		{
			return new BnfExpressionTransient(term.AsBnfTerm());
		}
	}

	public partial class TypeForBoundMembers
	{
		public static implicit operator BnfExpressionTransient(TypeForBoundMembers term)
		{
			return new BnfExpressionTransient(term.AsBnfTerm());
		}
	}

	public partial class TypeForTransient
	{
		public static implicit operator BnfExpressionTransient(TypeForTransient term)
		{
			return new BnfExpressionTransient(term.AsBnfTerm());
		}
	}

	public partial class TypeForValue
	{
		public static implicit operator BnfExpressionTransient(TypeForValue term)
		{
			return new BnfExpressionTransient(term.AsBnfTerm());
		}
	}

	#endregion

	#region TypeForCollection '+' operators for BnfExpression

	public partial class TypeForCollection
	{
        public static BnfExpressionTransient operator +(TypeForCollection term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(KeyTermPunctuation term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(TypeForCollection term1, GrammarHint term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(GrammarHint term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForBoundMembers '+' operators for BnfExpression

	public partial class TypeForBoundMembers
	{
        public static BnfExpressionTransient operator +(TypeForBoundMembers term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(KeyTermPunctuation term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(TypeForBoundMembers term1, GrammarHint term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(GrammarHint term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForTransient '+' operators for BnfExpression

	public partial class TypeForTransient
	{
        public static BnfExpressionTransient operator +(TypeForTransient term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(KeyTermPunctuation term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(TypeForTransient term1, GrammarHint term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(GrammarHint term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForValue '+' operators for BnfExpression

	public partial class TypeForValue
	{
        public static BnfExpressionTransient operator +(TypeForValue term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(KeyTermPunctuation term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(TypeForValue term1, GrammarHint term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(GrammarHint term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForCollection '|' operators for BnfExpression

	public partial class TypeForCollection
	{
        public static BnfExpressionTransient operator |(TypeForCollection term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForCollection term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForBoundMembers term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForCollection term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForTransient term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForCollection term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForValue term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForCollection term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(BnfExpressionTransient term1, TypeForCollection term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForBoundMembers '|' operators for BnfExpression

	public partial class TypeForBoundMembers
	{
        public static BnfExpressionTransient operator |(TypeForBoundMembers term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForBoundMembers term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForTransient term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForBoundMembers term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForValue term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForBoundMembers term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(BnfExpressionTransient term1, TypeForBoundMembers term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForTransient '|' operators for BnfExpression

	public partial class TypeForTransient
	{
        public static BnfExpressionTransient operator |(TypeForTransient term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForTransient term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForValue term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForTransient term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(BnfExpressionTransient term1, TypeForTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForValue '|' operators for BnfExpression

	public partial class TypeForValue
	{
        public static BnfExpressionTransient operator |(TypeForValue term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(TypeForValue term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator |(BnfExpressionTransient term1, TypeForValue term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionTransient '+' operators for BnfExpression

	public partial class BnfExpressionTransient
	{
        public static BnfExpressionTransient operator +(BnfExpressionTransient term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(KeyTermPunctuation term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(BnfExpressionTransient term1, GrammarHint term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient operator +(GrammarHint term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionTransient '|' operators for BnfExpression

	public partial class BnfExpressionTransient
	{
        public static BnfExpressionTransient operator |(BnfExpressionTransient term1, BnfExpressionTransient term2)
        {
            return (BnfExpressionTransient)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfExpressionTransient<T> definition and operators

	public interface IBnfExpressionTransient<out T> : IBnfExpression<T> { }

	#region BnfExpressionTransient<T> definition

	public partial class BnfExpressionTransient<T> : BnfExpressionTransient, IBnfExpressionTransient<T>
	{
		#region Construction

		public BnfExpressionTransient()
		{
		}

		public BnfExpressionTransient(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfExpressionTransient(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfExpressionTransient<T>(BnfExpression bnfExpression)
		{
			return new BnfExpressionTransient<T>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region [TypeForCollection<TCollectionType, TElementType>, TypeForCollection<TCollectionType>, TypeForBoundMembers<TType>, TypeForConstant<T>, TypeForTransient<TType>, TypeForValue<T>]: implicit conversions from BnfExpressionTransient<T>

	public partial class TypeForCollection<TCollectionType, TElementType>
	{
		public static implicit operator BnfExpressionTransient<TCollectionType>(TypeForCollection<TCollectionType, TElementType> term)
		{
			return new BnfExpressionTransient<TCollectionType>(term.AsBnfTerm());
		}
	}

	public partial class TypeForCollection<TCollectionType>
	{
		public static implicit operator BnfExpressionTransient<TCollectionType>(TypeForCollection<TCollectionType> term)
		{
			return new BnfExpressionTransient<TCollectionType>(term.AsBnfTerm());
		}
	}

	public partial class TypeForBoundMembers<TType>
	{
		public static implicit operator BnfExpressionTransient<TType>(TypeForBoundMembers<TType> term)
		{
			return new BnfExpressionTransient<TType>(term.AsBnfTerm());
		}
	}

	public partial class TypeForConstant<T>
	{
		public static implicit operator BnfExpressionTransient<T>(TypeForConstant<T> term)
		{
			return new BnfExpressionTransient<T>(term.AsBnfTerm());
		}
	}

	public partial class TypeForTransient<TType>
	{
		public static implicit operator BnfExpressionTransient<TType>(TypeForTransient<TType> term)
		{
			return new BnfExpressionTransient<TType>(term.AsBnfTerm());
		}
	}

	public partial class TypeForValue<T>
	{
		public static implicit operator BnfExpressionTransient<T>(TypeForValue<T> term)
		{
			return new BnfExpressionTransient<T>(term.AsBnfTerm());
		}
	}

	#endregion

	#region TypeForCollection<TCollectionType, TElementType> '+' operators for BnfExpression

	public partial class TypeForCollection<TCollectionType, TElementType>
	{
        public static BnfExpressionTransient<TCollectionType> operator +(TypeForCollection<TCollectionType, TElementType> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(KeyTermPunctuation term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(TypeForCollection<TCollectionType, TElementType> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(GrammarHint term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForCollection<TCollectionType> '+' operators for BnfExpression

	public partial class TypeForCollection<TCollectionType>
	{
        public static BnfExpressionTransient<TCollectionType> operator +(TypeForCollection<TCollectionType> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(KeyTermPunctuation term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(TypeForCollection<TCollectionType> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator +(GrammarHint term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForBoundMembers<TType> '+' operators for BnfExpression

	public partial class TypeForBoundMembers<TType>
	{
        public static BnfExpressionTransient<TType> operator +(TypeForBoundMembers<TType> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(KeyTermPunctuation term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(TypeForBoundMembers<TType> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(GrammarHint term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForConstant<T> '+' operators for BnfExpression

	public partial class TypeForConstant<T>
	{
        public static BnfExpressionTransient<T> operator +(TypeForConstant<T> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(KeyTermPunctuation term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(TypeForConstant<T> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(GrammarHint term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForTransient<TType> '+' operators for BnfExpression

	public partial class TypeForTransient<TType>
	{
        public static BnfExpressionTransient<TType> operator +(TypeForTransient<TType> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(KeyTermPunctuation term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(TypeForTransient<TType> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator +(GrammarHint term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForValue<T> '+' operators for BnfExpression

	public partial class TypeForValue<T>
	{
        public static BnfExpressionTransient<T> operator +(TypeForValue<T> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(KeyTermPunctuation term1, TypeForValue<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(TypeForValue<T> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(GrammarHint term1, TypeForValue<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForCollection<TCollectionType, TElementType> '|' operators for BnfExpression

	public partial class TypeForCollection<TCollectionType, TElementType>
	{
        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, TypeForBoundMembers<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForBoundMembers<TCollectionType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, TypeForConstant<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForConstant<TCollectionType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, TypeForTransient<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForTransient<TCollectionType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, TypeForValue<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForValue<TCollectionType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType, TElementType> term1, BnfExpressionTransient<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(BnfExpressionTransient<TCollectionType> term1, TypeForCollection<TCollectionType, TElementType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForCollection<TCollectionType> '|' operators for BnfExpression

	public partial class TypeForCollection<TCollectionType>
	{
        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, TypeForBoundMembers<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForBoundMembers<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, TypeForConstant<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForConstant<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, TypeForTransient<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForTransient<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, TypeForValue<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForValue<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(TypeForCollection<TCollectionType> term1, BnfExpressionTransient<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TCollectionType> operator |(BnfExpressionTransient<TCollectionType> term1, TypeForCollection<TCollectionType> term2)
        {
            return (BnfExpressionTransient<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForBoundMembers<TType> '|' operators for BnfExpression

	public partial class TypeForBoundMembers<TType>
	{
        public static BnfExpressionTransient<TType> operator |(TypeForBoundMembers<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForBoundMembers<TType> term1, TypeForConstant<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForConstant<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForBoundMembers<TType> term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForTransient<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForBoundMembers<TType> term1, TypeForValue<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForValue<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForBoundMembers<TType> term1, BnfExpressionTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(BnfExpressionTransient<TType> term1, TypeForBoundMembers<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForConstant<T> '|' operators for BnfExpression

	public partial class TypeForConstant<T>
	{
        public static BnfExpressionTransient<T> operator |(TypeForConstant<T> term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForConstant<T> term1, TypeForTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForTransient<T> term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForConstant<T> term1, TypeForValue<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForValue<T> term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForConstant<T> term1, BnfExpressionTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(BnfExpressionTransient<T> term1, TypeForConstant<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForTransient<TType> '|' operators for BnfExpression

	public partial class TypeForTransient<TType>
	{
        public static BnfExpressionTransient<TType> operator |(TypeForTransient<TType> term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForTransient<TType> term1, TypeForValue<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForValue<TType> term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(TypeForTransient<TType> term1, BnfExpressionTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<TType> operator |(BnfExpressionTransient<TType> term1, TypeForTransient<TType> term2)
        {
            return (BnfExpressionTransient<TType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region TypeForValue<T> '|' operators for BnfExpression

	public partial class TypeForValue<T>
	{
        public static BnfExpressionTransient<T> operator |(TypeForValue<T> term1, TypeForValue<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(TypeForValue<T> term1, BnfExpressionTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator |(BnfExpressionTransient<T> term1, TypeForValue<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionTransient<T> '+' operators for BnfExpression

	public partial class BnfExpressionTransient<T>
	{
        public static BnfExpressionTransient<T> operator +(BnfExpressionTransient<T> term1, KeyTermPunctuation term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(KeyTermPunctuation term1, BnfExpressionTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(BnfExpressionTransient<T> term1, GrammarHint term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

        public static BnfExpressionTransient<T> operator +(GrammarHint term1, BnfExpressionTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Plus((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#region BnfExpressionTransient<T> '|' operators for BnfExpression

	public partial class BnfExpressionTransient<T>
	{
        public static BnfExpressionTransient<T> operator |(BnfExpressionTransient<T> term1, BnfExpressionTransient<T> term2)
        {
            return (BnfExpressionTransient<T>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfExpressionCollection definition and operators

	public interface IBnfExpressionCollection : IBnfExpression { }

	#region BnfExpressionCollection definition

	public partial class BnfExpressionCollection : BnfExpressionCommon, IBnfExpressionCollection
	{
		#region Construction

		public BnfExpressionCollection()
		{
		}

		public BnfExpressionCollection(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfExpressionCollection(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfExpressionCollection(BnfExpression bnfExpression)
		{
			return new BnfExpressionCollection(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region BnfExpressionCollection '|' operators for BnfExpression

	public partial class BnfExpressionCollection
	{
        public static BnfExpressionCollection operator |(BnfExpressionCollection term1, BnfExpressionCollection term2)
        {
            return (BnfExpressionCollection)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

	#region BnfExpressionCollection<TCollectionType> definition and operators

	public interface IBnfExpressionCollection<out TCollectionType> : IBnfExpression<TCollectionType> { }

	#region BnfExpressionCollection<TCollectionType> definition

	public partial class BnfExpressionCollection<TCollectionType> : BnfExpressionCollection, IBnfExpressionCollection<TCollectionType>
	{
		#region Construction

		public BnfExpressionCollection()
		{
		}

		public BnfExpressionCollection(BnfExpression bnfExpression)
			: base(bnfExpression)
		{
		}

        public BnfExpressionCollection(BnfTerm bnfTerm)
			: base(bnfTerm)
        {
        }

		#endregion

		#region Cast operators

		public static explicit operator BnfExpressionCollection<TCollectionType>(BnfExpression bnfExpression)
		{
			return new BnfExpressionCollection<TCollectionType>(bnfExpression);
		}

		#endregion
	}

	#endregion

	#region BnfExpressionCollection<TCollectionType> '|' operators for BnfExpression

	public partial class BnfExpressionCollection<TCollectionType>
	{
        public static BnfExpressionCollection<TCollectionType> operator |(BnfExpressionCollection<TCollectionType> term1, BnfExpressionCollection<TCollectionType> term2)
        {
            return (BnfExpressionCollection<TCollectionType>)BnfTerm.Op_Pipe((BnfTerm)term1, (BnfTerm)term2);
        }

	}

	#endregion

	#endregion

}
