using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarcasm.Unparsing;

using Media = System.Windows.Media;
using Windows = System.Windows;
using Styles = Sarcasm.Unparsing.Styles;
using Unparsing = Sarcasm.Unparsing;

namespace DecorationConnector.WPF
{
    public class DecorConnector : IReadOnlyDecorationConnector
    {
        private readonly IReadOnlyDecoration decoration;

        public DecorConnector(IReadOnlyDecoration decoration)
        {
            this.decoration = decoration;
        }

        public bool ContainsKey(IDecorationKeyConnector key)
        {
            var connectedKey = key.DecorationKeyFrom;
            return decoration.ContainsKey(connectedKey);
        }

        public bool TryGetValue<T>(IDecorationKeyConnector<T> key, out T value)
        {
            object connectedValue;
            bool found = TryGetValueTypeless(key, out connectedValue);
            value = (T)key.Convert(connectedValue);
            return found;
        }

        public bool TryGetValueTypeless(IDecorationKeyConnector key, out object value)
        {
            var connectedKey = key.DecorationKeyFrom;
            return decoration.TryGetValueTypeless(connectedKey, out value);
        }
    }

    public static class DecorConnectorExtensions
    {
        public static DecorConnector ToWPF(this IReadOnlyDecoration decoration)
        {
            return new DecorConnector(decoration);
        }
    }

    public static class DecorationKey
    {
        public static DecorationKeyConnector<Styles.FontFamily, Media.FontFamily> FontFamily { get; private set; }
        public static DecorationKeyConnector<Styles.FontStretch, Windows.FontStretch> FontStretch { get; private set; }
        public static DecorationKeyConnector<Styles.FontStyle, Windows.FontStyle> FontStyle { get; private set; }
        public static DecorationKeyConnector<Styles.FontWeight, Windows.FontWeight> FontWeight { get; private set; }

        public static DecorationKeyConnector<double, double> FontSize { get; private set; }
        public static DecorationKeyConnector<double, double> FontSizeRelativePercent { get; private set; }

        public static DecorationKeyConnector<Styles.TextDecoration, Windows.TextDecorationCollection> TextDecoration { get; private set; }
        public static DecorationKeyConnector<Styles.BaselineAlignment, Windows.BaselineAlignment> BaselineAlignment { get; private set; }

        public static DecorationKeyConnector<Styles.Color, Media.Color> Foreground { get; private set; }
        public static DecorationKeyConnector<Styles.Color, Media.Color> Background { get; private set; }

        static DecorationKey()
        {
            FontFamily = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontFamily, fontFamily => new Media.FontFamily(fontFamily.Name));

            FontStretch = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontStretch,
                fontStretch =>
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
                );

            FontStyle = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontStyle,
                fontStyle =>
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
                );

            FontWeight = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontWeight,
                fontWeight =>
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
                );

            FontSize = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontSize, fontSize => fontSize);

            FontSizeRelativePercent = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontSizeRelativePercent, fontSizeRelativePercent => fontSizeRelativePercent);

            TextDecoration = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.TextDecoration,
                textDecoration =>
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
                );

            BaselineAlignment = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.BaselineAlignment,
                baselineAlignment =>
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
                );

            Foreground = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.Foreground, color => new Media.Color() { A = color.A, R = color.R, G = color.G, B = color.B });

            Background = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.Background, color => new Media.Color() { A = color.A, R = color.R, G = color.G, B = color.B });
        }
    }
}
