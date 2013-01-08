using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Irony.ITG
{
    public static class Util
    {
        public static bool EqualToAny<T>(this T value, T candidateValue)
        {
            return EqualityComparer<T>.Default.Equals(value, candidateValue);
        }

        public static bool EqualToAny<T>(this T value, T candidateValue1, T candidateValue2)
        {
            return EqualityComparer<T>.Default.Equals(value, candidateValue1) || EqualityComparer<T>.Default.Equals(value, candidateValue2);
        }

        public static bool EqualToAny<T>(this T value, T candidateValue1, T candidateValue2, T candidateValue3)
        {
            return EqualityComparer<T>.Default.Equals(value, candidateValue1)
                || EqualityComparer<T>.Default.Equals(value, candidateValue2)
                || EqualityComparer<T>.Default.Equals(value, candidateValue3);
        }

        public static bool EqualToAny<T>(this T value, params T[] candidateValues)
        {
            return candidateValues.Contains(value);
        }

        public static IEnumerable<T> TraceWriteLines<T>(this IEnumerable<T> items, string category = null)
        {
            foreach (T item in items)
            {
                System.Diagnostics.Trace.WriteLine(item, category);
                yield return item;
            }
        }

        public static IEnumerable<T> TraceWriteLines<T>(this IEnumerable<T> items, TraceSource ts, TraceEventType traceEventType)
        {
            foreach (T item in items)
            {
                ts.Trace(traceEventType, item);
                yield return item;
            }
        }

        public static IEnumerable<T> DebugWriteLines<T>(this IEnumerable<T> items, TraceSource ts)
        {
#if DEBUG
            foreach (T item in items)
            {
                ts.Debug(item);
                yield return item;
            }
#else
            return items;
#endif
        }

        public static IEnumerable<T> DebugWriteLines<T>(this IEnumerable<T> items, string category = null)
        {
#if DEBUG
            foreach (T item in items)
            {
                System.Diagnostics.Debug.WriteLine(item, category);
                yield return item;
            }
#else
            return items;
#endif
        }

        [Conditional("TRACE")]
        public static void Indent(this TraceSource ts)
        {
            foreach (TraceListener listener in ts.Listeners)
                listener.IndentLevel++;
        }

        [Conditional("TRACE")]
        public static void Unindent(this TraceSource ts)
        {
            foreach (TraceListener listener in ts.Listeners)
                listener.IndentLevel--;
        }

        [Conditional("TRACE")]
        public static void Trace(this TraceSource ts, TraceEventType traceEventType, object obj)
        {
            ts.TraceEvent(traceEventType, 0, obj.ToString());
        }

        [Conditional("TRACE")]
        public static void Trace(this TraceSource ts, TraceEventType traceEventType, string message)
        {
            ts.TraceEvent(traceEventType, 0, message);
        }

        [Conditional("TRACE")]
        public static void Trace(this TraceSource ts, TraceEventType traceEventType, string format, params object[] args)
        {
            ts.TraceEvent(traceEventType, 0, format, args);
        }

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, object obj)
        {
            ts.TraceEvent(TraceEventType.Verbose, 0, obj.ToString());
        }

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, string message)
        {
            ts.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, string format, params object[] args)
        {
            ts.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }
    }
}
