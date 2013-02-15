using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.Ast
{
    public abstract partial class BnfiTermCopy : BnfiTermNonTerminal, IBnfiTerm, IUnparsable
    {
        private readonly BnfTerm childBnfTerm;

        protected BnfiTermCopy(Type type, BnfTerm bnfTerm)
            : base(type, name: null, isReferable: false)
        {
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

        bool IUnparsable.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableObject> IUnparsable.GetChildUnparsableObjects(BnfTermList childBnfTerms, object obj)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableObject(childBnfTerm, obj));
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<UnparsableObject> childUnparsableObjects)
        {
            return childUnparsableObjects.SumIncludingNullValues(childValue => unparser.GetBnfTermPriority(childValue.bnfTerm, childValue.obj));
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
    public partial class BnfiTermCopy<T> : BnfiTermCopy, IBnfiTerm<T>, IBnfiTermPlusAbleForType<T>
    {
        internal BnfiTermCopy(BnfTerm bnfTerm)
            : base(typeof(T), bnfTerm)
        {
        }
    }
}
