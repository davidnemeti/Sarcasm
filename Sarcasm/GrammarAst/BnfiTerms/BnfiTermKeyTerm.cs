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
        }

        BnfTerm IBnfiTerm.AsBnfTerm()
        {
            return this;
        }

        public object Tag { get; set; }
    }
}
