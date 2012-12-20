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

namespace Irony.ITG
{
    public partial class MemberBoundToBnfTerm : NonTerminal, IBnfTerm
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            base.Rule = new BnfExpression(bnfTerm);
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;      // the parent TypeForBoundMembers will take care of the child ast node
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
                return new MemberBoundToBnfTerm(memberInfo, bnfTerm.AsBnfTerm());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberBoundToBnfTerm<TDeclaringType>(memberInfo, bnfTerm.AsBnfTerm());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static MemberBoundToBnfTerm<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(IBnfTerm<TDeclaringType> dummyBnfTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, IBnfTerm<TBnfTermType> bnfTerm)
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

        public static MemberBoundToBnfTerm BindToNone(BnfTerm bnfTerm)
        {
            return new MemberBoundToBnfTerm(memberInfo: null, bnfTerm: bnfTerm);
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }

        [Obsolete("Cannot use Rule method", error: true)]
        public new BnfExpression Rule
        {
            get { return base.Rule; }
            set { base.Rule = value; }
        }
    }

    public partial class MemberBoundToBnfTerm<TDeclaringType> : MemberBoundToBnfTerm, IBnfTerm<TDeclaringType>
    {
        internal MemberBoundToBnfTerm(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }

    }
}
