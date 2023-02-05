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
using System.Runtime.Serialization;

namespace Sarcasm.Unparsing
{
    [Serializable]
    public class UnparserInitializationException : Exception
    {
        public UnparserInitializationException()
        {
        }

        public UnparserInitializationException(string message)
            : base(message)
        {
        }

        protected UnparserInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class UnparseException : Exception
    {
        public UnparseException()
        {
        }

        public UnparseException(string message)
            : base(message)
        {
        }

        protected UnparseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    internal class NonCalculatedException : Exception
    {
        public NonCalculatedException()
        {
        }

        public NonCalculatedException(string message)
            : base(message)
        {
        }

        protected NonCalculatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    internal class ThrownOutException : Exception
    {
        public ThrownOutException()
        {
        }

        public ThrownOutException(string message)
            : base(message)
        {
        }

        protected ThrownOutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
