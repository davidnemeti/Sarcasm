using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarcasm.Unparsing;
using Sarcasm.Unparsing.Styles;

using Media = System.Windows.Media;
using Windows = System.Windows;
using Styles = Sarcasm.Unparsing.Styles;
using Unparsing = Sarcasm.Unparsing;

namespace DecorationConnector.WPF
{
    public class DecorConnector : IReadOnlyDecoration
    {
        private readonly IReadOnlyDecoration decoration;

        public DecorConnector(IReadOnlyDecoration decoration)
        {
            this.decoration = decoration;
        }

        public bool ContainsKey(IDecorationKey key)
        {
            object _;
            return TryGetValueTypeless(key, out _);
        }

        public bool TryGetValueTypeless(IDecorationKey key, out object value)
        {
            bool found;

            if (key == DecorKeyConnector.FontFamily)
                value = decoration.GetValueConnectedTry(DecorationKey.FontFamily, TransformFontFamily, out found);

            else if (key == DecorKeyConnector.FontStretch)
                value = decoration.GetValueConnectedTry(DecorationKey.FontStretch, TransformFontStretch, out found);

            else if (key == DecorKeyConnector.FontStyle)
                value = decoration.GetValueConnectedTry(DecorationKey.FontStyle, TransformFontStyle, out found);

            else if (key == DecorKeyConnector.FontWeight)
                value = decoration.GetValueConnectedTry(DecorationKey.FontWeight, TransformFontWeight, out found);

            else if (key == DecorKeyConnector.TextDecoration)
                value = decoration.GetValueConnectedTry(DecorationKey.TextDecoration, TransformTextDecoration, out found);

            else if (key == DecorKeyConnector.BaselineAlignment)
                value = decoration.GetValueConnectedTry(DecorationKey.BaselineAlignment, TransformBaselineAlignment, out found);

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

        private Media.FontFamily TransformFontFamily(Styles.FontFamily fontFamily)
        {
            if (fontFamily == Styles.FontFamily.GenericMonospace)
                return new Media.FontFamily("Courier New");

            else if (fontFamily == Styles.FontFamily.GenericSansSerif)
                return new Media.FontFamily("Arial");

            else if (fontFamily == Styles.FontFamily.GenericSerif)
                return new Media.FontFamily("Times New Roman");

            else
                return new Media.FontFamily(fontFamily.Name);
        }

        private Windows.FontStretch TransformFontStretch(Styles.FontStretch fontStretch)
        {
            switch (fontStretch)
            {
                case Styles.FontStretch.Normal:
                    return Windows.FontStretches.Normal;

                case Styles.FontStretch.Condensed:
                    return Windows.FontStretches.Condensed;

                case Styles.FontStretch.Expanded:
                    return Windows.FontStretches.Expanded;

                default:
                    throw new ArgumentException("fontStretch");
            }
        }

        private Windows.FontStyle TransformFontStyle(Styles.FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case Styles.FontStyle.Normal:
                    return Windows.FontStyles.Normal;

                case Styles.FontStyle.Italic:
                    return Windows.FontStyles.Italic;

                default:
                    throw new ArgumentException("fontStyle");
            }
        }

        private Windows.FontWeight TransformFontWeight(Styles.FontWeight fontWeight)
        {
            switch (fontWeight)
            {
                case Styles.FontWeight.Normal:
                    return Windows.FontWeights.Normal;

                case Styles.FontWeight.Bold:
                    return Windows.FontWeights.Bold;

                case Styles.FontWeight.Thin:
                    return Windows.FontWeights.Thin;

                default:
                    throw new ArgumentException("fontWeight");
            }
        }

        private Windows.TextDecorationCollection TransformTextDecoration(Styles.TextDecoration textDecoration)
        {
            switch (textDecoration)
            {
                case Styles.TextDecoration.Baseline:
                    return Windows.TextDecorations.Baseline;

                case Styles.TextDecoration.OverLine:
                    return Windows.TextDecorations.OverLine;

                case Styles.TextDecoration.Strikethrough:
                    return Windows.TextDecorations.Strikethrough;

                case Styles.TextDecoration.Underline:
                    return Windows.TextDecorations.Underline;

                default:
                    throw new ArgumentException("textDecoration");
            }
        }

        private Windows.BaselineAlignment TransformBaselineAlignment(Styles.BaselineAlignment baselineAlignment)
        {
            switch (baselineAlignment)
            {
                case Styles.BaselineAlignment.Baseline:
                    return Windows.BaselineAlignment.Baseline;

                case Styles.BaselineAlignment.Bottom:
                    return Windows.BaselineAlignment.Bottom;

                case Styles.BaselineAlignment.Center:
                    return Windows.BaselineAlignment.Center;

                case Styles.BaselineAlignment.Subscript:
                    return Windows.BaselineAlignment.Subscript;

                case Styles.BaselineAlignment.Superscript:
                    return Windows.BaselineAlignment.Superscript;

                case Styles.BaselineAlignment.TextBottom:
                    return Windows.BaselineAlignment.TextBottom;

                case Styles.BaselineAlignment.TextTop:
                    return Windows.BaselineAlignment.TextTop;

                case Styles.BaselineAlignment.Top:
                    return Windows.BaselineAlignment.Top;

                default:
                    throw new ArgumentException("baselineAlignment");
            }
        }

        private Media.Color TransformColor(Styles.Color color)
        {
            return Media.Color.FromArgb(color.A(), color.R(), color.G(), color.B());
        }
    }

    public static class DecorConnectorExtensions
    {
        public static DecorConnector ToWPF(this IReadOnlyDecoration decoration)
        {
            return new DecorConnector(decoration);
        }
    }

    public static class DecorKeyConnector
    {
        public static DecorationKey<Media.FontFamily> FontFamily { get; private set; }
        public static DecorationKey<Windows.FontStretch> FontStretch { get; private set; }
        public static DecorationKey<Windows.FontStyle> FontStyle { get; private set; }
        public static DecorationKey<Windows.FontWeight> FontWeight { get; private set; }

        public static DecorationKey<double> FontSize { get; private set; }
        public static DecorationKey<double> FontSizeRelativePercent { get; private set; }

        public static DecorationKey<Windows.TextDecorationCollection> TextDecoration { get; private set; }
        public static DecorationKey<Windows.BaselineAlignment> BaselineAlignment { get; private set; }

        public static DecorationKey<Media.Color> Foreground { get; private set; }
        public static DecorationKey<Media.Color> Background { get; private set; }

        static DecorKeyConnector()
        {
            FontFamily = new DecorationKey<Media.FontFamily>("FontFamily");
            FontStretch = new DecorationKey<Windows.FontStretch>("FontStretch");
            FontStyle = new DecorationKey<Windows.FontStyle>("FontStyle");
            FontWeight = new DecorationKey<Windows.FontWeight>("FontWeight");

            FontSize = new DecorationKey<double>("FontSize");
            FontSizeRelativePercent = new DecorationKey<double>("FontSizeRelativePercent");

            TextDecoration = new DecorationKey<Windows.TextDecorationCollection>("TextDecorations");
            BaselineAlignment = new DecorationKey<Windows.BaselineAlignment>("BaselineAlignment");

            Foreground = new DecorationKey<Media.Color>("Foreground");
            Background = new DecorationKey<Media.Color>("Background");
        }
    }
}
