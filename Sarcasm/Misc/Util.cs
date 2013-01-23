using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sarcasm
{
    public static class Util
    {
        public static string ToString(IFormatProvider formatProvider, object obj)
        {
            return string.Format(formatProvider, "{0}", obj);
        }

        public static int? SumIncludingNullValues<TSource>(this IEnumerable<TSource> items, Func<TSource, int?> selector)
        {
            return items.Select(selector).SumIncludingNullValues();
        }

        public static int? SumIncludingNullValues(this IEnumerable<int?> items)
        {
            int sum = 0;

            foreach (int? item in items)
            {
                if (item.HasValue)
                    sum += item.Value;
                else
                    return null;    // does not need to go further, since the result will be null anyway
            }

            return sum;
        }

        public static int? GetInheritanceDistance(this Type ancestor, object descendantObj)
        {
            return GetInheritanceDistance(ancestor, descendantObj.GetType());
        }

        public static int? GetInheritanceDistance(this Type ancestor, Type descendant)
        {
            if (ancestor == null)
                throw new ArgumentNullException("ancestor");

            if (descendant == null)
                return null;
            else if (descendant == ancestor)
                return 0;
            else
            {
                return new[] { descendant.BaseType }.Concat(descendant.GetInterfaces())
                    .Select(parentType => GetInheritanceDistance(ancestor, parentType))
                    .FirstOrDefault(distance => distance != null);
            }
        }

        #region EqualToAny

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

        #endregion

        #region Trace and Debug

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

        public static IEnumerable<T> TraceWriteLines<T>(this IEnumerable<T> items, TraceSource ts, TraceEventType traceEventType)
        {
            return items.TraceWriteLines(ts, traceEventType, item => item.ToString());
        }

        public static IEnumerable<T> TraceWriteLines<T>(this IEnumerable<T> items, TraceSource ts, TraceEventType traceEventType, Func<T, string> toString)
        {
            if (ts.Listeners.Count > 0)
                return items._TraceWriteLines(ts, traceEventType, toString);
            else
                return items;
        }

        private static IEnumerable<T> _TraceWriteLines<T>(this IEnumerable<T> items, TraceSource ts, TraceEventType traceEventType, Func<T, string> toString)
        {
            foreach (T item in items)
            {
                ts.Trace(traceEventType, toString(item));
                yield return item;
            }
        }

        public static IEnumerable<T> DebugWriteLines<T>(this IEnumerable<T> items, TraceSource ts)
        {
            return items.DebugWriteLines(ts, item => item.ToString());
        }

        public static IEnumerable<T> DebugWriteLines<T>(this IEnumerable<T> items, TraceSource ts, Func<T, string> toString)
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

        #endregion
    }
}
