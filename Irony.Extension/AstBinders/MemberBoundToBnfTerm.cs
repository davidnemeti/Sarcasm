﻿using System;
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

        protected new static BnfExpression<TDeclaringType> Op_Plus(BnfTerm bnfTerm1, BnfTerm bnfTerm2)
        {
            //Check term1 and see if we can use it as result, simply adding term2 as operand
            BnfExpression expr1 = bnfTerm1 as BnfExpression;
            if (expr1 == null || expr1.Data.Count > 1) //either not expression at all, or Pipe-type expression (count > 1)
                expr1 = new BnfExpression(bnfTerm1);
            expr1.Data[expr1.Data.Count - 1].Add(bnfTerm2);
            return new BnfExpression<TDeclaringType>(expr1);
        }
    }
}