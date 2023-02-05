#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

using Irony.Parsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
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
            MemberInfo memberInfo = Util.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new Member<TDeclaringType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        protected static MemberTL Bind<TMemberType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = Util.GetMember(exprForFieldOrPropertyAccess);

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

        public Type DomainType
        {
            get { return MemberInfo is PropertyInfo ? ((PropertyInfo)MemberInfo).PropertyType : ((FieldInfo)MemberInfo).FieldType; }
        }
    }

    public partial class MemberTL : Member, IBnfiTermTL
    {
        internal MemberTL(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<TD>, instead it implements IBnfiTermPlusAbleForType<TD>
    public partial class Member<TDDeclaringType> : Member, IBnfiTerm<TDDeclaringType>, IBnfiTermPlusAbleForType<TDDeclaringType>
    {
        internal Member(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }
}
