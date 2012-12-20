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
    public partial class BnfiTermMember : NonTerminal, IBnfiTerm
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected BnfiTermMember(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            base.Rule = new BnfExpression(bnfTerm);
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;      // the parent TypeForBoundMembers will take care of the child ast node
        }

        public static BnfiTermMember Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMember(propertyInfo, bnfTerm);
        }

        public static BnfiTermMember Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMember(fieldInfo, bnfTerm);
        }

        public static BnfiTermMember Bind<TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMember(memberInfo, bnfTerm.AsBnfTerm());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMember<TDeclaringType>(memberInfo, bnfTerm.AsBnfTerm());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(IBnfiTerm<TDeclaringType> dummyBnfTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TBnfTermType> bnfTerm)
            where TBnfTermType : TMemberType
        {
            return Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            return Bind(typeof(TDeclaringType), fieldOrPropertyName, bnfTerm);
        }

        public static BnfiTermMember Bind(Type declaringType, string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new BnfiTermMember(memberInfo, bnfTerm);
        }

        public static BnfiTermMember BindToNone(BnfTerm bnfTerm)
        {
            return new BnfiTermMember(memberInfo: null, bnfTerm: bnfTerm);
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

    public partial class BnfiTermMember<TDeclaringType> : BnfiTermMember, IBnfiTerm<TDeclaringType>
    {
        internal BnfiTermMember(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }

    }
}
