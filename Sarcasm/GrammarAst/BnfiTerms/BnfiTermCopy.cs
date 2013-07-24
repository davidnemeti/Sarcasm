using System;
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
        protected BnfiTermCopy(Type type, BnfTerm bnfTerm, string name)
            : base(type, name: name ?? GetName(type, bnfTerm))
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
            return new BnfiTermCopyTL(bnfiTerm.Type, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermCopy<T> Copy<T>(IBnfiTermCopyable<T> bnfiTerm)
        {
            return new BnfiTermCopy<T>(bnfiTerm.AsBnfTerm());
        }

        private static string GetName(Type type, BnfTerm bnfTerm)
        {
            string name = string.Empty;

            if (bnfTerm != null)
                name += bnfTerm.Name + "_";

            name += "copyAs_" + type.Name.ToLower();

            return name;
        }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object astValue, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            return children.SumIncludingNullValues(child => unparser.GetPriority(child));
        }

        #endregion

        protected new BnfiExpression Rule { set { base.Rule = value; } }

        public BnfExpression RuleRaw { get { return base.Rule; } set { base.Rule = value; } }
    }

    public partial class BnfiTermCopyTL : BnfiTermCopy, IBnfiTermTL
    {
        public BnfiTermCopyTL(Type type, string name = null)
            : base(type, bnfTerm: null, name: name)
        {
        }

        internal BnfiTermCopyTL(Type type, BnfTerm bnfTerm)
            : base(type, bnfTerm: bnfTerm, name: null)
        {
            if (bnfTerm == null)
                throw new ArgumentNullException("bnfTerm");
        }

        public new BnfiExpression Rule { set { base.Rule = value; } }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<T>, instead it implements IBnfiTermPlusAbleForType<T>
    public partial class BnfiTermCopy<T> : BnfiTermCopy, IBnfiTerm<T>, IBnfiTermPlusAbleForType<T>, INonTerminal<T>
    {
        public BnfiTermCopy(string name = null)
            : base(typeof(T), bnfTerm: null, name: name)
        {
        }

        internal BnfiTermCopy(BnfTerm bnfTerm)
            : base(typeof(T), bnfTerm: bnfTerm, name: null)
        {
            if (bnfTerm == null)
                throw new ArgumentNullException("bnfTerm");
        }

        public BnfiExpression RuleTypeless { set { base.Rule = value; } }

        public new BnfiExpressionGeneral<T> Rule { set { base.Rule = value; } }
    }
}
