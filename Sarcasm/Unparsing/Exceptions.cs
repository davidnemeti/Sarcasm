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
using System.Runtime.Serialization;

namespace Sarcasm.Unparsing
{
#if !PCL
    [Serializable]
#endif
    public class UnparserInitializationException : Exception
    {
        public UnparserInitializationException()
        {
        }

        public UnparserInitializationException(string message)
            : base(message)
        {
        }

#if !PCL
        protected UnparserInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

#if !PCL
    [Serializable]
#endif
    public class UnparseException : Exception
    {
        public UnparseException()
        {
        }

        public UnparseException(string message)
            : base(message)
        {
        }

#if !PCL
        protected UnparseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

#if !PCL
    [Serializable]
#endif
    internal class NonCalculatedException : Exception
    {
        public NonCalculatedException()
        {
        }

        public NonCalculatedException(string message)
            : base(message)
        {
        }

#if !PCL
        protected NonCalculatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

#if !PCL
    [Serializable]
#endif
    internal class ThrownOutException : Exception
    {
        public ThrownOutException()
        {
        }

        public ThrownOutException(string message)
            : base(message)
        {
        }

#if !PCL
        protected ThrownOutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
