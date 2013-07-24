using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sarcasm.Unparsing;

using Drawing = System.Drawing;
using Windows = System.Windows;
using Styles = Sarcasm.Unparsing.Styles;
using Unparsing = Sarcasm.Unparsing;

namespace DecorationConnector.Forms
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
            object valueFrom;
            bool found = decoration.TryGetValueTypeless(key.DecorationKeyFrom, out valueFrom);
            value = (T)key.Convert(valueFrom);
            return found;
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
        /*
         * The following decorations has no pairs in Windows Forms:
         * 
         * Styles.FontStretch
         * Styles.BaselineAlignment
         * */

        public static DecorationKeyConnector<Styles.FontFamily, Drawing.FontFamily> FontFamily { get; private set; }
        public static DecorationKeyConnector<Styles.FontStyle, Drawing.FontStyle> FontStyle { get; private set; }
        public static DecorationKeyConnector<Styles.FontWeight, Drawing.FontStyle> FontWeight { get; private set; }

        public static DecorationKeyConnector<double, double> FontSize { get; private set; }
        public static DecorationKeyConnector<double, double> FontSizeRelativePercent { get; private set; }

        public static DecorationKeyConnector<Styles.TextDecoration, Drawing.FontStyle> TextDecoration { get; private set; }

        public static DecorationKeyConnector<Styles.Color, Drawing.Color> Foreground { get; private set; }
        public static DecorationKeyConnector<Styles.Color, Drawing.Color> Background { get; private set; }

        static DecorKeyConnector()
        {
            FontFamily = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontFamily,
                fontFamily =>
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
                );

            FontStyle = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontStyle,
                fontStyle =>
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
                );

            FontWeight = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontWeight,
                fontWeight =>
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
                );

            FontSize = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontSize, fontSize => fontSize);

            FontSizeRelativePercent = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.FontSizeRelativePercent, fontSizeRelativePercent => fontSizeRelativePercent);

            TextDecoration = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.TextDecoration,
                textDecoration =>
                {
                    switch (textDecoration)
                    {
                        case Styles.TextDecoration.Baseline:
                        case Styles.TextDecoration.OverLine:
                            return Drawing.FontStyle.Regular;       // no pair for these -> return the default style

                        case Styles.TextDecoration.Strikethrough:
                            return Drawing.FontStyle.Strikeout;

                        case Styles.TextDecoration.Underline:
                            return Drawing.FontStyle.Underline;

                        default:
                            throw new ArgumentException("textDecoration");
                    }
                }
                );

            Foreground = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.Foreground, color => Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));

            Background = DecorationKeyConnector.ConnectTo(Unparsing.DecorationKey.Background, color => Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
