﻿using System;
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
    public abstract partial class BnfiTermMember : NonTerminal, IBnfiTerm, IUnparsable
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected BnfiTermMember(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            base.Rule = bnfTerm.ToBnfExpression() + GrammarHelper.ReduceHere();

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    try
                    {
                        parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(
                            new MemberValue(memberInfo, GrammarHelper.AstNodeToValue(parseTreeNode.ChildNodes.Single().AstNode)),
                            context,
                            parseTreeNode
                            );
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new ArgumentException(string.Format("Only one child is allowed for a BnfiTermMember node: {0}", parseTreeNode.Term.Name), "parseTreeNode", e);
                    }
                };
        }

        protected static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMember<TDeclaringType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        protected static BnfiTermMemberTL Bind<TMemberType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMemberTL(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        // NOTE: the method's name is Bind_ instead of Bind to avoid ambiguous calls
        public static BnfiTermMember<TDeclaringType> Bind_<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, BnfTerm bnfTerm)
        {
            return Bind(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, IBnfiTermTL bnfiTerm)
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMemberTL Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMemberTL(propertyInfo, bnfTerm);
        }

        public static BnfiTermMemberTL Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMemberTL(fieldInfo, bnfTerm);
        }

        public static BnfiTermMemberTL Bind<TMemberType, TValueType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMemberTL Bind<TMemberElementType, TValueElementType>(Expression<Func<ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess,
            IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMemberTL Bind<TMemberElementType, TValueElementType>(Expression<Func<IList<TMemberElementType>>> exprForFieldOrPropertyAccess,
            IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TValueType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TValueType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TValueType> bnfiTerm)
            where TValueType : TMemberType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm);
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, ICollection<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberElementType, TValueElementType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, IList<TMemberElementType>>> exprForFieldOrPropertyAccess, IBnfiTerm<IEnumerable<TValueElementType>> bnfiTerm)
            where TValueElementType : TMemberElementType
        {
            return Bind(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMemberTL Bind<TDeclaringType>(string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            return Bind(typeof(TDeclaringType), fieldOrPropertyName, bnfTerm);
        }

        public static BnfiTermMemberTL Bind(Type declaringType, string fieldOrPropertyName, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = (MemberInfo)declaringType.GetField(fieldOrPropertyName) ?? (MemberInfo)declaringType.GetProperty(fieldOrPropertyName);

            if (memberInfo == null)
                throw new ArgumentException("Field or property not found", fieldOrPropertyName);

            return new BnfiTermMemberTL(memberInfo, bnfTerm);
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        NonTerminal INonTerminal.AsNonTerminal()
        {
            return this;
        }

        [Obsolete("Cannot use Rule method", error: true)]
        public new BnfExpression Rule
        {
            get { return base.Rule; }
            set { base.Rule = value; }
        }

        #region Unparse

        bool IUnparsable.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsable.GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj)
        {
            yield return new UnparsableObject(this.BnfTerm, obj);
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects)
        {
            if (obj != null)
                return 1;
            else if (this.BnfTerm is BnfiTermCollection)
                return unparser.GetBnfTermPriority(this.BnfTerm, obj);
            else
                return null;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[{0} (declaring type: {1}, member: {2}, value: {3})]", this.Name, this.MemberInfo.DeclaringType, this.MemberInfo.Name, this.BnfTerm.Name);
        }
    }

    public partial class BnfiTermMemberTL : BnfiTermMember, IBnfiTermTL
    {
        internal BnfiTermMemberTL(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<T>, instead it implements IBnfiTermPlusAbleForType<T>
    public partial class BnfiTermMember<TDeclaringType> : BnfiTermMember, IBnfiTerm<TDeclaringType>, IBnfiTermPlusAbleForType<TDeclaringType>
    {
        internal BnfiTermMember(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(memberInfo, bnfTerm)
        {
        }
    }

    internal class MemberValue
    {
        public MemberInfo MemberInfo { get; private set; }
        public object Value { get; private set; }

        public MemberValue(MemberInfo MemberInfo, object Value)
        {
            this.MemberInfo = MemberInfo;
            this.Value = Value;
        }

        public override string ToString()
        {
            return string.Format(".{{{0}}} : {1}", MemberInfo.Name, ToStringValue(Value));
        }

        protected static string ToStringValue(object value)
        {
            return value is System.Collections.IEnumerable
                ? GrammarHelper.TypeNameWithDeclaringTypes(value.GetType())
                : value.ToString();
        }
    }
}
