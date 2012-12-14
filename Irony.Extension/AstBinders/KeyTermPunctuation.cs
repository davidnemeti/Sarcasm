using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Ast;
using Irony.Parsing;

namespace Irony.Extension.AstBinders
{
    public class KeyTermPunctuation : KeyTerm
    {
        public KeyTermPunctuation(string text)
            : base(text, text)
        {
            this.SetFlag(TermFlags.IsPunctuation | TermFlags.NoAstNode);
        }
    }
}
