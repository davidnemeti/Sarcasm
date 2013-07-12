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
        bool TryGetValue<TValue>(object key, out TValue value);
    }

    public interface IDecoration : IReadOnlyDecoration
    {
        IDecoration Add(object key, object value);
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

        public static T GetValueOrDefault<T>(this IReadOnlyDecoration decoration, object key)
        {
            T value;

            if (decoration.TryGetValue<T>(key, out value))
                return value;
            else
                return default(T);
        }

        public static T GetValue<T>(this IReadOnlyDecoration decoration)
        {
            return GetValue<T>(decoration, typeof(T));
        }

        public static TValue GetValue<TValue>(this IReadOnlyDecoration decoration, object key)
        {
            TValue value;

            if (decoration.TryGetValue<TValue>(key, out value))
                return value;
            else
                throw new KeyNotFoundException();
        }

        public static bool TryGetValue<T>(this IReadOnlyDecoration decoration, out T value)
        {
            return decoration.TryGetValue<T>(typeof(T), out value);
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

        public bool TryGetValue<TValue>(object key, out TValue value)
        {
            object _value;
            bool success = keyToValue.TryGetValue(key, out _value);
            value = (TValue)_value;
            return success;
        }

        public static readonly IDecoration None = null;

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

        public bool TryGetValue<TValue>(object key, out TValue value)
        {
            return primaryDecoration.TryGetValue(key, out value) || secondaryDecoration.TryGetValue(key, out value);
        }

        public bool ContainsKey(object key)
        {
            return primaryDecoration.ContainsKey(key) || secondaryDecoration.ContainsKey(key);
        }
    }
}
