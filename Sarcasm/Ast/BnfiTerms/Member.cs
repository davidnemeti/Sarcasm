using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public abstract partial class Member : BnfTerm, IBnfiTerm
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected Member(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
        }

        protected static Member<TDeclaringType> Bind<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new Member<TDeclaringType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        protected static MemberTL Bind<TMemberType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new MemberTL(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        // NOTE: the method's name is Bind_ instead of Bind to avoid ambiguous calls
        public static Member<TDeclaringType> Bind_<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, BnfTerm bnfTerm)
        {
            return Bind(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static MemberTL Bind<TMemberType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfiTermTL bnfiTerm)
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, IBnfiTermTL bnfiTerm)
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static MemberTL Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new MemberTL(propertyInfo, bnfTerm);
        }

        public static MemberTL Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new MemberTL(fieldInfo, bnfTerm);
        }

        public static MemberTL Bind<TMemberType, TValueType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static MemberTL Bind<TMemberElementType, TValueElementType>(Expression<Func<ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess,
            IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static MemberTL Bind<TMemberElementType, TValueElementType>(Expression<Func<IList<TMemberElementType>>> exprForFieldOrPropertyAccess,
            IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberType, TValueType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberType, TValueType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static Member<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static MemberTL Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            return Bind(typeof(TDeclaringType), fieldOrPropertyName, bnfTerm);
        }

        public static MemberTL Bind(Type declaringType, string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new MemberTL(memberInfo, bnfTerm);
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public override string ToString()
        {
            return string.Format("[{0} (declaring type: {1}, member: {2}, value: {3})]", this.Name, this.MemberInfo.DeclaringType, this.MemberInfo.Name, this.BnfTerm.Name);
        }
    }

    public partial class MemberTL : Member, IBnfiTermTL
    {
        internal MemberTL(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<T>, instead it implements IBnfiTermPlusAbleForType<T>
    public partial class Member<TDeclaringType> : Member, IBnfiTerm<TDeclaringType>, IBnfiTermPlusAbleForType<TDeclaringType>
    {
        internal Member(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }
}
