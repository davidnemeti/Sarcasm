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
        bool ContainsKey<T>(DecorationKey<T> key);
        bool TryGetValue<T>(DecorationKey<T> key, out T value);
    }

    public interface IDecoration : IReadOnlyDecoration
    {
        IDecoration Add<T>(DecorationKey<T> key, T value);
    }

    public class DecorationKey<T>
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

            TextDecorations = new DecorationKey<IEnumerable<TextDecoration>>("TextDecorations");
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

        public static DecorationKey<IEnumerable<TextDecoration>> TextDecorations { get; private set; }
        public static DecorationKey<BaselineAlignment> BaselineAlignment { get; private set; }

        public static DecorationKey<Color> Foreground { get; private set; }
        public static DecorationKey<Color> Background { get; private set; }
    }

    public static class DecorationExtensions
    {
        public static T GetValueOrDefault<T>(this IReadOnlyDecoration decoration, DecorationKey<T> key)
        {
            T value;

            if (decoration.TryGetValue(key, out value))
                return value;
            else
                return default(T);
        }

        public static T GetValue<T>(this IReadOnlyDecoration decoration, DecorationKey<T> key)
        {
            T value;

            if (decoration.TryGetValue(key, out value))
                return value;
            else
                throw new KeyNotFoundException();
        }
    }

    public class Decoration : IDecoration
    {
        private Dictionary<object, object> keyToValue = new Dictionary<object, object>();

        public IDecoration Add<T>(DecorationKey<T> key, T value)
        {
            keyToValue.Add(key, value);
            return this;
        }

        public const IDecoration None = null;

        public bool TryGetValue<T>(DecorationKey<T> key, out T value)
        {
            object _value;
            bool found = keyToValue.TryGetValue(key, out _value);
            value = (T)_value;
            return found;
        }

        public bool ContainsKey<T>(DecorationKey<T> key)
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

        public bool TryGetValue<T>(DecorationKey<T> key, out T value)
        {
            return primaryDecoration.TryGetValue(key, out value) || secondaryDecoration.TryGetValue(key, out value);
        }

        public bool ContainsKey<T>(DecorationKey<T> key)
        {
            return primaryDecoration.ContainsKey(key) || secondaryDecoration.ContainsKey(key);
        }
    }
}
