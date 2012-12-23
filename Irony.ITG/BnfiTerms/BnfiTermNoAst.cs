using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.ITG
{
    public partial class BnfiTermNoAst : NonTerminal, IBnfiTerm
    {
        protected BnfiTermNoAst(BnfTerm bnfTerm)
            : base(string.Format("{0}->no_ast", bnfTerm.Name))
        {
            this.SetFlag(TermFlags.NoAstNode);
        }

        public static BnfiTermNoAst Create(BnfTerm bnfTerm)
        {
            return new BnfiTermNoAst(bnfTerm);
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }
}
