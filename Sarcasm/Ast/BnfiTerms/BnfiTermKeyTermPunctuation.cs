using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Ast;
using Irony.Parsing;

namespace Sarcasm.GrammarAst
{
    public partial class BnfiTermKeyTermPunctuation : BnfiTermKeyTerm
    {
        public BnfiTermKeyTermPunctuation(string text)
            : base(text, text)
        {
            this.SetFlag(TermFlags.IsPunctuation | TermFlags.NoAstNode);
        }
    }
}
