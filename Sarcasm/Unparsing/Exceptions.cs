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
using Sarcasm.Ast;

using Grammar = Sarcasm.Ast.Grammar;

namespace Sarcasm.Unparsing
{
    public class UnparserInitializationException : Exception
    {
        public UnparserInitializationException()
        {
        }

        public UnparserInitializationException(string message)
            : base(message)
        {
        }
    }

    public class UnparseException : Exception
    {
        public UnparseException()
        {
        }

        public UnparseException(string message)
            : base(message)
        {
        }
    }

    internal class NonCalculatedException : Exception
    {
        public NonCalculatedException()
        {
        }

        public NonCalculatedException(string message)
            : base(message)
        {
        }
    }

    internal class ThrownOutException : Exception
    {
        public ThrownOutException()
        {
        }

        public ThrownOutException(string message)
            : base(message)
        {
        }
    }
}
