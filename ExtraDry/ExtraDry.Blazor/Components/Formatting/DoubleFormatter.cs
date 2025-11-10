namespace ExtraDry.Blazor.Components.Formatting;

/// <summary>
/// Represents a roundtrip mechanism for formatting a Double to a string for user editing.
/// </summary>
public class DoubleFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,10})?";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "#,#.##";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "0";
        }
        var val = (double)value;
        var formatted = val == 0 ? "0" : val.ToString(DataFormat, CultureInfo.CurrentCulture);
        return formatted;
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            result = 0.0;
            return true;
        }
        value = value.Replace(",", "");
        if(double.TryParse(value, out var doubleResult)) {
            result = doubleResult;
            return true;
        }
        else {
            result = 0.0;
            return false;
        }
    }
}
