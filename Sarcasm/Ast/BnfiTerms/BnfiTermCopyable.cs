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
    public partial class BnfiTermCopyable : BnfiTermNonTerminal, IBnfiTerm, IBnfiTermCopyable, IUnparsable
    {
        private readonly BnfTerm childBnfTerm;

        protected BnfiTermCopyable(Type type, BnfTerm bnfTerm, string errorAlias = null)
            : base(type, errorAlias)
        {
            this.childBnfTerm = bnfTerm;
            this.Rule = new BnfExpression(bnfTerm);
            GrammarHelper.MarkTransientForced(this);    // default "transient" behavior (the Rule of this BnfiTermCopyable will contain the BnfiTerm... which actually does something)
        }

        public static BnfiTermCopyable Copy(IBnfiTermCopyable bnfiTerm)
        {
            return new BnfiTermCopyable(bnfiTerm.Type, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermCopyable<T> Copy<T>(IBnfiTermCopyable<T> bnfiTerm)
        {
            return new BnfiTermCopyable<T>(bnfiTerm.AsBnfTerm());
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        #region Unparse

        bool IUnparsable.TryGetUtokensDirectly(IUnparser unparser, object obj, out IEnumerable<Utoken> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<Value> IUnparsable.GetChildValues(BnfTermList childBnfTerms, object obj)
        {
            return childBnfTerms.Select(childBnfTerm => new Value(childBnfTerm, obj));
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<Value> childValues)
        {
            return childValues.SumIncludingNullValues(childValue => unparser.GetBnfTermPriority(childValue.bnfTerm, childValue.obj));
        }

        #endregion
    }

    public partial class BnfiTermCopyable<T> : BnfiTermCopyable, IBnfiTerm<T>, IBnfiTermCopyable<T>
    {
        internal BnfiTermCopyable(BnfTerm bnfTerm, string errorAlias = null)
            : base(typeof(T), bnfTerm, errorAlias)
        {
        }
    }
}
