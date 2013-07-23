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
        bool ContainsKey(IDecorationKey key);
        bool TryGetValueTypeless(IDecorationKey key, out object value);
        bool TryGetValue<T>(IDecorationKey<T> key, out T value);
    }

    public interface IDecoration : IReadOnlyDecoration
    {
        IDecoration Add<T>(IDecorationKey<T> key, T value);
    }

    public interface IDecorationKey
    {
    }

    public interface IDecorationKey<T> : IDecorationKey
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

    public interface IDecorationKeyConnector : IDecorationKey
    {
        IDecorationKey DecorationKeyFrom { get; }
        object Convert(object from);
    }

    public class DecorationKeyConnector<TFrom, TTo> : IDecorationKeyConnector, IDecorationKey<TTo>
    {
        public IDecorationKey<TFrom> DecorationKeyFrom { get; private set; }
        private Func<TFrom, TTo> convert;

        public DecorationKeyConnector(IDecorationKey<TFrom> decorationKeyFrom, Func<TFrom, TTo> convert)
        {
            this.DecorationKeyFrom = decorationKeyFrom;
            this.convert = convert;
        }

        public string Name { get { return DecorationKeyFrom.Name; } }

        object IDecorationKeyConnector.Convert(object from)
        {
            return Convert((TFrom)from);
        }

        public TTo Convert(TFrom from)
        {
            return convert(from);
        }

        IDecorationKey IDecorationKeyConnector.DecorationKeyFrom
        {
            get { return DecorationKeyFrom; }
        }
    }

    public static class DecorationKeyConnector
    {
        public static DecorationKeyConnector<TFrom, TTo> ConnectTo<TFrom, TTo>(IDecorationKey<TFrom> decorationKeyFrom, Func<TFrom, TTo> convert)
        {
            return new DecorationKeyConnector<TFrom, TTo>(decorationKeyFrom, convert);
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
    }

    public class Decoration : IDecoration
    {
        private Dictionary<object, object> keyToValue = new Dictionary<object, object>();

        public IDecoration Add<T>(IDecorationKey<T> key, T value)
        {
            keyToValue.Add(key, value);
            return this;
        }

        public const IDecoration None = null;

        public bool ContainsKey(IDecorationKey key)
        {
            return keyToValue.ContainsKey(key);
        }

        public bool TryGetValue<T>(IDecorationKey<T> key, out T value)
        {
            object _value;
            bool found = TryGetValueTypeless(key, out _value);
            value = (T)_value;
            return found;
        }

        public bool TryGetValueTypeless(IDecorationKey key, out object value)
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

        public bool ContainsKey(IDecorationKey key)
        {
            return primaryDecoration.ContainsKey(key) || secondaryDecoration.ContainsKey(key);
        }

        public bool TryGetValue<T>(IDecorationKey<T> key, out T value)
        {
            return primaryDecoration.TryGetValue(key, out value) || secondaryDecoration.TryGetValue(key, out value);
        }

        public bool TryGetValueTypeless(IDecorationKey key, out object value)
        {
            return primaryDecoration.TryGetValueTypeless(key, out value) || secondaryDecoration.TryGetValueTypeless(key, out value);
        }
    }
}
