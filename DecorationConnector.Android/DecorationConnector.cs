using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;

using Graphics = Android.Graphics;
using Styles = Sarcasm.Unparsing.Styles;
using Unparsing = Sarcasm.Unparsing;

namespace DecorationConnector.Android
{
    public class DecorConnector : IReadOnlyDecoration
    {
        private readonly IReadOnlyDecoration decoration;

        public DecorConnector(IReadOnlyDecoration decoration)
        {
            this.decoration = decoration;
        }

        public bool ContainsKey(object key)
        {
            object _;
            return TryGetValueTypeless(key, out _);
        }

        public bool TryGetValueTypeless(object key, out object value)
        {
            bool found;

            if (key == DecorKeyConnector.Typeface)
            {
                Graphics.Typeface typefaceFamily;
                if (this.TryGetValue(DecorKeyConnector.TypefaceFamily, out typefaceFamily))
                {
                    Graphics.TypefaceStyle typefaceStyle = this.GetValueOrDefault(DecorKeyConnector.TypefaceStyle);
                    Graphics.Typeface typeface = Graphics.Typeface.Create(typefaceFamily, typefaceStyle);
                    value = typeface;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            else if (key == DecorKeyConnector.TypefaceFamily)
                value = decoration.GetValueConnectedTry(DecorationKey.FontFamily, TransformFontFamilyToTypefaceFamily, out found);

            else if (key == DecorKeyConnector.TypefaceStyle)
            {
                Graphics.TypefaceStyle fontStyle;
                bool foundStyle = decoration.TryGetValueConnected(DecorationKey.FontStyle, TransformFontStyle, out fontStyle);

                Graphics.TypefaceStyle fontWeight;
                bool foundWeight = decoration.TryGetValueConnected(DecorationKey.FontWeight, TransformFontWeight, out fontWeight);

                found = foundStyle | foundWeight;
                value = fontStyle | fontWeight;

                value = decoration.GetValueConnectedTry(DecorationKey.FontFamily, TransformFontFamilyToTypefaceFamily, out found);
            }

            else if (key == DecorKeyConnector.FontSize)
                value = decoration.GetValueConnectedTry(DecorationKey.FontSize, DecorationExtensions.IdentityTransformation, out found);

            else if (key == DecorKeyConnector.FontSizeRelativePercent)
                value = decoration.GetValueConnectedTry(DecorationKey.FontSizeRelativePercent, DecorationExtensions.IdentityTransformation, out found);

            else if (key == DecorKeyConnector.Foreground)
                value = decoration.GetValueConnectedTry(DecorationKey.Foreground, TransformColor, out found);

            else if (key == DecorKeyConnector.Background)
                value = decoration.GetValueConnectedTry(DecorationKey.Background, TransformColor, out found);

            else
            {
                found = false;
                value = null;
            }

            return found;
        }

        private Graphics.Typeface TransformFontFamilyToTypeface(Styles.FontFamily fontFamily)
        {
            if (fontFamily == Styles.FontFamily.GenericMonospace)
                return Graphics.Typeface.Monospace;

            else if (fontFamily == Styles.FontFamily.GenericSansSerif)
                return Graphics.Typeface.SansSerif;

            else if (fontFamily == Styles.FontFamily.GenericSerif)
                return Graphics.Typeface.Serif;

            else
                return Graphics.Typeface.Create(fontFamily.Name, Graphics.TypefaceStyle.Normal);
        }

        private Graphics.Typeface TransformFontFamilyToTypefaceFamily(Styles.FontFamily fontFamily)
        {
            if (fontFamily == Styles.FontFamily.GenericMonospace)
                return Graphics.Typeface.Monospace;

            else if (fontFamily == Styles.FontFamily.GenericSansSerif)
                return Graphics.Typeface.SansSerif;

            else if (fontFamily == Styles.FontFamily.GenericSerif)
                return Graphics.Typeface.Serif;

            else
                return Graphics.Typeface.Create(fontFamily.Name, Graphics.TypefaceStyle.Normal);
        }

        private Graphics.TypefaceStyle TransformFontStyle(Styles.FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case Styles.FontStyle.Normal:
                    return Graphics.TypefaceStyle.Normal;

                case Styles.FontStyle.Italic:
                    return Graphics.TypefaceStyle.Italic;

                default:
                    throw new ArgumentException("fontStyle");
            }
        }

        private Graphics.TypefaceStyle TransformFontWeight(Styles.FontWeight fontWeight)
        {
            switch (fontWeight)
            {
                case Styles.FontWeight.Normal:
                    return Graphics.TypefaceStyle.Normal;

                case Styles.FontWeight.Bold:
                    return Graphics.TypefaceStyle.Bold;

                case Styles.FontWeight.Thin:
                    return Graphics.TypefaceStyle.Normal;       // no pair for this -> return the default style

                default:
                    throw new ArgumentException("fontWeight");
            }
        }

        private Graphics.Color TransformColor(Styles.Color color)
        {
            return Graphics.Color.Argb(color.A(), color.R(), color.G(), color.B());
        }
    }

    public static class DecorConnectorExtensions
    {
        public static DecorConnector ToAndroid(this IReadOnlyDecoration decoration)
        {
            return new DecorConnector(decoration);
        }
    }

    public static class DecorKeyConnector
    {
        public static DecorationKey<Graphics.Typeface> Typeface { get; private set; }
        public static DecorationKey<Graphics.Typeface> TypefaceFamily { get; private set; }
        public static DecorationKey<Graphics.TypefaceStyle> TypefaceStyle { get; private set; }

        public static DecorationKey<double> FontSize { get; private set; }
        public static DecorationKey<double> FontSizeRelativePercent { get; private set; }

        public static DecorationKey<Graphics.Color> Foreground { get; private set; }
        public static DecorationKey<Graphics.Color> Background { get; private set; }

        static DecorKeyConnector()
        {
            Typeface = new DecorationKey<Graphics.Typeface>("Typeface");
            TypefaceFamily = new DecorationKey<Graphics.Typeface>("TypefaceFamily");
            TypefaceStyle = new DecorationKey<Graphics.TypefaceStyle>("TypefaceStyle");

            FontSize = new DecorationKey<double>("FontSize");
            FontSizeRelativePercent = new DecorationKey<double>("FontSizeRelativePercent");

            Foreground = new DecorationKey<Graphics.Color>("Foreground");
            Background = new DecorationKey<Graphics.Color>("Background");
        }
    }
}
