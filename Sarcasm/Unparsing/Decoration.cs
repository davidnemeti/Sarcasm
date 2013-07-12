using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasm.Unparsing
{
    public interface IReadOnlyDecoration
    {
        bool ContainsKey(object key);
        bool TryGetValue(object key, out object value);
    }

    public interface IDecoration : IReadOnlyDecoration
    {
        IDecoration Add(object key, object value);
    }

    public enum DecorationKey
    {
        Font,
        FontFamily,
        FontStretch,
        FontStyle,
        FontWeight,
        FontSize,
        FontSizeRelativePercent,
        TextDecorations,
        BaselineAlignment,
        Foreground,
        Background
    }

    public static class DecorationExtensions
    {
        public static bool ContainsKey<T>(this IReadOnlyDecoration decoration)
        {
            return decoration.ContainsKey(typeof(T));
        }

        public static T GetValueOrDefault<T>(this IReadOnlyDecoration decoration)
        {
            return GetValueOrDefault<T>(decoration, typeof(T));
        }

        public static TValue GetValueOrDefault<TValue>(this IReadOnlyDecoration decoration, object key)
        {
            TValue value;

            if (decoration.TryGetValue<TValue>(key, out value))
                return value;
            else
                return default(TValue);
        }

        public static object GetValueOrNull(this IReadOnlyDecoration decoration, object key)
        {
            return decoration.GetValueOrDefault<object>(key);
        }

        public static T GetValue<T>(this IReadOnlyDecoration decoration)
        {
            return GetValue<T>(decoration, typeof(T));
        }

        public static TValue GetValue<TValue>(this IReadOnlyDecoration decoration, object key)
        {
            TValue value;

            if (decoration.TryGetValue(key, out value))
                return value;
            else
                throw new KeyNotFoundException();
        }

        public static object GetValue(this IReadOnlyDecoration decoration, object key)
        {
            return decoration.GetValue<object>(key);
        }

        public static bool TryGetValue<T>(this IReadOnlyDecoration decoration, out T value)
        {
            return decoration.TryGetValue<T>(typeof(T), out value);
        }

        public static bool TryGetValue<TValue>(this IReadOnlyDecoration decoration, object key, out TValue value)
        {
            object _value;
            bool success = decoration.TryGetValue(key, out _value);
            value = _value != null ? (TValue)_value : default(TValue);
            return success;
        }

        public static IDecoration Add<T>(this IDecoration decoration, T value)
        {
            return decoration.Add(typeof(T), value);
        }
    }

    public class Decoration : IDecoration
    {
        private Dictionary<object, object> keyToValue = new Dictionary<object, object>();

        public IDecoration Add(object key, object value)
        {
            keyToValue.Add(key, value);
            return this;
        }

        public static readonly IDecoration None = null;

        public bool TryGetValue(object key, out object value)
        {
            return keyToValue.TryGetValue(key, out value);
        }

        public bool ContainsKey(object key)
        {
            return keyToValue.ContainsKey(key);
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

        public bool TryGetValue(object key, out object value)
        {
            return primaryDecoration.TryGetValue(key, out value) || secondaryDecoration.TryGetValue(key, out value);
        }

        public bool ContainsKey(object key)
        {
            return primaryDecoration.ContainsKey(key) || secondaryDecoration.ContainsKey(key);
        }
    }
}
