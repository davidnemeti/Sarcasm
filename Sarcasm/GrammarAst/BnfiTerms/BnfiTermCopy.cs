﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public abstract partial class BnfiTermCopy : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        protected BnfiTermCopy(Type domainType, BnfTerm bnfTerm, string name)
            : base(domainType, name: name ?? GetName(domainType, bnfTerm))
        {
            if (bnfTerm != null)
            {
                // "this" BnfiTermCopy is not an independent bnfTerm, just a syntax magic for BnfiTermRecord<TType> (we were called by the Copy method)
                this.IsContractible = true;
                this.RuleRaw = bnfTerm.ToBnfExpression();
            }
            else
            {
                // "this" BnfiTermCopy is an independent bnfTerm
                this.IsContractible = false;
            }

            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermCopyable will contain the BnfiTerm... which actually does something)
        }

        public static BnfiTermCopyTL Copy(IBnfiTermCopyableTL bnfiTerm)
        {
            return new BnfiTermCopyTL(bnfiTerm.DomainType, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermCopy<T> Copy<T>(IBnfiTermCopyable<T> bnfiTerm)
        {
            return new BnfiTermCopy<T>(bnfiTerm.AsBnfTerm());
        }

        private static string GetName(Type domainType, BnfTerm bnfTerm)
        {
            string name = string.Empty;

            if (bnfTerm != null)
                name += bnfTerm.Name + "_";

            name += "copyAs_" + domainType.Name.ToLower();

            return name;
        }

        #region Unparse

        protected override bool TryGetUtokensDirectly(IUnparser unparser, UnparsableAst self, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        protected override IEnumerable<UnparsableAst> GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue));
        }

        protected override int? GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            return children.SumIncludingNullValues(child => unparser.GetPriority(child));
        }

        #endregion

        protected new BnfiExpression Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }
    }

    public partial class BnfiTermCopyTL : BnfiTermCopy, IBnfiTermTL
    {
        public BnfiTermCopyTL(Type domainType, string name = null)
            : base(domainType, bnfTerm: null, name: name)
        {
        }

        internal BnfiTermCopyTL(Type domainType, BnfTerm bnfTerm)
            : base(domainType, bnfTerm: bnfTerm, name: null)
        {
            if (bnfTerm == null)
                throw new ArgumentNullException("bnfTerm");
        }

        public new BnfiExpressionChoiceTL Rule { set { base.Rule = value; } }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<TD>, instead it implements IBnfiTermPlusAbleForType<TD>
    public partial class BnfiTermCopy<TD> : BnfiTermCopy, IBnfiTerm<TD>, IBnfiTermPlusAbleForType<TD>, INonTerminal<TD>
    {
        public BnfiTermCopy(string name = null)
            : base(typeof(TD), bnfTerm: null, name: name)
        {
        }

        internal BnfiTermCopy(BnfTerm bnfTerm)
            : base(typeof(TD), bnfTerm: bnfTerm, name: null)
        {
            if (bnfTerm == null)
                throw new ArgumentNullException("bnfTerm");
        }

        public BnfiExpressionChoiceTL RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionChoice<TD> Rule { set { base.Rule = value; } }
    }
}
