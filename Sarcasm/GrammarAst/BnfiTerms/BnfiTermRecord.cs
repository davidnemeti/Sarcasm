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
using Sarcasm.DomainCore;
using Sarcasm.Parsing;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermRecord : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        #region Types

        private abstract class ReferredBnfTerm : IEquatable<ReferredBnfTerm>
        {
            public readonly BnfTerm BnfTerm;
            public readonly int BnfTermIndex;

            protected ReferredBnfTerm(BnfTerm bnfTerm, int bnfTermIndex)
            {
                this.BnfTerm = bnfTerm;
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
                    this.BnfTermIndex == that.BnfTermIndex;
            }

            public override int GetHashCode()
            {
                return Util.GetHashCodeMulti(BnfTerm, BnfTermIndex);
            }
        }

        private class ReferredBnfTermEI : ReferredBnfTerm, IEquatable<ReferredBnfTermEI>
        {
            public readonly int EncloserBnfTermsIndex;

            public ReferredBnfTermEI(int encloserBnfTermsIndex, BnfTerm bnfTerm, int bnfTermIndex)
                : base(bnfTerm, bnfTermIndex)
            {
                this.EncloserBnfTermsIndex = encloserBnfTermsIndex;
            }

            public override bool Equals(object obj)
            {
                return obj is ReferredBnfTermEI && Equals((ReferredBnfTermEI)obj);
            }

            public bool Equals(ReferredBnfTermEI that)
            {
                return base.Equals(that) &&
                    this.EncloserBnfTermsIndex == that.EncloserBnfTermsIndex;
            }

            public override int GetHashCode()
            {
                return Util.GetHashCodeMulti(base.GetHashCode(), EncloserBnfTermsIndex);
            }
        }

        private class ReferredBnfTermEL : ReferredBnfTerm, IEquatable<ReferredBnfTermEL>
        {
            public readonly IList<BnfTerm> EncloserBnfTerms;

            public ReferredBnfTermEL(IList<BnfTerm> encloserBnfTerms, BnfTerm bnfTerm, int bnfTermIndex)
                : base(bnfTerm, bnfTermIndex)
            {
                this.EncloserBnfTerms = encloserBnfTerms;
            }

            public override bool Equals(object obj)
            {
                return obj is ReferredBnfTermEL && Equals((ReferredBnfTermEL)obj);
            }

            public bool Equals(ReferredBnfTermEL that)
            {
                return base.Equals(that) &&
                    this.EncloserBnfTerms.Count == that.EncloserBnfTerms.Count &&
                    this.EncloserBnfTerms.SequenceEqual(that.EncloserBnfTerms);
            }

            public override int GetHashCode()
            {
                return Util.GetHashCodeMulti(base.GetHashCode(), Util.GetHashCodeMulti(EncloserBnfTerms));
            }
        }

        #endregion

        #region State

        private IDictionary<ReferredBnfTermEL, Member> referredBnfTermAtParseToMember = new Dictionary<ReferredBnfTermEL, Member>();
        private IDictionary<ReferredBnfTermEI, Member> referredBnfTermAtRuleToMember = new Dictionary<ReferredBnfTermEI, Member>();

        #endregion

        internal const BindingFlags bindingAttrInstanceAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        protected BnfiTermRecord(Type domainType, string name)
            : base(domainType, name)
        {
            if (domainType.GetConstructor(bindingAttrInstanceAll, binder: null, types: Type.EmptyTypes, modifiers: null) == null)
                throw new ArgumentException("Type has no default constructor (neither public nor nonpublic)", "type");

            this.AstConfig.NodeCreator = (context, parseTreeNode) =>
            {
                try
                {
                    object astValue;
                    try
                    {
                        astValue = Activator.CreateInstance(domainType, nonPublic: true);
                    }
                    catch (MissingMemberException)
                    {
                        throw new AstException(string.Format("Type '{0}' does not have a parameterless public or internal constructor", domainType.FullName));
                    }

                    var parseChildBnfTerms = parseTreeNode.ChildNodes.Select(childParseTreeNode => childParseTreeNode.Term).ToList();

                    var parseChildValues = parseTreeNode.ChildNodes
                        .Select(
                            (parseChildNode, parseChildNodeIndex) => new
                                {
                                    ReferredBnfTerm = new ReferredBnfTermEL(parseChildBnfTerms, parseChildNode.Term, parseChildNodeIndex),
                                    Value = GrammarHelper.AstNodeToValue(parseChildNode.AstNode)
                                }
                            )
                        .Where(parseChildValue => parseChildValue.Value != null)
                        .ToList();

                    // 1. memberwise copy for BnfiTermCopy items
                    foreach (var parseChildValue in parseChildValues)
                        if (!IsMemberAtParse(parseChildValue.ReferredBnfTerm) && IsMemberwiseCopyable(astValue, parseChildValue.Value))
                            MemberwiseCopyExceptNullAndEmptyCollectionValues(astValue, parseChildValue.Value);

                    // 2. set member values for member items (it's the second step, so that we can overwrite the copied members if we want)
                    foreach (var parseChildValue in parseChildValues)
                        if (IsMemberAtParse(parseChildValue.ReferredBnfTerm))
                            SetValue(GetMemberAtParse(parseChildValue.ReferredBnfTerm).MemberInfo, astValue, parseChildValue.Value);

                    parseTreeNode.AstNode = GrammarHelper.ValueToAstNode(astValue, context, parseTreeNode);
                }
                catch (AstException e)
                {
                    context.AddMessage(AstException.ErrorLevel, parseTreeNode.Span.Location, e.Message);
                }
                catch (FatalAstException e)
                {
                    context.AddMessage(FatalAstException.ErrorLevel, parseTreeNode.Span.Location, e.Message);   // although it will be abandoned anyway
                    e.Location = parseTreeNode.Span.Location;
                    throw;
                }
            };
        }

        protected static bool IsMemberwiseCopyable(object destination, object source)
        {
            return source.GetType().IsAssignableFrom(destination.GetType());
        }

        protected static void MemberwiseCopyExceptNullAndEmptyCollectionValues(object destination, object source)
        {
            MemberwiseCopy(destination, source,
                (destinationFieldValue, sourceFieldValue) => sourceFieldValue != null && !IsEmptyCollection(sourceFieldValue) || destinationFieldValue == null);
        }

        protected static void MemberwiseCopy(object destination, object source)
        {
            MemberwiseCopy(destination, source, (destinationFieldValue, sourceFieldValue) => true);
        }

        protected static void MemberwiseCopy(object destination, object source, Func<object, object, bool> cloneFieldValue)
        {
            Type destinationType = destination.GetType();
            Type sourceType = source.GetType();

            if (!IsMemberwiseCopyable(destination, source))
                throw new ArgumentException(string.Format("{0} is not assignable from {1}", destinationType.Name, sourceType.Name), "source");

            foreach (FieldInfo fieldInfo in GetAllFields(sourceType))
            {
                object destinationFieldValue = fieldInfo.GetValue(destination);
                object sourceFieldValue = fieldInfo.GetValue(source);

                if (cloneFieldValue(destinationFieldValue, sourceFieldValue))
                    fieldInfo.SetValue(destination, sourceFieldValue);
            }
        }

        protected static bool IsEmptyCollection(object value)
        {
            var items = value as System.Collections.IEnumerable;

            return items != null && !items.Cast<object>().Any();
        }

        protected static IEnumerable<FieldInfo> GetAllFields(Type domainType)
        {
#if WINDOWS_STORE
            return domainType.GetRuntimeFields();
#else
            return domainType.GetFields(bindingAttrInstanceAll)
                .Concat(domainType.BaseType != null
                    ? GetAllFields(domainType.BaseType)
                    : new FieldInfo[0]);
#endif
        }

        protected static void SetValue(MemberInfo memberInfo, object obj, object value)
        {
            if (memberInfo is PropertyInfo)
                ((PropertyInfo)memberInfo).SetValue(obj, value, index: null);
            else if (memberInfo is FieldInfo)
                ((FieldInfo)memberInfo).SetValue(obj, value);
            else
                throw new AstException("Object with wrong type in memberinfo: " + memberInfo.Name);
        }

        protected static object GetValue(MemberInfo memberInfo, object obj)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).GetValue(obj, index: null);
            else if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).GetValue(obj);
            else
                throw new AstException("Object with wrong type in memberinfo: " + memberInfo.Name);
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
            int bnfTermsIndex = 0;

            foreach (BnfTermList bnfTerms in bnfiExpression.GetBnfTermsList())
            {
                int bnfTermIndexAtParse = 0;
                int bnfTermIndexAtRule = 0;

                for (int bnfTermIndex = 0; bnfTermIndex < bnfTerms.Count; bnfTermIndex++)
                {
                    if (bnfTerms[bnfTermIndex] is Member)
                    {
                        Member member = (Member)bnfTerms[bnfTermIndex];

                        RegisterMember(member.BnfTerm, bnfTerms, bnfTermsIndex, bnfTermIndexAtParse, bnfTermIndexAtRule, member);
                        bnfTerms[bnfTermIndex] = member.BnfTerm;
                    }

                    if (!(bnfTerms[bnfTermIndex] is GrammarHint))
                    {
                        bnfTermIndexAtRule++;

                        // NOTE: punctuation terminals will not be put into the parsetree, that's why we have to index like this
                        if (!bnfTerms[bnfTermIndex].IsPunctuation())
                            bnfTermIndexAtParse++;
                    }
                }

                bnfTermsIndex++;
            }
        }

        private void RegisterMember(BnfTerm bnfTerm, IList<BnfTerm> rawEncloserBnfTerms, int rawEncloserBnfTermsIndex, int bnfTermIndexAtParse, int bnfTermIndexAtRule, Member member)
        {
            var encloserBnfTermsAtRule = rawEncloserBnfTerms
                .Where(_bnfTerm => !(_bnfTerm is GrammarHint))
                .Select(_bnfTerm => _bnfTerm is Member ? ((Member)_bnfTerm).BnfTerm : _bnfTerm)
                .ToList();

            var encloserBnfTermsAtParse = encloserBnfTermsAtRule
                .Where(encloserBnfTermAtRule => !encloserBnfTermAtRule.IsPunctuation())
                .ToList();

            this.referredBnfTermAtRuleToMember.Add(new ReferredBnfTermEI(rawEncloserBnfTermsIndex, bnfTerm, bnfTermIndexAtRule), member);
            this.referredBnfTermAtParseToMember.Add(new ReferredBnfTermEL(encloserBnfTermsAtParse, bnfTerm, bnfTermIndexAtParse), member);
        }

        private bool IsMemberAtParse(ReferredBnfTermEL referredBnfTerm)
        {
            return this.referredBnfTermAtParseToMember.ContainsKey(referredBnfTerm);
        }

        private Member GetMemberAtParse(ReferredBnfTermEL referredBnfTerm)
        {
            return this.referredBnfTermAtParseToMember[referredBnfTerm];
        }

        private bool IsMemberAtRule(ReferredBnfTermEI referredBnfTerm)
        {
            return this.referredBnfTermAtRuleToMember.ContainsKey(referredBnfTerm);
        }

        private Member GetMemberByAtRule(ReferredBnfTermEI referredBnfTerm)
        {
            return this.referredBnfTermAtRuleToMember[referredBnfTerm];
        }

        #region Unparse

        protected override bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        protected override IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            foreach (var childRuleReferredBnfTerm in childBnfTerms.Select((childBnfTerm, index) =>
                new ReferredBnfTermEI(childBnfTerms.ContentIndex, childBnfTerm, direction == Unparser.Direction.LeftToRight ? index : childBnfTerms.Count - 1 - index)
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

                if (childAstValue == null && childRuleReferredBnfTerm.BnfTerm is BnfiTermConversion && ((BnfiTermConversion)childRuleReferredBnfTerm.BnfTerm).isOptionalValue)
                    continue;

                yield return new UnparsableAst(childRuleReferredBnfTerm.BnfTerm, childAstValue, member);
            }
        }

        protected override int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children childrenAtRule, Unparser.Direction direction)
        {
            return childrenAtRule
                .SumIncludingNullValues(
                    (childAtRule, childIndexAtRule) => IsMemberAtRule(new ReferredBnfTermEI(childrenAtRule.ContentIndex, childAtRule.BnfTerm, childIndexAtRule))
                        ? GetBnfTermPriorityForMember(unparser, childAtRule)
                        : unparser.GetPriority(childAtRule)
                    );
        }

        private static int? GetBnfTermPriorityForMember(IUnparser unparser, UnparsableAst unparsableAst)
        {
            if (unparsableAst.AstValue != null)
                return 1;
            else if (unparsableAst.BnfTerm is BnfiTermCollection && ((BnfiTermCollection)unparsableAst.BnfTerm).EmptyCollectionHandling == EmptyCollectionHandling.ReturnNull)
                return 0;
            else if (unparsableAst.BnfTerm is BnfiTermConversion && ((BnfiTermConversion)unparsableAst.BnfTerm).isOptionalValue)
                return 0;
            else
                return null;
        }

        #endregion
    }

    public partial class BnfiTermRecordTL : BnfiTermRecord, IBnfiTermTL, IBnfiTermCopyableTL
    {
        public BnfiTermRecordTL(Type domainType, string name = null)
            : base(domainType, name)
        {
        }

        public new BnfiExpressionRecordTL Rule { set { base.Rule = value; } }
    }

    public partial class BnfiTermRecord<TD> : BnfiTermRecord, IBnfiTerm<TD>, IBnfiTermCopyable<TD>, IBnfiTermOrAbleForChoice<TD>, INonTerminal<TD>
        where TD : new()
    {
        public static TD __ { get; private set; }

        static BnfiTermRecord()
        {
            __ = new TD();
        }

        public TD _ { get { return BnfiTermRecord<TD>.__; } }

        public BnfiTermRecord(string name = null)
            : base(typeof(TD), name)
        {
        }

        [Obsolete(typelessQErrorMessage, error: true)]
        public new BnfExpression Q()
        {
            return base.Q();
        }

        public BnfiExpressionRecordTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionRecord<TD> Rule { set { base.Rule = value; } }

        // NOTE: type inference for superclasses works only if SetRulePlus is an instance method and not an extension method
        public void SetRulePlus(IBnfiTermPlusAbleForType<TD> bnfiTermFirst, IBnfiTermPlusAbleForType<TD> bnfiTermSecond, params IBnfiTermPlusAbleForType<TD>[] bnfiTerms)
        {
            this.Rule = Plus(bnfiTermFirst, bnfiTermSecond, bnfiTerms);
        }

        public BnfiExpressionRecord<TD> Plus(IBnfiTermPlusAbleForType<TD> bnfiTermFirst, IBnfiTermPlusAbleForType<TD> bnfiTermSecond, params IBnfiTermPlusAbleForType<TD>[] bnfiTerms)
        {
            return (BnfiExpressionRecord<TD>)bnfiTerms
                .Select(bnfiTerm => bnfiTerm.AsBnfTerm())
                .Aggregate(
                bnfiTermFirst.AsBnfTerm() + bnfiTermSecond.AsBnfTerm(),
                (bnfExpressionProcessed, bnfTermToBeProcess) => bnfExpressionProcessed + bnfTermToBeProcess
                );
        }
    }
}
