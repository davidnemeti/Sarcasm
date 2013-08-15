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
using System.Threading.Tasks;

using Sarcasm.Unparsing.Styles;

namespace Sarcasm.Unparsing
{
    public interface IReadOnlyDecoration
    {
        bool ContainsKey(object key);
        bool TryGetValueTypeless(object key, out object value);
    }

    public interface IDecoration : IReadOnlyDecoration
    {
        bool Remove(object key);
        IDecoration AddTypeless(object key, object value);
    }

    public interface IDecorationKey<T>
    {
        string Name { get; }
    }

    public class DecorationKey<T> : IDecorationKey<T>
    {
        public string Name { get; private set; }

        public DecorationKey()
            : this(name: null)
        {
        }

        public DecorationKey(string name)
        {
            this.Name = name;
        }
    }

    public static class DecorationKey
    {
        static DecorationKey()
        {
            FontFamily = new DecorationKey<FontFamily>("FontFamily");
            FontStretch = new DecorationKey<FontStretch>("FontStretch");
            FontStyle = new DecorationKey<FontStyle>("FontStyle");
            FontWeight = new DecorationKey<FontWeight>("FontWeight");

            FontSize = new DecorationKey<double>("FontSize");
            FontSizeRelativePercent = new DecorationKey<double>("FontSizeRelativePercent");

            TextDecoration = new DecorationKey<TextDecoration>("TextDecorations");
            BaselineAlignment = new DecorationKey<BaselineAlignment>("BaselineAlignment");

            Foreground = new DecorationKey<Color>("Foreground");
            Background = new DecorationKey<Color>("Background");
        }

        public static DecorationKey<FontFamily> FontFamily { get; private set; }
        public static DecorationKey<FontStretch> FontStretch { get; private set; }
        public static DecorationKey<FontStyle> FontStyle { get; private set; }
        public static DecorationKey<FontWeight> FontWeight { get; private set; }

        public static DecorationKey<double> FontSize { get; private set; }
        public static DecorationKey<double> FontSizeRelativePercent { get; private set; }

        public static DecorationKey<TextDecoration> TextDecoration { get; private set; }
        public static DecorationKey<BaselineAlignment> BaselineAlignment { get; private set; }

        public static DecorationKey<Color> Foreground { get; private set; }
        public static DecorationKey<Color> Background { get; private set; }
    }

    public static class DecorationExtensions
    {
        public static IDecoration Add<T>(this IDecoration decoration, IDecorationKey<T> key, T value)
        {
            return decoration.AddTypeless(key, value);
        }

        public static IDecoration AddByType<T>(this IDecoration decoration, T value)
        {
            return decoration.AddTypeless(typeof(T), value);
        }

        public static bool RemoveByType<T>(this IDecoration decoration)
        {
            return decoration.Remove(typeof(T));
        }

        public static T GetValueOrDefault<T>(this IReadOnlyDecoration decoration, IDecorationKey<T> key)
        {
            T value;

            if (decoration.TryGetValue(key, out value))
                return value;
            else
                return default(T);
        }

        public static T GetValue<T>(this IReadOnlyDecoration decoration, IDecorationKey<T> key)
        {
            T value;

            if (decoration.TryGetValue(key, out value))
                return value;
            else
                throw new KeyNotFoundException();
        }

        public static bool TryGetValue<T>(this IReadOnlyDecoration decoration, IDecorationKey<T> key, out T value)
        {
            object _value;
            bool found = decoration.TryGetValueTypeless(key, out _value);
            value = found ? (T)_value : default(T);
            return found;
        }

        public static T GetValueOrDefaultByType<T>(this IReadOnlyDecoration decoration)
        {
            T value;

            if (decoration.TryGetValueByType<T>(out value))
                return value;
            else
                return default(T);
        }

        public static T GetValueByType<T>(this IReadOnlyDecoration decoration)
        {
            T value;

            if (decoration.TryGetValueByType<T>(out value))
                return value;
            else
                throw new KeyNotFoundException();
        }

        public static bool ContainsKeyByType<T>(this IReadOnlyDecoration decoration)
        {
            return decoration.ContainsKey(typeof(T));
        }

        public static bool TryGetValueTypelessByType<T>(this IReadOnlyDecoration decoration, out object value)
        {
            return decoration.TryGetValueTypeless(typeof(T), out value);
        }

        public static bool TryGetValueByType<T>(this IReadOnlyDecoration decoration, out T value)
        {
            object _value;
            bool found = decoration.TryGetValueTypelessByType<T>(out _value);
            value = found ? (T)_value : default(T);
            return found;
        }

        public static bool TryGetValueConnected<TSource, TTarget>(this IReadOnlyDecoration sourceDecoration, DecorationKey<TSource> sourceDecorationKey,
            Func<TSource, TTarget> transformer, out TTarget targetValue)
        {
            TSource sourceValue;
            bool found = sourceDecoration.TryGetValue(sourceDecorationKey, out sourceValue);
            targetValue = found ? transformer(sourceValue) : default(TTarget);
            return found;
        }

        public static TTarget GetValueConnectedTry<TSource, TTarget>(this IReadOnlyDecoration sourceDecoration, DecorationKey<TSource> sourceDecorationKey,
            Func<TSource, TTarget> transformer, out bool found)
        {
            TTarget targetValue;
            found = sourceDecoration.TryGetValueConnected(sourceDecorationKey, transformer, out targetValue);
            return targetValue;
        }

        public static T IdentityTransformation<T>(T value)
        {
            return value;
        }

        public static IReadOnlyDecoration Compose(this IReadOnlyDecoration primaryDecoration, IReadOnlyDecoration secondaryDecoration)
        {
            return new DecorationComposer(primaryDecoration, secondaryDecoration);
        }
    }

    public class Decoration : IDecoration
    {
        public static IDecoration None { get { return new Decoration(); } }

        private Dictionary<object, object> keyToValue = new Dictionary<object, object>();

        public IDecoration AddTypeless(object key, object value)
        {
            keyToValue.Add(key, value);
            return this;
        }

        public bool Remove(object key)
        {
            return keyToValue.Remove(key);
        }

        public bool ContainsKey(object key)
        {
            return keyToValue.ContainsKey(key);
        }

        public bool TryGetValueTypeless(object key, out object value)
        {
            return keyToValue.TryGetValue(key, out value);
        }
    }

    public class DecorationComposer : IReadOnlyDecoration
    {
        private readonly IReadOnlyDecoration primaryDecoration;
        private readonly IReadOnlyDecoration secondaryDecoration;

        public DecorationComposer(IReadOnlyDecoration primaryDecoration, IReadOnlyDecoration secondaryDecoration)
        {
            this.primaryDecoration = primaryDecoration;
            this.secondaryDecoration = secondaryDecoration;
        }

        public bool ContainsKey(object key)
        {
            return primaryDecoration.ContainsKey(key) || secondaryDecoration.ContainsKey(key);
        }

        public bool TryGetValueTypeless(object key, out object value)
        {
            return primaryDecoration.TryGetValueTypeless(key, out value) || secondaryDecoration.TryGetValueTypeless(key, out value);
        }
    }
}
