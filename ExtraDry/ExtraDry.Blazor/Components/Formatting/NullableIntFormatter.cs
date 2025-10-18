namespace ExtraDry.Blazor.Components.Formatting;

/// <summary>
/// Represents a roundtrip mechanism for formatting a Int? to a string for user editing.
/// </summary>
public class NullableIntFormatter : INumberFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,9}";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "#,#";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        var val = (int)value;
        var formatted = val == 0 ? "0" : val.ToString(DataFormat, CultureInfo.CurrentCulture);
        return formatted;
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = null;
            return true;
        }
        value = value.Replace(",", "");
        if(int.TryParse(value, out var intResult)) {
            result = intResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }
}
