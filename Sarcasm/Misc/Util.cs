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
        #region Extreme item methods (min/max)

        public static TSource MaxItem<TSource>(this IEnumerable<TSource> items)
        {
            TSource maxValueUnused;
            return items.ExtremeItem(item => item, Comparer<TSource>.Default, max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource>(this IEnumerable<TSource> items)
        {
            TSource minValueUnused;
            return items.ExtremeItem(item => item, Comparer<TSource>.Default, max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource>(this IEnumerable<TSource> items, Comparison<TSource> comparison)
        {
            TSource maxValueUnused;
            return items.ExtremeItem(item => item, Comparer<TSource>.Create(comparison), max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource>(this IEnumerable<TSource> items, Comparison<TSource> comparison)
        {
            TSource minValueUnused;
            return items.ExtremeItem(item => item, Comparer<TSource>.Create(comparison), max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource>(this IEnumerable<TSource> items, IComparer<TSource> comparer)
        {
            TSource maxValueUnused;
            return items.ExtremeItem(item => item, comparer, max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource>(this IEnumerable<TSource> items, IComparer<TSource> comparer)
        {
            TSource minValueUnused;
            return items.ExtremeItem(item => item, comparer, max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector)
        {
            TValue maxValueUnused;
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Default, max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector)
        {
            TValue minValueUnused;
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Default, max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, Comparison<TValue> comparison)
        {
            TValue maxValueUnused;
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Create(comparison), max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, Comparison<TValue> comparison)
        {
            TValue minValueUnused;
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Create(comparison), max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, IComparer<TValue> comparer)
        {
            TValue maxValueUnused;
            return items.ExtremeItem(valueSelector, comparer, max: true, extremeValue: out maxValueUnused);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, IComparer<TValue> comparer)
        {
            TValue minValueUnused;
            return items.ExtremeItem(valueSelector, comparer, max: false, extremeValue: out minValueUnused);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, out TValue maxValue)
        {
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Default, max: true, extremeValue: out maxValue);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, out TValue minValue)
        {
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Default, max: false, extremeValue: out minValue);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, Comparison<TValue> comparison, out TValue maxValue)
        {
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Create(comparison), max: true, extremeValue: out maxValue);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, Comparison<TValue> comparison, out TValue minValue)
        {
            return items.ExtremeItem(valueSelector, Comparer<TValue>.Create(comparison), max: false, extremeValue: out minValue);
        }

        public static TSource MaxItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, IComparer<TValue> comparer, out TValue maxValue)
        {
            return items.ExtremeItem(valueSelector, comparer, max: true, extremeValue: out maxValue);
        }

        public static TSource MinItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, IComparer<TValue> comparer, out TValue minValue)
        {
            return items.ExtremeItem(valueSelector, comparer, max: false, extremeValue: out minValue);
        }

        private static TSource ExtremeItem<TSource, TValue>(this IEnumerable<TSource> items, Func<TSource, TValue> valueSelector, IComparer<TValue> comparer, bool max, out TValue extremeValue)
        {
            bool beyondFirst = false;
            TSource extremeItem = default(TSource);
            extremeValue = default(TValue);

            foreach (TSource item in items)
            {
                if (!beyondFirst)
                {
                    extremeItem = item;
                    extremeValue = valueSelector(item);
                    beyondFirst = true;
                }
                else
                {
                    TValue value = valueSelector(item);

                    int compareResult = comparer.Compare(value, extremeValue);

                    if (!max)
                        compareResult = -compareResult;

                    if (compareResult > 0)
                    {
                        extremeItem = item;
                        extremeValue = valueSelector(item);
                    }
                }
            }

            if (beyondFirst)
                return extremeItem;
            else
                throw new InvalidOperationException("Empty list");
        }

        #endregion

        public static IEnumerable<T> ReverseOptimized<T>(this IEnumerable<T> items)
        {
            if (items is IList<T>)
                return ReverseOptimized((IList<T>)items);
            else
                return items.Reverse();
        }

        public static IList<T> ReverseOptimized<T>(this IList<T> items)
        {
            return new ReverseList<T>(items);
        }

        public static System.Collections.IEnumerable ReverseNonGenericOptimized(this System.Collections.IEnumerable items)
        {
            if (items is System.Collections.IList)
                return ReverseNonGenericOptimized((System.Collections.IList)items);
            else
                return items.ReverseNonGeneric();
        }

        public static System.Collections.IEnumerable ReverseNonGeneric(this System.Collections.IEnumerable items)
        {
            var itemsFullyRead = new System.Collections.ArrayList();
            foreach (var item in items)
                itemsFullyRead.Add(item);
            return ReverseNonGenericOptimized(itemsFullyRead);
        }

        public static System.Collections.IEnumerable ReverseNonGenericOptimized(this System.Collections.IList items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
                yield return items[i];
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> items, T last)
        {
            foreach (T item in items)
                yield return item;

            yield return last;
        }

        public static IEnumerable<T> Concat<T>(T first, IEnumerable<T> items)
        {
            yield return first;

            foreach (T item in items)
                yield return item;
        }

        public static T SingleOrDefaultNoException<T>(this IEnumerable<T> items)
        {
            using (var enumerator = items.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    T firstItem = enumerator.Current;

                    return enumerator.MoveNext()
                        ? default(T)
                        : firstItem;
                }
                else
                    return default(T);
            }
        }

        public static IEnumerable<Tuple<TA, TB>> GetCartesianProduct<TA, TB>(IEnumerable<TA> items1, IEnumerable<TB> items2)
        {
            foreach (var item1 in items1)
                foreach (var item2 in items2)
                    yield return Tuple.Create(item1, item2);
        }

        #region Recurse

        public static IEnumerable<T> Recurse<T>(T init, Func<T, T> func)
        {
            return RecurseStopBefore(init, func, value => false);
        }

        public static IEnumerable<T> RecurseStopAt<T>(T init, Func<T, T> func, Predicate<T> fStopAt)
        {
            T value = init;
            while (true)
            {
                yield return value;
                if (fStopAt(value)) break;
                value = func(value);
            }
        }

        public static IEnumerable<T> RecurseStopBefore<T>(T init, Func<T, T> func, Predicate<T> fStopBefore)
        {
            T value = init;
            while (true)
            {
                if (fStopBefore(value)) break;
                yield return value;
                value = func(value);
            }
        }

        public static IEnumerable<T> RecurseStopBeforeNull<T>(T init, Func<T, T> func)
            where T : class
        {
            return RecurseStopBefore(init, func, value => value == null);
        }

        #endregion

        public static int GetHashCodeMulti(params object[] items)
        {
            return items.GetHashCodeMulti();
        }

        public static int GetHashCodeMulti(this System.Collections.IEnumerable items)
        {
            return items.Cast<object>().Aggregate(17, (hashCode, current) => hashCode * 23 + (current != null ? current.GetHashCode() : 0));
        }

        public static string ToString(IFormatProvider formatProvider, object obj)
        {
            return string.Format(formatProvider, "{0}", obj);
        }

        public static int? SumIncludingNullValues<TSource>(this IEnumerable<TSource> items, Func<TSource, int?> selector)
        {
            return items.Select(selector).SumIncludingNullValues();
        }

        public static int? SumIncludingNullValues<TSource>(this IEnumerable<TSource> items, Func<TSource, int, int?> selector)
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

        public static bool EqualToAny<T>(this T value, T candidateValue1, T candidateValue2, T candidateValue3, T candidateValue4)
        {
            return EqualityComparer<T>.Default.Equals(value, candidateValue1)
                || EqualityComparer<T>.Default.Equals(value, candidateValue2)
                || EqualityComparer<T>.Default.Equals(value, candidateValue3)
                || EqualityComparer<T>.Default.Equals(value, candidateValue4);
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
            ts.TraceEvent(traceEventType, 0, obj != null ? obj.ToString() : "<<NULL>>");
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
            ts.TraceEvent(TraceEventType.Verbose, 0, obj != null ? obj.ToString() : "<<NULL>>");
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

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, bool condition, object obj)
        {
            if (condition)
                Debug(ts, obj);
        }

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, bool condition, string message)
        {
            if (condition)
                Debug(ts, message);
        }

        [Conditional("DEBUG")]
        public static void Debug(this TraceSource ts, bool condition, string format, params object[] args)
        {
            if (condition)
                Debug(ts, format, args);
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
