using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public abstract partial class BnfiTermType : BnfiTermNonTerminal, IBnfiTerm, IBnfiTermCopyable, IUnparsableNonTerminal
    {
        private struct ParseIndexedBnfTerm
        {
            public readonly BnfTerm BnfTerm;
            public readonly int ParseIndex;

            public ParseIndexedBnfTerm(BnfTerm bnfTerm, int parseIndex)
            {
                this.BnfTerm = bnfTerm;
                this.ParseIndex = parseIndex;
            }
        }

        private IDictionary<ParseIndexedBnfTerm, Member> parseIndexedBnfTermToMember = new Dictionary<ParseIndexedBnfTerm, Member>();
        private IDictionary<int, int> ruleIndexToParseIndex = new Dictionary<int, int>();

        protected BnfiTermType(Type type, string name)
            : base(type, name, isReferable: true)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                object obj = Activator.CreateInstance(type, nonPublic: true);

                var parseIndexedChildValues = parseTreeNode.ChildNodes
                    .Select(
                        (childNode, index) => new
                            {
                                ChildBnfTerm = childNode.Term,
                                ChildValue = GrammarHelper.AstNodeToValue(childNode.AstNode),
                                ChildParseIndex = index
                            }
                        )
                    .Where(indexedChildValue => indexedChildValue.ChildValue != null)
                    .ToList();

                // 1. memberwise copy for BnfiTermCopy items
                foreach (var parseIndexedChildValue in parseIndexedChildValues.Where(indexedChildValue => obj.GetType().IsAssignableFrom(indexedChildValue.ChildValue.GetType())))
                    MemberwiseCopyExceptNullValues(obj, parseIndexedChildValue.ChildValue);

                // 2. set member values for member items (it's the second step, so that we can overwrite the copied members if we want)
                foreach (var parseIndexedChildValue in parseIndexedChildValues.Where(indexedChildValue => IsMemberByParseIndex(indexedChildValue.ChildBnfTerm, indexedChildValue.ChildParseIndex)))
                    SetValue(GetMemberByParseIndex(parseIndexedChildValue.ChildBnfTerm, parseIndexedChildValue.ChildParseIndex).MemberInfo, obj, parseIndexedChildValue.ChildValue);

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(obj, context, parseTreeNode);
            };
        }

        protected static void MemberwiseCopyExceptNullValues(object destination, object source)
        {
            MemberwiseCopy(destination, source, fieldValue => fieldValue != null);
        }

        protected static void MemberwiseCopy(object destination, object source)
        {
            MemberwiseCopy(destination, source, fieldValue => true);
        }

        protected static void MemberwiseCopy(object destination, object source, Predicate<object> cloneFieldValue)
        {
            Type destinationType = destination.GetType();
            Type sourceType = source.GetType();

            if (!destinationType.IsAssignableFrom(sourceType))
                throw new ArgumentException(string.Format("{0} is not assignable from {1}", destinationType.Name, sourceType.Name), "source");

            foreach (FieldInfo fieldInfo in GetAllFields(sourceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object fieldValue = fieldInfo.GetValue(source);
                if (cloneFieldValue(fieldValue))
                    fieldInfo.SetValue(destination, fieldValue);
            }
        }

        protected static IEnumerable<FieldInfo> GetAllFields(Type type, BindingFlags bindingFlags)
        {
            return type.GetFields(bindingFlags)
                .Concat(type.BaseType != null
                    ? GetAllFields(type.BaseType, bindingFlags)
                    : new FieldInfo[0]);
        }

        protected static void SetValue(MemberInfo memberInfo, object obj, object value)
        {
            if (memberInfo is PropertyInfo)
                ((PropertyInfo)memberInfo).SetValue(obj, value);
            else if (memberInfo is FieldInfo)
                ((FieldInfo)memberInfo).SetValue(obj, value);
            else
                throw new ApplicationException("Object with wrong type in memberinfo: " + memberInfo.Name);
        }

        protected static object GetValue(MemberInfo memberInfo, object obj)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).GetValue(obj);
            else if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).GetValue(obj);
            else
                throw new ApplicationException("Object with wrong type in memberinfo: " + memberInfo.Name);
        }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }

        protected new BnfiExpression Rule
        {
            set
            {
                ConvertAndStoreMembers(value);
                base.Rule = value;
            }
        }

        private void ConvertAndStoreMembers(BnfiExpression bnfiExpression)
        {
            foreach (BnfTermList bnfterms in bnfiExpression.GetBnfTermsList())
            {
                int parseIndex = 0;

                for (int ruleIndex = 0; ruleIndex < bnfterms.Count; ruleIndex++)
                {
                    if (bnfterms[ruleIndex] is Member)
                    {
                        Member member = (Member)bnfterms[ruleIndex];

                        RegisterMember(member.BnfTerm, parseIndex, ruleIndex, member);
                        bnfterms[ruleIndex] = member.BnfTerm;
                    }

                    if (!bnfterms[ruleIndex].IsPunctuation())
                        parseIndex++;
                }
            }
        }

        private void RegisterMember(ParseIndexedBnfTerm parseIndexedBnfTerm, int ruleIndex, Member member)
        {
        }

        private void RegisterMember(BnfTerm bnfTerm, int parseIndex, int ruleIndex, Member member)
        {
            this.parseIndexedBnfTermToMember.Add(new ParseIndexedBnfTerm(bnfTerm, parseIndex), member);
            this.ruleIndexToParseIndex.Add(ruleIndex, parseIndex);
        }

        private bool IsMemberByParseIndex(BnfTerm bnfTerm, int parseIndex)
        {
            return this.parseIndexedBnfTermToMember.ContainsKey(new ParseIndexedBnfTerm(bnfTerm, parseIndex));
        }

        private bool IsMemberByRuleIndex(BnfTerm bnfTerm, int ruleIndex)
        {
            int parseIndex;
            return this.ruleIndexToParseIndex.TryGetValue(ruleIndex, out parseIndex) && IsMemberByParseIndex(bnfTerm, parseIndex);
        }

        private Member GetMemberByParseIndex(BnfTerm bnfTerm, int parseIndex)
        {
            return this.parseIndexedBnfTermToMember[new ParseIndexedBnfTerm(bnfTerm, parseIndex)];
        }

        private Member GetMemberByRuleIndex(BnfTerm bnfTerm, int ruleIndex)
        {
            int parseIndex = this.ruleIndexToParseIndex[ruleIndex];
            return GetMemberByParseIndex(bnfTerm, parseIndex);
        }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsableNonTerminal.GetChildren(BnfTermList childBnfTerms, object obj)
        {
            foreach (var childRuleIndexedBnfTerm in childBnfTerms.Select((childBnfTerm, ruleIndex) => new { BnfTerm = childBnfTerm, RuleIndex = ruleIndex }))
            {
                object childObj;

                if (IsMemberByRuleIndex(childRuleIndexedBnfTerm.BnfTerm, childRuleIndexedBnfTerm.RuleIndex))
                    childObj = GetValue(GetMemberByRuleIndex(childRuleIndexedBnfTerm.BnfTerm, childRuleIndexedBnfTerm.RuleIndex).MemberInfo, obj);
                else if (childRuleIndexedBnfTerm.BnfTerm is BnfiTermCopy)
                    childObj = obj;
                else
                    childObj = obj;

                yield return new UnparsableObject(childRuleIndexedBnfTerm.BnfTerm, childObj);
            }
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children)
        {
            return children
                .SumIncludingNullValues(
                    (child, ruleIndex) => IsMemberByRuleIndex(child.BnfTerm, ruleIndex)
                        ? GetBnfTermPriorityForMember(unparser, child)
                        : unparser.GetPriority(child)
                    );
        }

        private static int? GetBnfTermPriorityForMember(IUnparser unparser, UnparsableObject unparsableObject)
        {
            if (unparsableObject.Obj != null)
                return 1;
            else if (unparsableObject.BnfTerm is BnfiTermCollection)
                return unparser.GetPriority(unparsableObject);
            else
                return null;
        }

        #endregion
    }

    public partial class BnfiTermTypeTL : BnfiTermType, IBnfiTermTL, IBnfiTermCopyableTL
    {
        public BnfiTermTypeTL(Type type, string name = null)
            : base(type, name)
        {
        }

        public new BnfiExpressionTypeTL Rule { set { base.Rule = value; } }
    }

    public partial class BnfiTermType<TType> : BnfiTermType, IBnfiTerm<TType>, IBnfiTermCopyable<TType>, IBnfiTermOrAbleForChoice<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static BnfiTermType()
        {
            __ = new TType();
        }

        public TType _ { get { return BnfiTermType<TType>.__; } }

        public BnfiTermType(string name = null)
            : base(typeof(TType), name)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionTypeTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionType<TType> Rule { set { base.Rule = value; } }

        // NOTE: type inference for superclasses works only if SetRulePlus is an instance method and not an extension method
        public void SetRulePlus(IBnfiTermPlusAbleForType<TType> bnfiTermFirst, IBnfiTermPlusAbleForType<TType> bnfiTermSecond, params IBnfiTermPlusAbleForType<TType>[] bnfiTerms)
        {
            this.Rule = Plus(bnfiTermFirst, bnfiTermSecond, bnfiTerms);
        }

        public BnfiExpressionType<TType> Plus(IBnfiTermPlusAbleForType<TType> bnfiTermFirst, IBnfiTermPlusAbleForType<TType> bnfiTermSecond, params IBnfiTermPlusAbleForType<TType>[] bnfiTerms)
        {
            return (BnfiExpressionType<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                bnfiTermFirst.AsBnfTerm() + bnfiTermSecond.AsBnfTerm(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed + bnfTermToBeProcess
                );
        }
    }
}
