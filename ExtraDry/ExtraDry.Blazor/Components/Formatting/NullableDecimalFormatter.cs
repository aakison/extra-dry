namespace ExtraDry.Blazor.Components.Formatting;

/// <summary>
/// Represents a roundtrip mechanism for formatting a Decimal? to a string for user editing.
/// </summary>
public class NullableDecimalFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @"-?[0-9]{0,10}(\.[0-9]{0,2})?";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "#,0.00";

    /// <summary>
    /// Value to return when the value is null. Default is "empty", but could be set to "null" or something else if desired.
    /// </summary>
    public string NullFormat { get; set; } = "empty";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return NullFormat;
        }
        var val = (decimal)value;
        var formatted = val == 0 ? "0.00" : val.ToString(DataFormat, CultureInfo.CurrentCulture);
        return formatted;
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        if(string.IsNullOrWhiteSpace(value) || value == NullFormat) {
            result = null;
            return true;
        }
        value = value.Replace(",", "");
        if(decimal.TryParse(value, out var intResult)) {
            result = intResult;
            return true;
        }
        else {
            result = null;
            return false;
        }
    }
}
