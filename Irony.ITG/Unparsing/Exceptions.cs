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
using Irony.ITG;
using Irony.ITG.Ast;

using Grammar = Irony.ITG.Ast.Grammar;

namespace Irony.ITG.Unparsing
{
    public abstract class UnparseException : Exception
    {
        public UnparseException()
        {
        }

        public UnparseException(string message)
            : base(message)
        {
        }
    }

    public class CannotUnparseException : UnparseException
    {
        public CannotUnparseException()
        {
        }

        public CannotUnparseException(string message)
            : base(message)
        {
        }
    }

    public class UnhandledInternalUnparseErrorException : UnparseException
    {
        public UnhandledInternalUnparseErrorException(Error error, string message)
            : base(string.Format("Unhandled internal unparse error: {0}, \"{1}\"", error, message))
        {
        }
    }
}
