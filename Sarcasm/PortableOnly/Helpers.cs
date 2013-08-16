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
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Sarcasm
{
#if PCL
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

#if NET4_0
    // in .NET 4.0 SpinWait exists but not portable
    public struct SpinWait
    {
        public static void SpinUntil(Func<bool> condition)
        {
            while (!condition())
            {
                // do nothing

                /*
                 * In portable .NET 4.0 almost nothing is accessible what we could use here (Thread.Sleep, Thread.Yield) to implement SpinUntil,
                 * so we just doing nothing inside the loop.
                 * 
                 * NOTE that a loop with a condition which body is really empty would actively use the CPU which would result in a performance decrease.
                 * So in the body we use a little trick: we create a task which does nothing and we are waiting for that task.
                 * Performance measurements confirm that this implementation of SpinUntil is as efficient as the original .NET version.
                 * */

                Task.Factory.StartNew(() => { /* do nothing */ }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
            }
        }
    }
#endif

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
#endif

#if NET4_0
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

        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            var task = new Task<TResult>(() => result);
            task.RunSynchronously();
            return task;
        }
    }
#endif
}
