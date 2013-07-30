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
    public partial class BnfiTermNoAst : BnfiTermNonTerminal, IBnfiTerm, IUnparsableNonTerminal
    {
        public BnfiTermNoAst(string name = null)
            : base(name)
        {
            this.SetFlag(TermFlags.NoAstNode);
        }

        protected BnfiTermNoAst(BnfTerm bnfTerm)
            : this(string.Format("{0}->no_ast", bnfTerm.Name))
        {
            base.Rule = bnfTerm.ToBnfExpression();
        }

        public static BnfiTermNoAst For(BnfTerm bnfTerm)
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

        bool IUnparsableNonTerminal.TryGetUtokensDirectly(IUnparser unparser, object astValue, out IEnumerable<UtokenValue> utokens)
        {
            utokens = null;
            return false;
        }

        IEnumerable<UnparsableAst> IUnparsableNonTerminal.GetChildren(Unparser.ChildBnfTerms childBnfTerms, object astValue, Unparser.Direction direction)
        {
            return childBnfTerms.Select(childBnfTerm => new UnparsableAst(childBnfTerm, astValue: null));
        }

        int? IUnparsableNonTerminal.GetChildrenPriority(IUnparser unparser, object astValue, Unparser.Children children, Unparser.Direction direction)
        {
            return 0;
        }

        #endregion

        public new BnfiExpressionNoAst Rule { set { base.Rule = value; } }
    }
}
