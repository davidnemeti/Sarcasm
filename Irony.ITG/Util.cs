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
                Trace.WriteLine(item, category);
                yield return item;
            }
        }

#if DEBUG
        public static IEnumerable<T> DebugWriteLines<T>(this IEnumerable<T> items, string category = null)
        {
            foreach (T item in items)
            {
                Debug.WriteLine(item, category);
                yield return item;
            }
        }
#endif
    }
}
