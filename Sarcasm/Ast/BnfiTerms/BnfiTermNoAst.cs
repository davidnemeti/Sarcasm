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
            this.Rule = bnfTerm.ToBnfExpression();
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

        NonTerminal INonTerminal.AsNonTerminal()
        {
            return this;
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
            return 0;
        }

        #endregion
    }
}
