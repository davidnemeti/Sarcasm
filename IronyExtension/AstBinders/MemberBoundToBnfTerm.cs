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

namespace Irony.AstBinders
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

        public static MemberBoundToBnfTerm<TMemberType, TBnfTermType> Bind<TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm<TMemberType, TBnfTermType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
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

    public class MemberBoundToBnfTerm<TMemberType, TBnfTermType> : MemberBoundToBnfTerm, IBnfTerm<TMemberType>
    {
        internal MemberBoundToBnfTerm(MemberInfo memberInfo, IBnfTerm<TBnfTermType> bnfTerm)
            : base(memberInfo, bnfTerm.AsTypeless())
        {
        }

        BnfTerm IBnfTerm<TMemberType>.AsTypeless()
        {
            return this;
        }
    }
}
