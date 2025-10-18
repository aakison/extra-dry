namespace ExtraDry.Blazor.Components.Formatting;

/// <summary>
/// Represents a roundtrip mechanism for formatting a number to a string for user editing.
/// </summary>
public interface INumberFormatter
{
    /// <summary>
    /// A regular expression to apply to the string representation to show validation issues to users.
    /// </summary>
    public string RegexPattern { get; set; }

    /// <summary>
    /// The format that is to be use for formatting, will have sensible defaults based on type and 
    /// can be overridden for custom formats, e.g. not using commas or different number of decimals.
    /// </summary>
    public string DataFormat { get; set; }

    /// <summary>
    /// Given a numeric object, output the string representation to be shown to users.
    /// </summary>
    public string Format(object? value);

    /// <summary>
    /// Given a string entered by a user, attempt to parse it into a specific value.
    /// </summary>
    public bool TryParse(string? value, out object? result);

}
