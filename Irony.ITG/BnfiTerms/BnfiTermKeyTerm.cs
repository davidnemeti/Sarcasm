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
    public partial class BnfiTermKeyTerm : KeyTerm, IBnfiTerm
    {
        public BnfiTermKeyTerm(string text, string name)
            : base(text, name)
        {
        }

        public BnfTerm AsBnfTerm()
        {
            return this;
        }
    }
}
