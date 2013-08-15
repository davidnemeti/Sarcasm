#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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
