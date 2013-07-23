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
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermRecord : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region Types

        private struct ReferredBnfTerm : IEquatable<ReferredBnfTerm>
        {
            public readonly IList<BnfTerm> EncloserBnfTerms;
            public readonly BnfTerm BnfTerm;
            public readonly int BnfTermIndex;

            public ReferredBnfTerm(IList<BnfTerm> encloserBnfTerms, BnfTerm bnfTerm, int bnfTermIndex)
            {
                this.BnfTerm = bnfTerm;
                this.EncloserBnfTerms = encloserBnfTerms;
                this.BnfTermIndex = bnfTermIndex;
            }

            public override bool Equals(object obj)
            {
                return obj is ReferredBnfTerm && Equals((ReferredBnfTerm)obj);
            }

            public bool Equals(ReferredBnfTerm that)
            {
                return object.ReferenceEquals(this, that)
                    ||
                    !object.ReferenceEquals(that, null) &&
                    this.BnfTerm == that.BnfTerm &&
                    this.BnfTermIndex == that.BnfTermIndex &&
                    this.EncloserBnfTerms.Count == that.EncloserBnfTerms.Count &&
                    this.EncloserBnfTerms.SequenceEqual(that.EncloserBnfTerms);
            }

            public override int GetHashCode()
            {
                return Util.GetHashCodeMulti(BnfTerm, BnfTermIndex, Util.GetHashCodeMulti(EncloserBnfTerms));
            }
        }

        #endregion

        #region State

        private IDictionary<ReferredBnfTerm, Member> parseReferredBnfTermToMember = new Dictionary<ReferredBnfTerm, Member>();
        private IDictionary<ReferredBnfTerm, Member> ruleReferredBnfTermToMember = new Dictionary<ReferredBnfTerm, Member>();

        #endregion

        protected BnfiTermRecord(Type type, string name)
            : base(type, name)
        {
            if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                object astValue = Activator.CreateInstance(type, nonPublic: true);

                var parseChildBnfTerms = parseTreeNode.ChildNodes.Select(childParseTreeNode => childParseTreeNode.Term).ToList();

                var parseChildValues = parseTreeNode.ChildNodes
                    .Select(
                        (parseChildNode, parseChildNodeIndex) => new
                            {
                                ReferredBnfTerm = new ReferredBnfTerm(parseChildBnfTerms, parseChildNode.Term, parseChildNodeIndex),
                                Value = GrammarHelper.AstNodeToValue(parseChildNode.AstNode)
                            }
                        )
                    .Where(parseChildValue => parseChildValue.Value != null)
                    .ToList();

                // 1. memberwise copy for BnfiTermCopy items
                foreach (var parseChildValue in parseChildValues)
                    if (astValue.GetType().IsAssignableFrom(parseChildValue.Value.GetType()))
                        MemberwiseCopyExceptNullValues(astValue, parseChildValue.Value);

                // 2. set member values for member items (it's the second step, so that we can overwrite the copied members if we want)
                foreach (var parseChildValue in parseChildValues)
                    if (IsMemberAtParse(parseChildValue.ReferredBnfTerm))
                        SetValue(GetMemberAtParse(parseChildValue.ReferredBnfTerm).MemberInfo, astValue, parseChildValue.Value);

                parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValue, context, parseTreeNode);
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
            foreach (BnfTermList bnfTerms in bnfiExpression.GetBnfTermsList())
            {
                int parseIndex = 0;

                for (int ruleIndex = 0; ruleIndex < bnfTerms.Count; ruleIndex++)
                {
                    if (bnfTerms[ruleIndex] is Member)
                    {
                        Member member = (Member)bnfTerms[ruleIndex];

                        RegisterMember(member.BnfTerm, bnfTerms, parseIndex, ruleIndex, member);
                        bnfTerms[ruleIndex] = member.BnfTerm;
                    }

                    if (!bnfTerms[ruleIndex].IsPunctuation())
                        parseIndex++;
                }
            }
        }

        private void RegisterMember(BnfTerm bnfTerm, IList<BnfTerm> bnfTerms, int parseIndex, int ruleIndex, Member member)
        {
            var ruleBnfTerms = bnfTerms.Where(_bnfTerm => !(_bnfTerm is GrammarHint)).Select(_bnfTerm => _bnfTerm is Member ? ((Member)_bnfTerm).BnfTerm : _bnfTerm).ToList();
            var parseBnfTerms = ruleBnfTerms.Where(ruleBnfTerm => !ruleBnfTerm.IsPunctuation()).ToList();

            this.ruleReferredBnfTermToMember.Add(new ReferredBnfTerm(ruleBnfTerms, bnfTerm, ruleIndex), member);
            this.parseReferredBnfTermToMember.Add(new ReferredBnfTerm(parseBnfTerms, bnfTerm, parseIndex), member);
        }

        private bool IsMemberAtParse(ReferredBnfTerm referredBnfTerm)
        {
            return this.parseReferredBnfTermToMember.ContainsKey(referredBnfTerm);
        }

        private bool IsMemberAtRule(ReferredBnfTerm referredBnfTerm)
        {
            return this.ruleReferredBnfTermToMember.ContainsKey(referredBnfTerm);
        }

        private Member GetMemberAtParse(ReferredBnfTerm referredBnfTerm)
        {
            return this.parseReferredBnfTermToMember[referredBnfTerm];
        }

        private Member GetMemberByAtRule(ReferredBnfTerm referredBnfTerm)
        {
            return this.ruleReferredBnfTermToMember[referredBnfTerm];
        }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object astValue, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(IList<BnfTerm> childBnfTerms, object astValue, Unparser.Direction direction)
        {
            var childRuleBnfTerms = direction == Unparser.Direction.LeftToRight
                ? childBnfTerms
                : childBnfTerms.ReverseOptimized();

            foreach (var childRuleReferredBnfTerm in childBnfTerms.Select((childBnfTerm, index) =>
                new ReferredBnfTerm(childRuleBnfTerms, childBnfTerm, direction == Unparser.Direction.LeftToRight ? index : childBnfTerms.Count - 1 - index)
                ))
            {
                object childAstValue;
                Member member;

                if (IsMemberAtRule(childRuleReferredBnfTerm))
                {
                    member = GetMemberByAtRule(childRuleReferredBnfTerm);
                    childAstValue = GetValue(member.MemberInfo, astValue);
                }
                else if (childRuleReferredBnfTerm.BnfTerm is BnfiTermCopy)
                {
                    member = null;
                    childAstValue = astValue;
                }
                else
                {
                    member = null;
                    childAstValue = astValue;
                }

                yield return new UnparsableAst(childRuleReferredBnfTerm.BnfTerm, childAstValue, member);
            }
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, IEnumerable<UnparsableAst> ruleChildren, Unparser.Direction direction)
        {
            var ruleChildBnfTerms = direction == Unparser.Direction.LeftToRight
                ? ruleChildren.Select(ruleChild => ruleChild.BnfTerm).ToList()
                : ruleChildren.Select(ruleChild => ruleChild.BnfTerm).Reverse().ToList();

            return ruleChildren
                .SumIncludingNullValues(
                    (ruleChild, ruleChildIndex) => IsMemberAtRule(new ReferredBnfTerm(ruleChildBnfTerms, ruleChild.BnfTerm, ruleChildIndex))
                        ? GetBnfTermPriorityForMember(unparser, ruleChild)
                        : unparser.GetPriority(ruleChild)
                    );
        }

        private static int? GetBnfTermPriorityForMember(IUnparser unparser, UnparsableAst unparsableAst)
        {
            if (unparsableAst.AstValue != null)
                return 1;
            else if (unparsableAst.BnfTerm is BnfiTermCollection && ((BnfiTermCollection)unparsableAst.BnfTerm).EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull)
                return 0;
            else
                return null;
        }

        #endregion
    }

    public partial class BnfiTermRecordTL : BnfiTermRecord, IBnfiTermTL, IBnfiTermCopyableTL
    {
        public BnfiTermRecordTL(Type type, string name = null)
            : base(type, name)
        {
        }

        public new BnfiExpressionRecordTL Rule { set { base.Rule = value; } }
    }

    public partial class BnfiTermRecord<TType> : BnfiTermRecord, IBnfiTerm<TType>, IBnfiTermCopyable<TType>, IBnfiTermOrAbleForChoice<TType>, INonTerminal<TType>
        where TType : new()
    {
        public static TType __ { get; private set; }

        static BnfiTermRecord()
        {
            __ = new TType();
        }

        public TType _ { get { return BnfiTermRecord<TType>.__; } }

        public BnfiTermRecord(string name = null)
            : base(typeof(TType), name)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionRecordTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionRecord<TType> Rule { set { base.Rule = value; } }

        // NOTE: type inference for superclasses works only if SetRulePlus is an instance method and not an extension method
        public void SetRulePlus(IBnfiTermPlusAbleForType<TType> bnfiTermFirst, IBnfiTermPlusAbleForType<TType> bnfiTermSecond, params IBnfiTermPlusAbleForType<TType>[] bnfiTerms)
        {
            this.Rule = Plus(bnfiTermFirst, bnfiTermSecond, bnfiTerms);
        }

        public BnfiExpressionRecord<TType> Plus(IBnfiTermPlusAbleForType<TType> bnfiTermFirst, IBnfiTermPlusAbleForType<TType> bnfiTermSecond, params IBnfiTermPlusAbleForType<TType>[] bnfiTerms)
        {
            return (BnfiExpressionRecord<TType>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                bnfiTermFirst.AsBnfTerm() + bnfiTermSecond.AsBnfTerm(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed + bnfTermToBeProcess
                );
        }
    }
}
