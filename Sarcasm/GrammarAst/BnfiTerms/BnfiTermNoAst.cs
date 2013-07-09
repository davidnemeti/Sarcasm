using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.Unparsing;

namespace Sarcasm.GrammarAst
{
    public partial class BnfiTermNoAst : NonTerminal, IBnfiTerm, IUnparsableNonTerminal
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
            return 0;
        }

        #endregion
    }
}
