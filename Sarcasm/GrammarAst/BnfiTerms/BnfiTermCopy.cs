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
        private readonly BnfTerm childBnfTerm;

        protected BnfiTermCopy(Type type, BnfTerm bnfTerm)
            : base(type, name: null)
        {
            this.IsContractible = true;
            this.childBnfTerm = bnfTerm;
            this.Rule = bnfTerm.ToBnfExpression();
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermCopyable will contain the BnfiTerm... which actually does something)
        }

        public static BnfiTermCopyTL Copy(IBnfiTermCopyable bnfiTerm)
        {
            return new BnfiTermCopyTL(bnfiTerm.Type, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermCopy<T> Copy<T>(IBnfiTermCopyable<T> bnfiTerm)
        {
            return new BnfiTermCopy<T>(bnfiTerm.AsBnfTerm());
        }

        #region Unparse

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsableNonTerminal.GetChildren(IList<BnfTerm> childBnfTerms, object obj, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableObject(childBnfTerm, obj));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> children)
        {
            return children.SumIncludingNullValues(child => unparser.GetPriority(child));
        }

        #endregion
    }

    public partial class BnfiTermCopyTL : BnfiTermCopy, IBnfiTermTL
    {
        internal BnfiTermCopyTL(Type type, BnfTerm bnfTerm)
            : base(type, bnfTerm)
        {
        }
    }

    // NOTE: it does not implement IBnfiTermOrAbleForChoice<T>, instead it implements IBnfiTermPlusAbleForType<T>
    public partial class BnfiTermCopy<T> : BnfiTermCopy, IBnfiTerm<T>, IBnfiTermPlusAbleForType<T>, INonTerminal<T>
    {
        internal BnfiTermCopy(BnfTerm bnfTerm)
            : base(typeof(T), bnfTerm)
        {
        }
    }
}
