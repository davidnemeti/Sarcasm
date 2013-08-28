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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Parsing;
using Sarcasm.DomainCore;
using Sarcasm.Unparsing;
using Sarcasm.Utility;

namespace Sarcasm.GrammarAst
{
    public class NumberLiteralInfo
    {
        private Dictionary<NumberLiteralBase, string> baseToPrefix = new Dictionary<NumberLiteralBase, string>();
        private Dictionary<string, NumberLiteralBase> prefixToBase = new Dictionary<string, NumberLiteralBase>();
        private Dictionary<TypeCode, string> typeCodeToSuffix = new Dictionary<TypeCode, string>();
        private Dictionary<string, TypeCode> suffixToTypeCode = new Dictionary<string, TypeCode>();
        private NumberLiteral numberLiteral;
        private const NumberLiteralBase defaultNumberLiteralBase = NumberLiteralBase.Decimal;   // default base in Irony is 10 and it cannot be changed

        private List<string> _sortedPrefixesByDescLength = null;
        private List<string> sortedPrefixesByDescLength
        {
            get { return _sortedPrefixesByDescLength ?? (_sortedPrefixesByDescLength = prefixToBase.Keys.OrderByDescending(prefix => prefix.Length).ToList()); }
        }

        private void ClearSortedPrefixesByDescLength()
        {
            _sortedPrefixesByDescLength = null;
        }

        private List<string> _sortedSuffixesByDescLength = null;
        private List<string> sortedSuffixesByDescLength
        {
            get { return _sortedSuffixesByDescLength ?? (_sortedSuffixesByDescLength = suffixToTypeCode.Keys.OrderByDescending(suffix => suffix.Length).ToList()); }
        }

        private void ClearSortedSuffixesByDescLength()
        {
            _sortedSuffixesByDescLength = null;
        }

        private bool _caseSensitivePrefixesSuffixes = false;
        public bool CaseSensitivePrefixesSuffixes
        {
            get { return NumberLiteral != null ? (_caseSensitivePrefixesSuffixes = NumberLiteral.CaseSensitivePrefixesSuffixes) : _caseSensitivePrefixesSuffixes; }

            set
            {
                _caseSensitivePrefixesSuffixes = value;

                if (NumberLiteral != null)
                    NumberLiteral.CaseSensitivePrefixesSuffixes = value;
            }
        }

        private StringComparison comparisonType { get { return CaseSensitivePrefixesSuffixes ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase; } }

        public NumberLiteral NumberLiteral
        {
            get { return numberLiteral; }

            internal set
            {
                if (numberLiteral != null)
                    throw new InvalidOperationException("NumberLiteral has been already set on NumberLiteralInfo");

                numberLiteral = value;

                if (value != null)
                {
                    foreach (var prefix_base in prefixToBase)
                        value.AddPrefix(prefix_base.Key, NumberLiteralBaseToNumberOptions(prefix_base.Value));

                    foreach (var suffix_typeCode in suffixToTypeCode)
                        value.AddSuffix(suffix_typeCode.Key, suffix_typeCode.Value);
                }

                numberLiteral.CaseSensitivePrefixesSuffixes = this._caseSensitivePrefixesSuffixes;  // NOTE: we are getting the _caseSensitivePrefixesSuffixes field here intentionally
            }
        }

        public NumberLiteralInfo()
        {
        }

        public NumberLiteralInfo(NumberLiteral numberLiteral)
        {
            this.numberLiteral = numberLiteral;     // NOTE: we are setting the numberLiteral field here intentionally
        }

        public NumberLiteralInfo AddPrefix(string prefix, NumberLiteralBase @base)
        {
            if (NumberLiteral != null)
                NumberLiteral.AddPrefix(prefix, NumberLiteralBaseToNumberOptions(@base));

            prefixToBase.Add(prefix, @base);

            // the same base can belong to different prefixes -> we store the first prefix into the baseToPrefix dictionary
            if (!baseToPrefix.ContainsKey(@base))
                baseToPrefix.Add(@base, prefix);

            ClearSortedPrefixesByDescLength();

            return this;
        }

        private static NumberOptions NumberLiteralBaseToNumberOptions(NumberLiteralBase @base)
        {
            switch (@base)
            {
                case NumberLiteralBase.Decimal:
                    return NumberOptions.Default;

                case NumberLiteralBase.Hexadecimal:
                    return NumberOptions.Hex;

                case NumberLiteralBase.Octal:
                    return NumberOptions.Octal;

                case NumberLiteralBase.Binary:
                    return NumberOptions.Binary;

                default:
                    throw new ArgumentException("Unknown numberLiteralBase: " + @base, "@base");
            }
        }

        public NumberLiteralInfo AddSuffix(string suffix, TypeCode typeCode)
        {
            if (NumberLiteral != null)
                NumberLiteral.AddSuffix(suffix, typeCode);

            suffixToTypeCode.Add(suffix, typeCode);

            // the same typeCode can belong to different suffixes -> we store the first suffix into the typeCodeToSuffix dictionary
            if (!typeCodeToSuffix.ContainsKey(typeCode))
                typeCodeToSuffix.Add(typeCode, suffix);

            ClearSortedSuffixesByDescLength();

            return this;
        }

        public NumberLiteralBase PrefixToBase(string prefix)
        {
            return prefixToBase[prefix];
        }

        public string BaseToPrefix(NumberLiteralBase @base)
        {
            string prefix;

            if (baseToPrefix.TryGetValue(@base, out prefix))
                return prefix;
            else if (@base == defaultNumberLiteralBase)
                return string.Empty;
            else
                throw new UnparseException(string.Format("NumberLiteral error: no prefix for non-default base '{0}'", @base));
        }

        public NumberLiteralBase GetBaseFromTokenText(string tokenText)
        {
            foreach (string prefix in sortedPrefixesByDescLength)
            {
                if (prefix.Length < tokenText.Length && tokenText.StartsWith(prefix, comparisonType))
                    return prefixToBase[prefix];
            }

            return defaultNumberLiteralBase;
        }

        public bool HasTokenTextExplicitTypeSuffix(string tokenText)
        {
            return sortedSuffixesByDescLength.Any(suffix => suffix.Length < tokenText.Length && tokenText.EndsWith(suffix, comparisonType));
        }

        public string TypeToSuffix(INumberLiteral numberLiteral)
        {
            if (!numberLiteral.HasExplicitTypeModifier)
                return string.Empty;

            else
            {
                Type type = numberLiteral.Value.GetType();
                TypeCode typeCode = Type.GetTypeCode(type);

                try
                {
                    return typeCodeToSuffix[typeCode];
                }
                catch (KeyNotFoundException)
                {
                    throw new UnparseException(string.Format("Type suffix not found for type: {0}", typeCode));
                }
            }
        }

        public string NumberLiteralToText(INumberLiteral numberLiteral, IFormatProvider formatProvider)
        {
            string body = NumberToText(numberLiteral.Value, (int)numberLiteral.Base, formatProvider, numberLiteral.HasExplicitTypeModifier);
            return BaseToPrefix(numberLiteral.Base) + body + TypeToSuffix(numberLiteral);
        }

        public static string NumberToText(object number, int @base, IFormatProvider formatProvider, bool hasExplicitTypeModifier = false)
        {
            if (@base == 10)
            {
                return IsExplicitDecimalSeparatorNeeded(number, hasExplicitTypeModifier)
                    ? string.Format(formatProvider, "{0:0.0}", number)
                    : Convert.ToString(number, formatProvider);
            }

            else if (number is byte)
                return Convert.ToString((byte)number, @base);

            else if (number is short)
                return Convert.ToString((short)number, @base);

            else if (number is int)
                return Convert.ToString((int)number, @base);

            else if (number is long)
                return Convert.ToString((long)number, @base);

            else if (number is sbyte)
                return Convert.ToString((sbyte)number, @base);

            else if (number is ushort)
                return Convert.ToString((ushort)number, @base);

            else if (number is uint)
                return Convert.ToString((uint)number, @base);

            else if (number is ulong && ((ulong)number) <= long.MaxValue)
                return Convert.ToString((long)(ulong)number, @base);

            else
                throw new UnparseException(string.Format("Cannot unparse number {0} with type '{1}' and base {2}", number, number.GetType().FullName, @base));
        }

        public static bool IsNumber(object number)
        {
            return
                number is byte || number is short || number is int || number is long ||
                number is sbyte || number is ushort || number is uint || number is ulong ||
                number is float || number is double || number is decimal;
        }

        private static bool IsExplicitDecimalSeparatorNeeded(object number, bool hasExplicitTypeModifier)
        {
            return
                (number is float || number is double || number is decimal)
                &&
                object.Equals(Round(number), number)
                &&
                !hasExplicitTypeModifier;
        }

        private static object Round(object number)
        {
            if (number is float)
                return Math.Round((double)(float)number);

            else if (number is double)
                return Math.Round((double)number);

            else if (number is decimal)
                return Math.Round((decimal)number);

            else
                return number;
        }
    }

    public static class NumberLiteralInfoExtensions
    {
        public static NumberLiteral AttachNumberLiteralInfo(this NumberLiteral numberLiteral, NumberLiteralInfo numberLiteralInfo)
        {
            numberLiteralInfo.NumberLiteral = numberLiteral;
            return numberLiteral;
        }
    }
}
