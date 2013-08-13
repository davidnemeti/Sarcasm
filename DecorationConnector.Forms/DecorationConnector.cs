using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;

using Drawing = System.Drawing;
using Windows = System.Windows;
using Styles = Sarcasm.Unparsing.Styles;
using Unparsing = Sarcasm.Unparsing;

namespace DecorationConnector.Forms
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

            if (key == DecorKeyConnector.FontFamily)
                value = decoration.GetValueConnectedTry(DecorationKey.FontFamily, TransformFontFamily, out found);

            else if (key == DecorKeyConnector.FontStyle)
            {
                Drawing.FontStyle fontStyle;
                bool foundStyle = decoration.TryGetValueConnected(DecorationKey.FontStyle, TransformFontStyle, out fontStyle);

                Drawing.FontStyle fontWeight;
                bool foundWeight = decoration.TryGetValueConnected(DecorationKey.FontWeight, TransformFontWeight, out fontWeight);

                Drawing.FontStyle textDecoration;
                bool foundTextDecoration = decoration.TryGetValueConnected(DecorationKey.TextDecoration, TransformTextDecoration, out textDecoration);

                found = foundStyle | foundWeight | foundTextDecoration;
                value = fontStyle | fontWeight | textDecoration;
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

        private Drawing.FontFamily TransformFontFamily(Styles.FontFamily fontFamily)
        {
            if (fontFamily == Styles.FontFamily.GenericMonospace)
                return Drawing.FontFamily.GenericMonospace;

            else if (fontFamily == Styles.FontFamily.GenericSansSerif)
                return Drawing.FontFamily.GenericSansSerif;

            else if (fontFamily == Styles.FontFamily.GenericSerif)
                return Drawing.FontFamily.GenericSerif;

            else
                return new Drawing.FontFamily(fontFamily.Name);
        }

        private Drawing.FontStyle TransformFontStyle(Styles.FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case Styles.FontStyle.Normal:
                    return Drawing.FontStyle.Regular;

                case Styles.FontStyle.Italic:
                    return Drawing.FontStyle.Italic;

                default:
                    throw new ArgumentException("fontStyle");
            }
        }

        private Drawing.FontStyle TransformFontWeight(Styles.FontWeight fontWeight)
        {
            switch (fontWeight)
            {
                case Styles.FontWeight.Normal:
                    return Drawing.FontStyle.Regular;

                case Styles.FontWeight.Bold:
                    return Drawing.FontStyle.Bold;

                case Styles.FontWeight.Thin:
                    return Drawing.FontStyle.Regular;       // no pair for this -> return the default style

                default:
                    throw new ArgumentException("fontWeight");
            }
        }

        private Drawing.FontStyle TransformTextDecoration(Styles.TextDecoration textDecoration)
        {
            if (textDecoration == TextDecoration.Baseline)
                return Drawing.FontStyle.Regular;    // no pair for this -> return the default style

            else if (textDecoration == TextDecoration.OverLine)
                return Drawing.FontStyle.Regular;    // no pair for this -> return the default style

            else if (textDecoration == TextDecoration.Strikethrough)
                return Drawing.FontStyle.Strikeout;

            else if (textDecoration == TextDecoration.Underline)
                return Drawing.FontStyle.Underline;

            else
            {
                switch (textDecoration.Location)
                {
                    case Styles.TextDecorationLocation.Baseline:
                        return Drawing.FontStyle.Regular;    // no pair for this -> return the default style

                    case Styles.TextDecorationLocation.OverLine:
                        return Drawing.FontStyle.Regular;    // no pair for this -> return the default style

                    case Styles.TextDecorationLocation.Strikethrough:
                        return Drawing.FontStyle.Strikeout;

                    case Styles.TextDecorationLocation.Underline:
                        return Drawing.FontStyle.Underline;

                    default:
                        throw new ArgumentException("textDecoration");
                }
            }
        }

        private Drawing.Color TransformColor(Styles.Color color)
        {
            return Drawing.Color.FromArgb(color.A(), color.R(), color.G(), color.B());
        }
    }

    public static class DecorConnectorExtensions
    {
        public static DecorConnector ToForms(this IReadOnlyDecoration decoration)
        {
            return new DecorConnector(decoration);
        }
    }

    public static class DecorKeyConnector
    {
        /*
         * The following decorations has no pairs in Windows Forms:
         * 
         * Styles.FontStretch
         * Styles.BaselineAlignment
         * */

        public static DecorationKey<Drawing.FontFamily> FontFamily { get; private set; }
        public static DecorationKey<Drawing.FontStyle> FontStyle { get; private set; }

        public static DecorationKey<double> FontSize { get; private set; }
        public static DecorationKey<double> FontSizeRelativePercent { get; private set; }

        public static DecorationKey<Drawing.Color> Foreground { get; private set; }
        public static DecorationKey<Drawing.Color> Background { get; private set; }

        static DecorKeyConnector()
        {
            FontFamily = new DecorationKey<Drawing.FontFamily>("FontFamily");
            FontStyle = new DecorationKey<Drawing.FontStyle>("FontStyle");

            FontSize = new DecorationKey<double>("FontSize");
            FontSizeRelativePercent = new DecorationKey<double>("FontSizeRelativePercent");

            Foreground = new DecorationKey<Drawing.Color>("Foreground");
            Background = new DecorationKey<Drawing.Color>("Background");
        }
    }
}
