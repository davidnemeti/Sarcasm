using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
