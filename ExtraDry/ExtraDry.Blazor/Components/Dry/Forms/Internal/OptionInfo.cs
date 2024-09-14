namespace ExtraDry.Blazor.Forms;

internal class OptionInfo(
    string key, 
    string display, 
    object? value)
{
    public string Key { get; set; } = key;

    public string Display { get; set; } = display;

    public bool Selected { get; set; }

    public object? Value { get; set; } = value;
}
