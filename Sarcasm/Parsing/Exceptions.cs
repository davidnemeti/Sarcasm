using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm;
using Sarcasm.GrammarAst;

using Grammar = Sarcasm.GrammarAst.Grammar;

namespace Sarcasm.Parsing
{
    public class ParseException : Exception
    {
        public ParseException()
        {
        }

        public ParseException(string message)
            : base(message)
        {
        }
    }

    public class FatalParseException : Exception
    {
        public SourceLocation Location { get; internal set; }

        public FatalParseException()
        {
        }

        public FatalParseException(string message)
            : base(message)
        {
        }
    }
}
