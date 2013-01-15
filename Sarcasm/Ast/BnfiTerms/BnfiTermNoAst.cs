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
    public partial class BnfiTermNoAst : NonTerminal, IBnfiTerm, IUnparsable
    {
        private readonly BnfTerm childBnfTerm;

        protected BnfiTermNoAst(BnfTerm bnfTerm)
            : base(string.Format("{0}->no_ast", bnfTerm.Name))
        {
            this.childBnfTerm = bnfTerm;
            this.Rule = new BnfExpression(bnfTerm);
            this.SetFlag(TermFlags.NoAstNode);
        }

        public static BnfiTermNoAst Create(BnfTerm bnfTerm)
        {
            return new BnfiTermNoAst(bnfTerm);
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
            return childBnfTerms.Select(childBnfTerm => new Value(childBnfTerm, obj: null));
        }

        int? IUnparsable.GetChildBnfTermListPriority(IUnparser unparser, object obj, IEnumerable<Value> childValues)
        {
            return 0;
        }

        #endregion
    }
}
