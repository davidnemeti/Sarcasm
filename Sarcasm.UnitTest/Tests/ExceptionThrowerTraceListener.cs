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
            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debug.Listeners.Insert(0, new ExceptionThrowerTraceListener());
        }

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
