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
    public class AstException : Exception
    {
        public AstException()
        {
        }

        public AstException(string message)
            : base(message)
        {
        }

        internal readonly static ErrorLevel ErrorLevel = ErrorLevel.Error;
    }

    public class FatalAstException : Exception
    {
        public SourceLocation Location { get; internal set; }

        public FatalAstException()
        {
        }

        public FatalAstException(string message)
            : base(message)
        {
        }

        internal readonly static ErrorLevel ErrorLevel = ErrorLevel.Error;
    }
}
