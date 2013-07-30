using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public partial class BnfiTermKeyTerm : KeyTerm, IBnfiTerm
    {
        public BnfiTermKeyTerm(string text, string name)
            : base(text, name)
        {
            // base class KeyTerm will set TermFlags.NoAstNode
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public BnfiTermKeyTerm ToPunctuation()
        {
            this.SetFlag(TermFlags.IsPunctuation);

            return this;
        }
    }
}
