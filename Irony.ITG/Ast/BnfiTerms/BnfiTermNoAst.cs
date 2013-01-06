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

        public IEnumerable<Utoken> Unparse(IUnparser unparser, object obj)
        {
            foreach (Utoken utoken in unparser.Unparse(obj: null, bnfTerm: childBnfTerm))
                yield return utoken;
        }
    }
}
