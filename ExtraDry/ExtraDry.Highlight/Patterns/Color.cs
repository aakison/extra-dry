using System;
using System.Collections.Generic;

namespace ExtraDry.Highlight;

public class Color
{
    public Color(string name, string code)
    {
        Name = name;
        Code = code;
        var hex = Code.TrimStart('#');
        if(hex.Length >= 6) {
            R = Convert.ToByte(hex.Substring(0, 2), 16);
            G = Convert.ToByte(hex.Substring(2, 2), 16);
            B = Convert.ToByte(hex.Substring(4, 2), 16);
            if(hex.Length >= 8)
            {
                A = Convert.ToByte(hex.Substring(6, 2), 16);
            }
            else
            {
                A = 255;
            }
        }

    }

    public static Color Empty => HtmlColors["transparent"];

    public static Color FromName(string? name)
    {
        if(string.IsNullOrWhiteSpace(name)) {
            return Empty;
        }
        else if(name.StartsWith("#"))
        {
            return new Color(name, name);
        }
        else
        {
            return HtmlColors[name];
        }
    }

    private static Dictionary<string, Color> HtmlColors = new Dictionary<string, Color>(StringComparer.InvariantCultureIgnoreCase);

    static Color()
    {
        var dict = HtmlColors;

        dict.Add("Transparent", new Color("Transparent", "#00000000"));

        dict.Add("AliceBlue", new Color("AliceBlue", "#F0F8FF"));

        dict.Add("AntiqueWhite", new Color("AntiqueWhite", "#FAEBD7"));

        dict.Add("Aqua", new Color("Aqua", "#00FFFF"));

        dict.Add("Aquamarine", new Color("Aquamarine", "#7FFFD4"));

        dict.Add("Azure", new Color("Azure", "#F0FFFF"));

        dict.Add("Beige", new Color("Beige", "#F5F5DC"));

        dict.Add("Bisque", new Color("Bisque", "#FFE4C4"));

        dict.Add("Black", new Color("Black", "#000000"));

        dict.Add("BlanchedAlmond", new Color("BlanchedAlmond", "#FFEBCD"));

        dict.Add("Blue", new Color("Blue", "#0000FF"));

        dict.Add("BlueViolet", new Color("BlueViolet", "#8A2BE2"));

        dict.Add("Brown", new Color("Brown", "#A52A2A"));

        dict.Add("BurlyWood", new Color("BurlyWood", "#DEB887"));

        dict.Add("CadetBlue", new Color("CadetBlue", "#5F9EA0"));

        dict.Add("Chartreuse", new Color("Chartreuse", "#7FFF00"));

        dict.Add("Chocolate", new Color("Chocolate", "#D2691E"));

        dict.Add("Coral", new Color("Coral", "#FF7F50"));

        dict.Add("CornflowerBlue", new Color("CornflowerBlue", "#6495ED"));

        dict.Add("Cornsilk", new Color("Cornsilk", "#FFF8DC"));

        dict.Add("Crimson", new Color("Crimson", "#DC143C"));

        dict.Add("Cyan", new Color("Cyan", "#00FFFF"));

        dict.Add("DarkBlue", new Color("DarkBlue", "#00008B"));

        dict.Add("DarkCyan", new Color("DarkCyan", "#008B8B"));

        dict.Add("DarkGoldenRod", new Color("DarkGoldenRod", "#B8860B"));

        dict.Add("DarkGray", new Color("DarkGray", "#A9A9A9"));

        dict.Add("DarkGrey", new Color("DarkGrey", "#A9A9A9"));

        dict.Add("DarkGreen", new Color("DarkGreen", "#006400"));

        dict.Add("DarkKhaki", new Color("DarkKhaki", "#BDB76B"));

        dict.Add("DarkMagenta", new Color("DarkMagenta", "#8B008B"));

        dict.Add("DarkOliveGreen", new Color("DarkOliveGreen", "#556B2F"));

        dict.Add("DarkOrange", new Color("DarkOrange", "#FF8C00"));

        dict.Add("DarkOrchid", new Color("DarkOrchid", "#9932CC"));

        dict.Add("DarkRed", new Color("DarkRed", "#8B0000"));

        dict.Add("DarkSalmon", new Color("DarkSalmon", "#E9967A"));

        dict.Add("DarkSeaGreen", new Color("DarkSeaGreen", "#8FBC8F"));

        dict.Add("DarkSlateBlue", new Color("DarkSlateBlue", "#483D8B"));

        dict.Add("DarkSlateGray", new Color("DarkSlateGray", "#2F4F4F"));

        dict.Add("DarkSlateGrey", new Color("DarkSlateGrey", "#2F4F4F"));

        dict.Add("DarkTurquoise", new Color("DarkTurquoise", "#00CED1"));

        dict.Add("DarkViolet", new Color("DarkViolet", "#9400D3"));

        dict.Add("DeepPink", new Color("DeepPink", "#FF1493"));

        dict.Add("DeepSkyBlue", new Color("DeepSkyBlue", "#00BFFF"));

        dict.Add("DimGray", new Color("DimGray", "#696969"));

        dict.Add("DimGrey", new Color("DimGrey", "#696969"));

        dict.Add("DodgerBlue", new Color("DodgerBlue", "#1E90FF"));

        dict.Add("FireBrick", new Color("FireBrick", "#B22222"));

        dict.Add("FloralWhite", new Color("FloralWhite", "#FFFAF0"));

        dict.Add("ForestGreen", new Color("ForestGreen", "#228B22"));

        dict.Add("Fuchsia", new Color("Fuchsia", "#FF00FF"));

        dict.Add("Gainsboro", new Color("Gainsboro", "#DCDCDC"));

        dict.Add("GhostWhite", new Color("GhostWhite", "#F8F8FF"));

        dict.Add("Gold", new Color("Gold", "#FFD700"));

        dict.Add("GoldenRod", new Color("GoldenRod", "#DAA520"));

        dict.Add("Gray", new Color("Gray", "#808080"));

        dict.Add("Grey", new Color("Grey", "#808080"));

        dict.Add("Green", new Color("Green", "#008000"));

        dict.Add("GreenYellow", new Color("GreenYellow", "#ADFF2F"));

        dict.Add("HoneyDew", new Color("HoneyDew", "#F0FFF0"));

        dict.Add("HotPink", new Color("HotPink", "#FF69B4"));

        dict.Add("IndianRed", new Color("IndianRed", "#CD5C5C"));

        dict.Add("Indigo", new Color("Indigo", "#4B0082"));

        dict.Add("Ivory", new Color("Ivory", "#FFFFF0"));

        dict.Add("Khaki", new Color("Khaki", "#F0E68C"));

        dict.Add("Lavender", new Color("Lavender", "#E6E6FA"));

        dict.Add("LavenderBlush", new Color("LavenderBlush", "#FFF0F5"));

        dict.Add("LawnGreen", new Color("LawnGreen", "#7CFC00"));

        dict.Add("LemonChiffon", new Color("LemonChiffon", "#FFFACD"));

        dict.Add("LightBlue", new Color("LightBlue", "#ADD8E6"));

        dict.Add("LightCoral", new Color("LightCoral", "#F08080"));

        dict.Add("LightCyan", new Color("LightCyan", "#E0FFFF"));

        dict.Add("LightGoldenRodYellow", new Color("LightGoldenRodYellow", "#FAFAD2"));

        dict.Add("LightGray", new Color("LightGray", "#D3D3D3"));

        dict.Add("LightGrey", new Color("LightGrey", "#D3D3D3"));

        dict.Add("LightGreen", new Color("LightGreen", "#90EE90"));

        dict.Add("LightPink", new Color("LightPink", "#FFB6C1"));

        dict.Add("LightSalmon", new Color("LightSalmon", "#FFA07A"));

        dict.Add("LightSeaGreen", new Color("LightSeaGreen", "#20B2AA"));

        dict.Add("LightSkyBlue", new Color("LightSkyBlue", "#87CEFA"));

        dict.Add("LightSlateGray", new Color("LightSlateGray", "#778899"));

        dict.Add("LightSlateGrey", new Color("LightSlateGrey", "#778899"));

        dict.Add("LightSteelBlue", new Color("LightSteelBlue", "#B0C4DE"));

        dict.Add("LightYellow", new Color("LightYellow", "#FFFFE0"));

        dict.Add("Lime", new Color("Lime", "#00FF00"));

        dict.Add("LimeGreen", new Color("LimeGreen", "#32CD32"));

        dict.Add("Linen", new Color("Linen", "#FAF0E6"));

        dict.Add("Magenta", new Color("Magenta", "#FF00FF"));

        dict.Add("Maroon", new Color("Maroon", "#800000"));

        dict.Add("MediumAquaMarine", new Color("MediumAquaMarine", "#66CDAA"));

        dict.Add("MediumBlue", new Color("MediumBlue", "#0000CD"));

        dict.Add("MediumOrchid", new Color("MediumOrchid", "#BA55D3"));

        dict.Add("MediumPurple", new Color("MediumPurple", "#9370DB"));

        dict.Add("MediumSeaGreen", new Color("MediumSeaGreen", "#3CB371"));

        dict.Add("MediumSlateBlue", new Color("MediumSlateBlue", "#7B68EE"));

        dict.Add("MediumSpringGreen", new Color("MediumSpringGreen", "#00FA9A"));

        dict.Add("MediumTurquoise", new Color("MediumTurquoise", "#48D1CC"));

        dict.Add("MediumVioletRed", new Color("MediumVioletRed", "#C71585"));

        dict.Add("MidnightBlue", new Color("MidnightBlue", "#191970"));

        dict.Add("MintCream", new Color("MintCream", "#F5FFFA"));

        dict.Add("MistyRose", new Color("MistyRose", "#FFE4E1"));

        dict.Add("Moccasin", new Color("Moccasin", "#FFE4B5"));

        dict.Add("NavajoWhite", new Color("NavajoWhite", "#FFDEAD"));

        dict.Add("Navy", new Color("Navy", "#000080"));

        dict.Add("OldLace", new Color("OldLace", "#FDF5E6"));

        dict.Add("Olive", new Color("Olive", "#808000"));

        dict.Add("OliveDrab", new Color("OliveDrab", "#6B8E23"));

        dict.Add("Orange", new Color("Orange", "#FFA500"));

        dict.Add("OrangeRed", new Color("OrangeRed", "#FF4500"));

        dict.Add("Orchid", new Color("Orchid", "#DA70D6"));

        dict.Add("PaleGoldenRod", new Color("PaleGoldenRod", "#EEE8AA"));

        dict.Add("PaleGreen", new Color("PaleGreen", "#98FB98"));

        dict.Add("PaleTurquoise", new Color("PaleTurquoise", "#AFEEEE"));

        dict.Add("PaleVioletRed", new Color("PaleVioletRed", "#DB7093"));

        dict.Add("PapayaWhip", new Color("PapayaWhip", "#FFEFD5"));

        dict.Add("PeachPuff", new Color("PeachPuff", "#FFDAB9"));

        dict.Add("Peru", new Color("Peru", "#CD853F"));

        dict.Add("Pink", new Color("Pink", "#FFC0CB"));

        dict.Add("Plum", new Color("Plum", "#DDA0DD"));

        dict.Add("PowderBlue", new Color("PowderBlue", "#B0E0E6"));

        dict.Add("Purple", new Color("Purple", "#800080"));

        dict.Add("RebeccaPurple", new Color("RebeccaPurple", "#663399"));

        dict.Add("Red", new Color("Red", "#FF0000"));

        dict.Add("RosyBrown", new Color("RosyBrown", "#BC8F8F"));

        dict.Add("RoyalBlue", new Color("RoyalBlue", "#4169E1"));

        dict.Add("SaddleBrown", new Color("SaddleBrown", "#8B4513"));

        dict.Add("Salmon", new Color("Salmon", "#FA8072"));

        dict.Add("SandyBrown", new Color("SandyBrown", "#F4A460"));

        dict.Add("SeaGreen", new Color("SeaGreen", "#2E8B57"));

        dict.Add("SeaShell", new Color("SeaShell", "#FFF5EE"));

        dict.Add("Sienna", new Color("Sienna", "#A0522D"));

        dict.Add("Silver", new Color("Silver", "#C0C0C0"));

        dict.Add("SkyBlue", new Color("SkyBlue", "#87CEEB"));

        dict.Add("SlateBlue", new Color("SlateBlue", "#6A5ACD"));

        dict.Add("SlateGray", new Color("SlateGray", "#708090"));

        dict.Add("SlateGrey", new Color("SlateGrey", "#708090"));

        dict.Add("Snow", new Color("Snow", "#FFFAFA"));

        dict.Add("SpringGreen", new Color("SpringGreen", "#00FF7F"));

        dict.Add("SteelBlue", new Color("SteelBlue", "#4682B4"));

        dict.Add("Tan", new Color("Tan", "#D2B48C"));

        dict.Add("Teal", new Color("Teal", "#008080"));

        dict.Add("Thistle", new Color("Thistle", "#D8BFD8"));

        dict.Add("Tomato", new Color("Tomato", "#FF6347"));

        dict.Add("Turquoise", new Color("Turquoise", "#40E0D0"));

        dict.Add("Violet", new Color("Violet", "#EE82EE"));

        dict.Add("Wheat", new Color("Wheat", "#F5DEB3"));

        dict.Add("White", new Color("White", "#FFFFFF"));

        dict.Add("WhiteSmoke", new Color("WhiteSmoke", "#F5F5F5"));

        dict.Add("Yellow", new Color("Yellow", "#FFFF00"));

        dict.Add("YellowGreen", new Color("YellowGreen", "#9ACD32"));

    }

    public string Name { get; set; }

    public string Code { get; set; }

    public byte R { get; set; }

    public byte G { get; set; }

    public byte B { get; set; }

    public byte A { get; set; }
}
