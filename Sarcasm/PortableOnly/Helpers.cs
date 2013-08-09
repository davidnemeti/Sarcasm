#if PCL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sarcasm
{
    public static class TaskEx
    {
        public static Task Run(Action action)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }

    public class TraceSource
    {
        public TraceSource(string name = null, SourceLevels sourceLevels = SourceLevels.Verbose)
        {
        }
    }

    public enum TraceEventType
    {
    }

    public enum SourceLevels
    {
        Verbose
    }
}

#endif
