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
    public partial class BnfiTermMember : NonTerminal, IBnfiTermTL, IUnparsable
    {
        public MemberInfo MemberInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        protected BnfiTermMember(MemberInfo memberInfo, BnfTerm bnfTerm)
            : base(name: string.Format("{0}.{1}", GrammarHelper.TypeNameWithDeclaringTypes(memberInfo.DeclaringType), memberInfo.Name.ToLower()))
        {
            this.MemberInfo = memberInfo;
            this.BnfTerm = bnfTerm;
            base.Rule = new BnfExpression(bnfTerm);

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
                {
                    if (parseTreeNode.ChildNodes.Count != 1)
                        throw new ArgumentException("Only one child is allowed for a BnfiTermMember node: {0}", parseTreeNode.Term.Name);

                    parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(new MemberValue(memberInfo, GrammarHelper.AstNodeToValue(parseTreeNode.ChildNodes[0].AstNode)), context, parseTreeNode);
                };
        }

        protected static BnfiTermMember<TDeclaringType> Bind_<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, BnfTerm bnfTerm)
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMember<TDeclaringType>(memberInfo, bnfTerm);
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        // NOTE: first parameter is a IBnfiTerm instead of BnfTerm to avoid ambiguous calls
        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, IBnfiTerm bnfiTerm)
        {
            return Bind_(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        // NOTE: the method's name is Bind_ instead of Bind to avoid ambiguous calls
        public static BnfiTermMember<TDeclaringType> Bind_<TDeclaringType, TMemberType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TDeclaringType> dummyBnfiTerm, BnfTerm bnfTerm)
        {
            return Bind_(exprForFieldOrPropertyAccess, bnfTerm);
        }

        public static BnfiTermMember Bind(PropertyInfo propertyInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMember(propertyInfo, bnfTerm);
        }

        public static BnfiTermMember Bind(FieldInfo fieldInfo, BnfTerm bnfTerm)
        {
            return new BnfiTermMember(fieldInfo, bnfTerm);
        }

        public static BnfiTermMember Bind<TMemberType, TBnfTermType>(Expression<Func<TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TBnfTermType> bnfiTerm)
            where TBnfTermType : TMemberType
        {
            MemberInfo memberInfo = GrammarHelper.GetMember(exprForFieldOrPropertyAccess);

            if (memberInfo is FieldInfo || memberInfo is PropertyInfo)
                return new BnfiTermMember(memberInfo, bnfiTerm.AsBnfTerm());
            else
                throw new ArgumentException("Field or property not found", memberInfo.Name);
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess,
            IBnfiTerm<TBnfTermType> bnfiTerm)
            where TBnfTermType : TMemberType
        {
            return Bind_(exprForFieldOrPropertyAccess, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermMember<TDeclaringType> Bind<TDeclaringType, TMemberType, TBnfTermType>(IBnfiTerm<TDeclaringType> dummyBnfiTerm,
            Expression<Func<TDeclaringType, TMemberType>> exprForFieldOrPropertyAccess, IBnfiTerm<TBnfTermType> bnfiTerm)
            where TBnfTermType : TMemberType
        {
            return Bind<TDeclaringType, TMemberType, TBnfTermType>(exprForFieldOrPropertyAccess, bnfiTerm);
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

        BnfTerm IBnfiTerm.AsBnfTerm()
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

    public partial class BnfiTermMember<TDeclaringType> : BnfiTermMember, IBnfiTerm<TDeclaringType>
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
