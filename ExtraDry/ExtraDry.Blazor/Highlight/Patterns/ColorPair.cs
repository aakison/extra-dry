namespace ExtraDry.Highlight;

public class ColorPair
{
    public Color ForeColor { get; set; }

    public Color BackColor { get; set; }

    public ColorPair(Color foreColor, Color backColor)
    {
        ForeColor = foreColor;
        BackColor = backColor;
    }
}
