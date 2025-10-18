namespace ExtraDry.Blazor.Components.Formatting;

/// <summary>
/// Represents a roundtrip mechanism for formatting a String to a string for user editing.
/// Used when no formatting or parsing is required.
/// </summary>
public class IdentityFormatter : INumberFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @".*";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value == null) {
            return "";
        }
        return value.ToString() ?? "";
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        result = value;
        return true;
    }
}
