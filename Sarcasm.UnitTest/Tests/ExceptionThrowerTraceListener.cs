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
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sarcasm.UnitTest
{
    /// <summary>
    /// Trace listener which converts asserts to exceptions. It is mainly used in unit testing.
    /// </summary>
    public class ExceptionThrowerTraceListener : TraceListener
    {
        private ExceptionThrowerTraceListener()
        {
        }

        public static void Register()
        {
#if !PCL
            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debug.Listeners.Insert(0, new ExceptionThrowerTraceListener());
#endif
        }

#if !PCL
        public override void Fail(string message, string detailMessage)
        {
            string messageException = "Assert failed: " + message;

            if (!string.IsNullOrEmpty(detailMessage))
                messageException += string.Format(" [{0}]", detailMessage);

            throw new AssertionFailedException(messageException);
        }

        public override void Write(string message)
        {
            // do nothing
        }

        public override void WriteLine(string message)
        {
            // do nothing
        }
#endif
    }

    [Serializable]
    public class AssertionFailedException : Exception
    {
        public AssertionFailedException(string message)
            : base(message)
        {
        }

        protected AssertionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
