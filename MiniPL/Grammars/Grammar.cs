using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.Ast;
using Sarcasm.Unparsing;

using MiniPL.DomainModel;

namespace MiniPL
{
    public class Grammar : Irony.Parsing.Grammar
    {
        public class BnfTerms
        {
            internal BnfTerms() { }

            public readonly BnfiTermType<Program> Program = new BnfiTermType<Program>();
        }

        public readonly BnfTerms B = new BnfTerms();

        public Grammar()
        {
        }
    }
}
