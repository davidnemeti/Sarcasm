namespace Sarcasm.Unparsing.Styles
{
    public enum FontWeight { Normal, Bold, Thin }

    public enum TextDecoration { Baseline, OverLine, Strikethrough, Underline }

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

    public struct Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static Color AliceBlue            { get { return new Color() { A = 255, R = 240, G = 248, B = 255} ; } }
        public static Color AntiqueWhite         { get { return new Color() { A = 255, R = 250, G = 235, B = 215} ; } }
        public static Color Aqua                 { get { return new Color() { A = 255, R = 0, G = 255, B = 255} ; } }
        public static Color Aquamarine           { get { return new Color() { A = 255, R = 127, G = 255, B = 212} ; } }
        public static Color Azure                { get { return new Color() { A = 255, R = 240, G = 255, B = 255} ; } }
        public static Color Beige                { get { return new Color() { A = 255, R = 245, G = 245, B = 220} ; } }
        public static Color Bisque               { get { return new Color() { A = 255, R = 255, G = 228, B = 196} ; } }
        public static Color Black                { get { return new Color() { A = 255, R = 0, G = 0, B = 0} ; } }
        public static Color BlanchedAlmond       { get { return new Color() { A = 255, R = 255, G = 235, B = 205} ; } }
        public static Color Blue                 { get { return new Color() { A = 255, R = 0, G = 0, B = 255} ; } }
        public static Color BlueViolet           { get { return new Color() { A = 255, R = 138, G = 43, B = 226} ; } }
        public static Color Brown                { get { return new Color() { A = 255, R = 165, G = 42, B = 42} ; } }
        public static Color BurlyWood            { get { return new Color() { A = 255, R = 222, G = 184, B = 135} ; } }
        public static Color CadetBlue            { get { return new Color() { A = 255, R = 95, G = 158, B = 160} ; } }
        public static Color Chartreuse           { get { return new Color() { A = 255, R = 127, G = 255, B = 0} ; } }
        public static Color Chocolate            { get { return new Color() { A = 255, R = 210, G = 105, B = 30} ; } }
        public static Color Coral                { get { return new Color() { A = 255, R = 255, G = 127, B = 80} ; } }
        public static Color CornflowerBlue       { get { return new Color() { A = 255, R = 100, G = 149, B = 237} ; } }
        public static Color Cornsilk             { get { return new Color() { A = 255, R = 255, G = 248, B = 220} ; } }
        public static Color Crimson              { get { return new Color() { A = 255, R = 220, G = 20, B = 60} ; } }
        public static Color Cyan                 { get { return new Color() { A = 255, R = 0, G = 255, B = 255} ; } }
        public static Color DarkBlue             { get { return new Color() { A = 255, R = 0, G = 0, B = 139} ; } }
        public static Color DarkCyan             { get { return new Color() { A = 255, R = 0, G = 139, B = 139} ; } }
        public static Color DarkGoldenrod        { get { return new Color() { A = 255, R = 184, G = 134, B = 11} ; } }
        public static Color DarkGray             { get { return new Color() { A = 255, R = 169, G = 169, B = 169} ; } }
        public static Color DarkGreen            { get { return new Color() { A = 255, R = 0, G = 100, B = 0} ; } }
        public static Color DarkKhaki            { get { return new Color() { A = 255, R = 189, G = 183, B = 107} ; } }
        public static Color DarkMagenta          { get { return new Color() { A = 255, R = 139, G = 0, B = 139} ; } }
        public static Color DarkOliveGreen       { get { return new Color() { A = 255, R = 85, G = 107, B = 47} ; } }
        public static Color DarkOrange           { get { return new Color() { A = 255, R = 255, G = 140, B = 0} ; } }
        public static Color DarkOrchid           { get { return new Color() { A = 255, R = 153, G = 50, B = 204} ; } }
        public static Color DarkRed              { get { return new Color() { A = 255, R = 139, G = 0, B = 0} ; } }
        public static Color DarkSalmon           { get { return new Color() { A = 255, R = 233, G = 150, B = 122} ; } }
        public static Color DarkSeaGreen         { get { return new Color() { A = 255, R = 143, G = 188, B = 143} ; } }
        public static Color DarkSlateBlue        { get { return new Color() { A = 255, R = 72, G = 61, B = 139} ; } }
        public static Color DarkSlateGray        { get { return new Color() { A = 255, R = 47, G = 79, B = 79} ; } }
        public static Color DarkTurquoise        { get { return new Color() { A = 255, R = 0, G = 206, B = 209} ; } }
        public static Color DarkViolet           { get { return new Color() { A = 255, R = 148, G = 0, B = 211} ; } }
        public static Color DeepPink             { get { return new Color() { A = 255, R = 255, G = 20, B = 147} ; } }
        public static Color DeepSkyBlue          { get { return new Color() { A = 255, R = 0, G = 191, B = 255} ; } }
        public static Color DimGray              { get { return new Color() { A = 255, R = 105, G = 105, B = 105} ; } }
        public static Color DodgerBlue           { get { return new Color() { A = 255, R = 30, G = 144, B = 255} ; } }
        public static Color Firebrick            { get { return new Color() { A = 255, R = 178, G = 34, B = 34} ; } }
        public static Color FloralWhite          { get { return new Color() { A = 255, R = 255, G = 250, B = 240} ; } }
        public static Color ForestGreen          { get { return new Color() { A = 255, R = 34, G = 139, B = 34} ; } }
        public static Color Fuchsia              { get { return new Color() { A = 255, R = 255, G = 0, B = 255} ; } }
        public static Color Gainsboro            { get { return new Color() { A = 255, R = 220, G = 220, B = 220} ; } }
        public static Color GhostWhite           { get { return new Color() { A = 255, R = 248, G = 248, B = 255} ; } }
        public static Color Gold                 { get { return new Color() { A = 255, R = 255, G = 215, B = 0} ; } }
        public static Color Goldenrod            { get { return new Color() { A = 255, R = 218, G = 165, B = 32} ; } }
        public static Color Gray                 { get { return new Color() { A = 255, R = 128, G = 128, B = 128} ; } }
        public static Color Green                { get { return new Color() { A = 255, R = 0, G = 128, B = 0} ; } }
        public static Color GreenYellow          { get { return new Color() { A = 255, R = 173, G = 255, B = 47} ; } }
        public static Color Honeydew             { get { return new Color() { A = 255, R = 240, G = 255, B = 240} ; } }
        public static Color HotPink              { get { return new Color() { A = 255, R = 255, G = 105, B = 180} ; } }
        public static Color IndianRed            { get { return new Color() { A = 255, R = 205, G = 92, B = 92} ; } }
        public static Color Indigo               { get { return new Color() { A = 255, R = 75, G = 0, B = 130} ; } }
        public static Color Ivory                { get { return new Color() { A = 255, R = 255, G = 255, B = 240} ; } }
        public static Color Khaki                { get { return new Color() { A = 255, R = 240, G = 230, B = 140} ; } }
        public static Color Lavender             { get { return new Color() { A = 255, R = 230, G = 230, B = 250} ; } }
        public static Color LavenderBlush        { get { return new Color() { A = 255, R = 255, G = 240, B = 245} ; } }
        public static Color LawnGreen            { get { return new Color() { A = 255, R = 124, G = 252, B = 0} ; } }
        public static Color LemonChiffon         { get { return new Color() { A = 255, R = 255, G = 250, B = 205} ; } }
        public static Color LightBlue            { get { return new Color() { A = 255, R = 173, G = 216, B = 230} ; } }
        public static Color LightCoral           { get { return new Color() { A = 255, R = 240, G = 128, B = 128} ; } }
        public static Color LightCyan            { get { return new Color() { A = 255, R = 224, G = 255, B = 255} ; } }
        public static Color LightGoldenrodYellow { get { return new Color() { A = 255, R = 250, G = 250, B = 210} ; } }
        public static Color LightGray            { get { return new Color() { A = 255, R = 211, G = 211, B = 211} ; } }
        public static Color LightGreen           { get { return new Color() { A = 255, R = 144, G = 238, B = 144} ; } }
        public static Color LightPink            { get { return new Color() { A = 255, R = 255, G = 182, B = 193} ; } }
        public static Color LightSalmon          { get { return new Color() { A = 255, R = 255, G = 160, B = 122} ; } }
        public static Color LightSeaGreen        { get { return new Color() { A = 255, R = 32, G = 178, B = 170} ; } }
        public static Color LightSkyBlue         { get { return new Color() { A = 255, R = 135, G = 206, B = 250} ; } }
        public static Color LightSlateGray       { get { return new Color() { A = 255, R = 119, G = 136, B = 153} ; } }
        public static Color LightSteelBlue       { get { return new Color() { A = 255, R = 176, G = 196, B = 222} ; } }
        public static Color LightYellow          { get { return new Color() { A = 255, R = 255, G = 255, B = 224} ; } }
        public static Color Lime                 { get { return new Color() { A = 255, R = 0, G = 255, B = 0} ; } }
        public static Color LimeGreen            { get { return new Color() { A = 255, R = 50, G = 205, B = 50} ; } }
        public static Color Linen                { get { return new Color() { A = 255, R = 250, G = 240, B = 230} ; } }
        public static Color Magenta              { get { return new Color() { A = 255, R = 255, G = 0, B = 255} ; } }
        public static Color Maroon               { get { return new Color() { A = 255, R = 128, G = 0, B = 0} ; } }
        public static Color MediumAquamarine     { get { return new Color() { A = 255, R = 102, G = 205, B = 170} ; } }
        public static Color MediumBlue           { get { return new Color() { A = 255, R = 0, G = 0, B = 205} ; } }
        public static Color MediumOrchid         { get { return new Color() { A = 255, R = 186, G = 85, B = 211} ; } }
        public static Color MediumPurple         { get { return new Color() { A = 255, R = 147, G = 112, B = 219} ; } }
        public static Color MediumSeaGreen       { get { return new Color() { A = 255, R = 60, G = 179, B = 113} ; } }
        public static Color MediumSlateBlue      { get { return new Color() { A = 255, R = 123, G = 104, B = 238} ; } }
        public static Color MediumSpringGreen    { get { return new Color() { A = 255, R = 0, G = 250, B = 154} ; } }
        public static Color MediumTurquoise      { get { return new Color() { A = 255, R = 72, G = 209, B = 204} ; } }
        public static Color MediumVioletRed      { get { return new Color() { A = 255, R = 199, G = 21, B = 133} ; } }
        public static Color MidnightBlue         { get { return new Color() { A = 255, R = 25, G = 25, B = 112} ; } }
        public static Color MintCream            { get { return new Color() { A = 255, R = 245, G = 255, B = 250} ; } }
        public static Color MistyRose            { get { return new Color() { A = 255, R = 255, G = 228, B = 225} ; } }
        public static Color Moccasin             { get { return new Color() { A = 255, R = 255, G = 228, B = 181} ; } }
        public static Color NavajoWhite          { get { return new Color() { A = 255, R = 255, G = 222, B = 173} ; } }
        public static Color Navy                 { get { return new Color() { A = 255, R = 0, G = 0, B = 128} ; } }
        public static Color OldLace              { get { return new Color() { A = 255, R = 253, G = 245, B = 230} ; } }
        public static Color Olive                { get { return new Color() { A = 255, R = 128, G = 128, B = 0} ; } }
        public static Color OliveDrab            { get { return new Color() { A = 255, R = 107, G = 142, B = 35} ; } }
        public static Color Orange               { get { return new Color() { A = 255, R = 255, G = 165, B = 0} ; } }
        public static Color OrangeRed            { get { return new Color() { A = 255, R = 255, G = 69, B = 0} ; } }
        public static Color Orchid               { get { return new Color() { A = 255, R = 218, G = 112, B = 214} ; } }
        public static Color PaleGoldenrod        { get { return new Color() { A = 255, R = 238, G = 232, B = 170} ; } }
        public static Color PaleGreen            { get { return new Color() { A = 255, R = 152, G = 251, B = 152} ; } }
        public static Color PaleTurquoise        { get { return new Color() { A = 255, R = 175, G = 238, B = 238} ; } }
        public static Color PaleVioletRed        { get { return new Color() { A = 255, R = 219, G = 112, B = 147} ; } }
        public static Color PapayaWhip           { get { return new Color() { A = 255, R = 255, G = 239, B = 213} ; } }
        public static Color PeachPuff            { get { return new Color() { A = 255, R = 255, G = 218, B = 185} ; } }
        public static Color Peru                 { get { return new Color() { A = 255, R = 205, G = 133, B = 63} ; } }
        public static Color Pink                 { get { return new Color() { A = 255, R = 255, G = 192, B = 203} ; } }
        public static Color Plum                 { get { return new Color() { A = 255, R = 221, G = 160, B = 221} ; } }
        public static Color PowderBlue           { get { return new Color() { A = 255, R = 176, G = 224, B = 230} ; } }
        public static Color Purple               { get { return new Color() { A = 255, R = 128, G = 0, B = 128} ; } }
        public static Color Red                  { get { return new Color() { A = 255, R = 255, G = 0, B = 0} ; } }
        public static Color RosyBrown            { get { return new Color() { A = 255, R = 188, G = 143, B = 143} ; } }
        public static Color RoyalBlue            { get { return new Color() { A = 255, R = 65, G = 105, B = 225} ; } }
        public static Color SaddleBrown          { get { return new Color() { A = 255, R = 139, G = 69, B = 19} ; } }
        public static Color Salmon               { get { return new Color() { A = 255, R = 250, G = 128, B = 114} ; } }
        public static Color SandyBrown           { get { return new Color() { A = 255, R = 244, G = 164, B = 96} ; } }
        public static Color SeaGreen             { get { return new Color() { A = 255, R = 46, G = 139, B = 87} ; } }
        public static Color SeaShell             { get { return new Color() { A = 255, R = 255, G = 245, B = 238} ; } }
        public static Color Sienna               { get { return new Color() { A = 255, R = 160, G = 82, B = 45} ; } }
        public static Color Silver               { get { return new Color() { A = 255, R = 192, G = 192, B = 192} ; } }
        public static Color SkyBlue              { get { return new Color() { A = 255, R = 135, G = 206, B = 235} ; } }
        public static Color SlateBlue            { get { return new Color() { A = 255, R = 106, G = 90, B = 205} ; } }
        public static Color SlateGray            { get { return new Color() { A = 255, R = 112, G = 128, B = 144} ; } }
        public static Color Snow                 { get { return new Color() { A = 255, R = 255, G = 250, B = 250} ; } }
        public static Color SpringGreen          { get { return new Color() { A = 255, R = 0, G = 255, B = 127} ; } }
        public static Color SteelBlue            { get { return new Color() { A = 255, R = 70, G = 130, B = 180} ; } }
        public static Color Tan                  { get { return new Color() { A = 255, R = 210, G = 180, B = 140} ; } }
        public static Color Teal                 { get { return new Color() { A = 255, R = 0, G = 128, B = 128} ; } }
        public static Color Thistle              { get { return new Color() { A = 255, R = 216, G = 191, B = 216} ; } }
        public static Color Tomato               { get { return new Color() { A = 255, R = 255, G = 99, B = 71} ; } }
        public static Color Transparent          { get { return new Color() { A = 0, R = 255, G = 255, B = 255} ; } }
        public static Color Turquoise            { get { return new Color() { A = 255, R = 64, G = 224, B = 208} ; } }
        public static Color Violet               { get { return new Color() { A = 255, R = 238, G = 130, B = 238} ; } }
        public static Color Wheat                { get { return new Color() { A = 255, R = 245, G = 222, B = 179} ; } }
        public static Color White                { get { return new Color() { A = 255, R = 255, G = 255, B = 255} ; } }
        public static Color WhiteSmoke           { get { return new Color() { A = 255, R = 245, G = 245, B = 245} ; } }
        public static Color Yellow               { get { return new Color() { A = 255, R = 255, G = 255, B = 0} ; } }
        public static Color YellowGreen          { get { return new Color() { A = 255, R = 154, G = 205, B = 50} ; } }
    }
}
