using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using System.IO;

namespace Irony.Extension.AstBinders
{
    public class MemberBoundToBnfTerm : NonTerminal
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
            this.Rule = new BnfExpression(bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new MemberBoundToBnfTerm(propertyInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new MemberBoundToBnfTerm(fieldInfo, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind<TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm(memberInfo, bnfTerm.AsTypeless());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm<TDeclaringType>(memberInfo, bnfTerm.AsTypeless());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(IBnfTerm<TDeclaringType> dummyBnfTerm, Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            return Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            return Bind(typeof(TDeclaringType), fieldOrPropertyName, bnfTerm);
        }

        public static MemberBoundToBnfTerm Bind(Type declaringType, string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new MemberBoundToBnfTerm(memberInfo, bnfTerm);
        }

        public static BnfExpressionWithMemberBoundToBnfTerm operator +(MemberBoundToBnfTerm bnfTerm1, MemberBoundToBnfTerm bnfTerm2)
        {
            return new BnfExpressionWithMemberBoundToBnfTerm(Op_Plus(bnfTerm1, bnfTerm2));
        }

        public static BnfExpressionWithMemberBoundToBnfTerm operator +(MemberBoundToBnfTerm bnfTerm1, BnfExpression bnfTerm2)
        {
            return new BnfExpressionWithMemberBoundToBnfTerm(Op_Plus(bnfTerm1, bnfTerm2));
        }

        public static BnfExpressionWithMemberBoundToBnfTerm operator +(BnfExpression bnfTerm1, MemberBoundToBnfTerm bnfTerm2)
        {
            return new BnfExpressionWithMemberBoundToBnfTerm(Op_Plus(bnfTerm1, bnfTerm2));
        }
    }

    public class MemberBoundToBnfTerm<TDeclaringType> : MemberBoundToBnfTerm, IBnfTerm<TDeclaringType>
    {
        internal MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }

        BnfTerm IBnfTerm<TDeclaringType>.AsTypeless()
        {
            return this;
        }

        [Obsolete(TypeForNonTerminal.typelessMemberBoundErrorMessage, error: true)]
        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> bnfTerm1, BnfExpressionWithMemberBoundToBnfTerm bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        [Obsolete(TypeForNonTerminal.typelessMemberBoundErrorMessage, error: true)]
        public static BnfExpression<TDeclaringType> operator +(BnfExpressionWithMemberBoundToBnfTerm bnfTerm1, MemberBoundToBnfTerm<TDeclaringType> bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        [Obsolete(TypeForNonTerminal.typelessMemberBoundErrorMessage, error: true)]
        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> bnfTerm1, MemberBoundToBnfTerm bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        [Obsolete(TypeForNonTerminal.typelessMemberBoundErrorMessage, error: true)]
        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm bnfTerm1, MemberBoundToBnfTerm<TDeclaringType> bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> bnfTerm1, BnfExpression bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator +(BnfExpression bnfTerm1, MemberBoundToBnfTerm<TDeclaringType> bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> bnfTerm1, MemberBoundToBnfTerm<TDeclaringType> bnfTerm2)
        {
            return Op_Plus(bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator +(MemberBoundToBnfTerm<TDeclaringType> bnfTerm1, BnfExpression<TDeclaringType> bnfTerm2)
        {
            return Op_Plus(bnfTerm1, (BnfExpression)bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator +(BnfExpression<TDeclaringType> bnfTerm1, MemberBoundToBnfTerm<TDeclaringType> bnfTerm2)
        {
            return Op_Plus((BnfExpression)bnfTerm1, bnfTerm2);
        }

        public static BnfExpression<TDeclaringType> operator |(MemberBoundToBnfTerm<TDeclaringType> term1, MemberBoundToBnfTerm<TDeclaringType> term2)
        {
            return GrammarHelper.Op_Pipe<TDeclaringType>(term1, term2);
        }

        protected new static BnfExpression<TDeclaringType> Op_Plus(BnfTerm bnfTerm1, BnfTerm bnfTerm2)
        {
            return GrammarHelper.Op_Plus<TDeclaringType>(bnfTerm1, bnfTerm2);
        }
    }
}
