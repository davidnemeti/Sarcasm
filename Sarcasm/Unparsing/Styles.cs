
// GENERATED FILE
// NOTE: we store the resulted C# file in a separate file detached from the tt file since using the PresentationCore assembly during generation somehow causing problems for MSBuild

using System;

namespace Sarcasm.Unparsing.Styles
{
    public enum FontWeight { Normal, Bold, Thin }

    public class TextDecoration
    {
        static TextDecoration()
        {
            Baseline = new TextDecoration() { Location = TextDecorationLocation.Baseline };
            OverLine = new TextDecoration() { Location = TextDecorationLocation.OverLine };
            Strikethrough = new TextDecoration() { Location = TextDecorationLocation.Strikethrough };
            Underline = new TextDecoration() { Location = TextDecorationLocation.Underline };
        }

        public TextDecorationLocation Location { get; set; }
        public Pen Pen { get; set; }
        public double PenOffset { get; set; }

        public static TextDecoration Baseline { get; private set; }
        public static TextDecoration OverLine { get; private set; }
        public static TextDecoration Strikethrough { get; private set; }
        public static TextDecoration Underline { get; private set; }
    }

    public enum TextDecorationLocation { Baseline, OverLine, Strikethrough, Underline }

    public class Pen
    {
        public Color Color { get; set; }
        public double Thickness { get; set; }
    }

    public enum FontStyle { Normal, Italic }

    /// <summary>
    /// Describes how the baseline for a text-based element is positioned on the
    /// vertical axis, relative to the established baseline for text.
    /// </summary>
    public enum BaselineAlignment
    {
        // A baseline that is aligned to the upper edge of the containing box.
        Top,
        // A baseline that is aligned to the center of the containing box.
        Center,
        // A baseline that is aligned at the lower edge of the containing box.
        Bottom,
        // A baseline that is aligned at the actual baseline of the containing box.
        Baseline,
        // A baseline that is aligned at the upper edge of the text baseline.
        TextTop,
        // A baseline that is aligned at the lower edge of the text baseline.
        TextBottom,
        // A baseline that is aligned at the subscript position of the containing box.
        Subscript,
        // A baseline that is aligned at the superscript position of the containing box.
        Superscript
    }

    public enum FontStretch { Normal, Condensed, Expanded }

    public class FontFamily
    {
        static FontFamily()
        {
            GenericMonospace = new FontFamily("GenericMonospace");
            GenericSansSerif = new FontFamily("GenericSansSerif");
            GenericSerif = new FontFamily("GenericSerif");
        }

        public FontFamily(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static FontFamily GenericMonospace { get; private set; }
        public static FontFamily GenericSansSerif { get; private set; }
        public static FontFamily GenericSerif { get; private set; }
    }

    public enum Color : uint
    {
        AliceBlue = (255u << 24 | 240u << 16 | 248u << 8 | 255u),
        AntiqueWhite = (255u << 24 | 250u << 16 | 235u << 8 | 215u),
        Aqua = (255u << 24 | 0u << 16 | 255u << 8 | 255u),
        Aquamarine = (255u << 24 | 127u << 16 | 255u << 8 | 212u),
        Azure = (255u << 24 | 240u << 16 | 255u << 8 | 255u),
        Beige = (255u << 24 | 245u << 16 | 245u << 8 | 220u),
        Bisque = (255u << 24 | 255u << 16 | 228u << 8 | 196u),
        Black = (255u << 24 | 0u << 16 | 0u << 8 | 0u),
        BlanchedAlmond = (255u << 24 | 255u << 16 | 235u << 8 | 205u),
        Blue = (255u << 24 | 0u << 16 | 0u << 8 | 255u),
        BlueViolet = (255u << 24 | 138u << 16 | 43u << 8 | 226u),
        Brown = (255u << 24 | 165u << 16 | 42u << 8 | 42u),
        BurlyWood = (255u << 24 | 222u << 16 | 184u << 8 | 135u),
        CadetBlue = (255u << 24 | 95u << 16 | 158u << 8 | 160u),
        Chartreuse = (255u << 24 | 127u << 16 | 255u << 8 | 0u),
        Chocolate = (255u << 24 | 210u << 16 | 105u << 8 | 30u),
        Coral = (255u << 24 | 255u << 16 | 127u << 8 | 80u),
        CornflowerBlue = (255u << 24 | 100u << 16 | 149u << 8 | 237u),
        Cornsilk = (255u << 24 | 255u << 16 | 248u << 8 | 220u),
        Crimson = (255u << 24 | 220u << 16 | 20u << 8 | 60u),
        Cyan = (255u << 24 | 0u << 16 | 255u << 8 | 255u),
        DarkBlue = (255u << 24 | 0u << 16 | 0u << 8 | 139u),
        DarkCyan = (255u << 24 | 0u << 16 | 139u << 8 | 139u),
        DarkGoldenrod = (255u << 24 | 184u << 16 | 134u << 8 | 11u),
        DarkGray = (255u << 24 | 169u << 16 | 169u << 8 | 169u),
        DarkGreen = (255u << 24 | 0u << 16 | 100u << 8 | 0u),
        DarkKhaki = (255u << 24 | 189u << 16 | 183u << 8 | 107u),
        DarkMagenta = (255u << 24 | 139u << 16 | 0u << 8 | 139u),
        DarkOliveGreen = (255u << 24 | 85u << 16 | 107u << 8 | 47u),
        DarkOrange = (255u << 24 | 255u << 16 | 140u << 8 | 0u),
        DarkOrchid = (255u << 24 | 153u << 16 | 50u << 8 | 204u),
        DarkRed = (255u << 24 | 139u << 16 | 0u << 8 | 0u),
        DarkSalmon = (255u << 24 | 233u << 16 | 150u << 8 | 122u),
        DarkSeaGreen = (255u << 24 | 143u << 16 | 188u << 8 | 143u),
        DarkSlateBlue = (255u << 24 | 72u << 16 | 61u << 8 | 139u),
        DarkSlateGray = (255u << 24 | 47u << 16 | 79u << 8 | 79u),
        DarkTurquoise = (255u << 24 | 0u << 16 | 206u << 8 | 209u),
        DarkViolet = (255u << 24 | 148u << 16 | 0u << 8 | 211u),
        DeepPink = (255u << 24 | 255u << 16 | 20u << 8 | 147u),
        DeepSkyBlue = (255u << 24 | 0u << 16 | 191u << 8 | 255u),
        DimGray = (255u << 24 | 105u << 16 | 105u << 8 | 105u),
        DodgerBlue = (255u << 24 | 30u << 16 | 144u << 8 | 255u),
        Firebrick = (255u << 24 | 178u << 16 | 34u << 8 | 34u),
        FloralWhite = (255u << 24 | 255u << 16 | 250u << 8 | 240u),
        ForestGreen = (255u << 24 | 34u << 16 | 139u << 8 | 34u),
        Fuchsia = (255u << 24 | 255u << 16 | 0u << 8 | 255u),
        Gainsboro = (255u << 24 | 220u << 16 | 220u << 8 | 220u),
        GhostWhite = (255u << 24 | 248u << 16 | 248u << 8 | 255u),
        Gold = (255u << 24 | 255u << 16 | 215u << 8 | 0u),
        Goldenrod = (255u << 24 | 218u << 16 | 165u << 8 | 32u),
        Gray = (255u << 24 | 128u << 16 | 128u << 8 | 128u),
        Green = (255u << 24 | 0u << 16 | 128u << 8 | 0u),
        GreenYellow = (255u << 24 | 173u << 16 | 255u << 8 | 47u),
        Honeydew = (255u << 24 | 240u << 16 | 255u << 8 | 240u),
        HotPink = (255u << 24 | 255u << 16 | 105u << 8 | 180u),
        IndianRed = (255u << 24 | 205u << 16 | 92u << 8 | 92u),
        Indigo = (255u << 24 | 75u << 16 | 0u << 8 | 130u),
        Ivory = (255u << 24 | 255u << 16 | 255u << 8 | 240u),
        Khaki = (255u << 24 | 240u << 16 | 230u << 8 | 140u),
        Lavender = (255u << 24 | 230u << 16 | 230u << 8 | 250u),
        LavenderBlush = (255u << 24 | 255u << 16 | 240u << 8 | 245u),
        LawnGreen = (255u << 24 | 124u << 16 | 252u << 8 | 0u),
        LemonChiffon = (255u << 24 | 255u << 16 | 250u << 8 | 205u),
        LightBlue = (255u << 24 | 173u << 16 | 216u << 8 | 230u),
        LightCoral = (255u << 24 | 240u << 16 | 128u << 8 | 128u),
        LightCyan = (255u << 24 | 224u << 16 | 255u << 8 | 255u),
        LightGoldenrodYellow = (255u << 24 | 250u << 16 | 250u << 8 | 210u),
        LightGray = (255u << 24 | 211u << 16 | 211u << 8 | 211u),
        LightGreen = (255u << 24 | 144u << 16 | 238u << 8 | 144u),
        LightPink = (255u << 24 | 255u << 16 | 182u << 8 | 193u),
        LightSalmon = (255u << 24 | 255u << 16 | 160u << 8 | 122u),
        LightSeaGreen = (255u << 24 | 32u << 16 | 178u << 8 | 170u),
        LightSkyBlue = (255u << 24 | 135u << 16 | 206u << 8 | 250u),
        LightSlateGray = (255u << 24 | 119u << 16 | 136u << 8 | 153u),
        LightSteelBlue = (255u << 24 | 176u << 16 | 196u << 8 | 222u),
        LightYellow = (255u << 24 | 255u << 16 | 255u << 8 | 224u),
        Lime = (255u << 24 | 0u << 16 | 255u << 8 | 0u),
        LimeGreen = (255u << 24 | 50u << 16 | 205u << 8 | 50u),
        Linen = (255u << 24 | 250u << 16 | 240u << 8 | 230u),
        Magenta = (255u << 24 | 255u << 16 | 0u << 8 | 255u),
        Maroon = (255u << 24 | 128u << 16 | 0u << 8 | 0u),
        MediumAquamarine = (255u << 24 | 102u << 16 | 205u << 8 | 170u),
        MediumBlue = (255u << 24 | 0u << 16 | 0u << 8 | 205u),
        MediumOrchid = (255u << 24 | 186u << 16 | 85u << 8 | 211u),
        MediumPurple = (255u << 24 | 147u << 16 | 112u << 8 | 219u),
        MediumSeaGreen = (255u << 24 | 60u << 16 | 179u << 8 | 113u),
        MediumSlateBlue = (255u << 24 | 123u << 16 | 104u << 8 | 238u),
        MediumSpringGreen = (255u << 24 | 0u << 16 | 250u << 8 | 154u),
        MediumTurquoise = (255u << 24 | 72u << 16 | 209u << 8 | 204u),
        MediumVioletRed = (255u << 24 | 199u << 16 | 21u << 8 | 133u),
        MidnightBlue = (255u << 24 | 25u << 16 | 25u << 8 | 112u),
        MintCream = (255u << 24 | 245u << 16 | 255u << 8 | 250u),
        MistyRose = (255u << 24 | 255u << 16 | 228u << 8 | 225u),
        Moccasin = (255u << 24 | 255u << 16 | 228u << 8 | 181u),
        NavajoWhite = (255u << 24 | 255u << 16 | 222u << 8 | 173u),
        Navy = (255u << 24 | 0u << 16 | 0u << 8 | 128u),
        OldLace = (255u << 24 | 253u << 16 | 245u << 8 | 230u),
        Olive = (255u << 24 | 128u << 16 | 128u << 8 | 0u),
        OliveDrab = (255u << 24 | 107u << 16 | 142u << 8 | 35u),
        Orange = (255u << 24 | 255u << 16 | 165u << 8 | 0u),
        OrangeRed = (255u << 24 | 255u << 16 | 69u << 8 | 0u),
        Orchid = (255u << 24 | 218u << 16 | 112u << 8 | 214u),
        PaleGoldenrod = (255u << 24 | 238u << 16 | 232u << 8 | 170u),
        PaleGreen = (255u << 24 | 152u << 16 | 251u << 8 | 152u),
        PaleTurquoise = (255u << 24 | 175u << 16 | 238u << 8 | 238u),
        PaleVioletRed = (255u << 24 | 219u << 16 | 112u << 8 | 147u),
        PapayaWhip = (255u << 24 | 255u << 16 | 239u << 8 | 213u),
        PeachPuff = (255u << 24 | 255u << 16 | 218u << 8 | 185u),
        Peru = (255u << 24 | 205u << 16 | 133u << 8 | 63u),
        Pink = (255u << 24 | 255u << 16 | 192u << 8 | 203u),
        Plum = (255u << 24 | 221u << 16 | 160u << 8 | 221u),
        PowderBlue = (255u << 24 | 176u << 16 | 224u << 8 | 230u),
        Purple = (255u << 24 | 128u << 16 | 0u << 8 | 128u),
        Red = (255u << 24 | 255u << 16 | 0u << 8 | 0u),
        RosyBrown = (255u << 24 | 188u << 16 | 143u << 8 | 143u),
        RoyalBlue = (255u << 24 | 65u << 16 | 105u << 8 | 225u),
        SaddleBrown = (255u << 24 | 139u << 16 | 69u << 8 | 19u),
        Salmon = (255u << 24 | 250u << 16 | 128u << 8 | 114u),
        SandyBrown = (255u << 24 | 244u << 16 | 164u << 8 | 96u),
        SeaGreen = (255u << 24 | 46u << 16 | 139u << 8 | 87u),
        SeaShell = (255u << 24 | 255u << 16 | 245u << 8 | 238u),
        Sienna = (255u << 24 | 160u << 16 | 82u << 8 | 45u),
        Silver = (255u << 24 | 192u << 16 | 192u << 8 | 192u),
        SkyBlue = (255u << 24 | 135u << 16 | 206u << 8 | 235u),
        SlateBlue = (255u << 24 | 106u << 16 | 90u << 8 | 205u),
        SlateGray = (255u << 24 | 112u << 16 | 128u << 8 | 144u),
        Snow = (255u << 24 | 255u << 16 | 250u << 8 | 250u),
        SpringGreen = (255u << 24 | 0u << 16 | 255u << 8 | 127u),
        SteelBlue = (255u << 24 | 70u << 16 | 130u << 8 | 180u),
        Tan = (255u << 24 | 210u << 16 | 180u << 8 | 140u),
        Teal = (255u << 24 | 0u << 16 | 128u << 8 | 128u),
        Thistle = (255u << 24 | 216u << 16 | 191u << 8 | 216u),
        Tomato = (255u << 24 | 255u << 16 | 99u << 8 | 71u),
        Transparent = (0u << 24 | 255u << 16 | 255u << 8 | 255u),
        Turquoise = (255u << 24 | 64u << 16 | 224u << 8 | 208u),
        Violet = (255u << 24 | 238u << 16 | 130u << 8 | 238u),
        Wheat = (255u << 24 | 245u << 16 | 222u << 8 | 179u),
        White = (255u << 24 | 255u << 16 | 255u << 8 | 255u),
        WhiteSmoke = (255u << 24 | 245u << 16 | 245u << 8 | 245u),
        Yellow = (255u << 24 | 255u << 16 | 255u << 8 | 0u),
        YellowGreen = (255u << 24 | 154u << 16 | 205u << 8 | 50u),
    }

    public static class ColorFactory
    {
        public static Color CreateColor(byte A, byte R, byte G, byte B)
        {
            return (Color)(((uint)A) << 24 | ((uint)R) << 16 | ((uint)G) << 8 | ((uint)B));
        }

        public static byte A(this Color color)
        {
            return (byte)((((uint)color) & 0xFF000000) >> 24);
        }

        public static byte R(this Color color)
        {
            return (byte)((((uint)color) & 0x00FF0000) >> 16);
        }

        public static byte G(this Color color)
        {
            return (byte)((((uint)color) & 0x0000FF00) >> 8);
        }

        public static byte B(this Color color)
        {
            return (byte)((((uint)color) & 0x000000FF) >> 0);
        }
    }
}
