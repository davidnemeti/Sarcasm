using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.ITG.Unparsing;

namespace Irony.ITG.Ast
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

        public static BnfiTermCopyable Copy(IBnfiTerm bnfiTerm)
        {
            return Copy(typeof(object), bnfiTerm);
        }

        public static BnfiTermCopyable Copy(Type type, IBnfiTerm bnfiTerm)
        {
            return new BnfiTermCopyable(type, bnfiTerm.AsBnfTerm());
        }

        public static BnfiTermCopyable<T> Copy<T>(IBnfiTerm<T> bnfiTerm)
        {
            return new BnfiTermCopyable<T>(bnfiTerm.AsBnfTerm());
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public IEnumerable<Utoken> Unparse(Unparser unparser, object obj)
        {
            foreach (Utoken utoken in unparser.Unparse(obj, childBnfTerm))
                yield return utoken;
        }
    }

    public partial class BnfiTermCopyable<T> : BnfiTermCopyable, IBnfiTerm<T>, IBnfiTermCopyable<T>
    {
        internal BnfiTermCopyable(BnfTerm bnfTerm, string errorAlias = null)
            : base(typeof(T), bnfTerm, errorAlias)
        {
        }
    }
}
