#if PCL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Sarcasm
{
    public static class Extensions
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return (TAttribute)type.GetCustomAttributes(typeof(TAttribute), inherit: false).FirstOrDefault();
        }

        public static IEnumerable<char> Take(this string source, int count)
        {
            return source.Cast<char>().Take(count);
        }

#if WINDOWS_STORE
        public static bool IsAssignableFrom(this Type type, Type fromType)
        {
            return type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
        }
#endif
    }

    [AttributeUsageAttribute(AttributeTargets.Parameter, Inherited = false)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }

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

        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            return Task.Factory.StartNew(function, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }

    public class TraceSource
    {
        public TraceSource(string name = null, SourceLevels sourceLevels = SourceLevels.Verbose)
        {
            this.Listeners = new List<TraceListener>();
        }

        public void Trace(TraceEventType traceEventType, int id, string message)
        {
        }

        public void TraceEvent(TraceEventType traceEventType, int id, string format, params object[] args)
        {
        }

        public IList<TraceListener> Listeners { get; private set; }
    }

    public class TraceListener
    {
        public int IndentLevel { get; set; }
    }

    public enum TraceEventType
    {
        Verbose
    }

    public enum SourceLevels
    {
        Verbose
    }
}

#endif
