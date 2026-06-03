namespace ExtraDry.Core.Formatters;

/// <summary>
/// Represents a roundtrip mechanism for formatting a value to a string for user editing or
/// display. Implementations are used by input fields (round-trip) and column formatters
/// (display-only, via <see cref="TableColumnAttribute" />).
/// </summary>
public interface IValueFormatter
{
    /// <summary>
    /// A regular expression to apply to the string representation to show validation issues to users.
    /// </summary>
    public string RegexPattern { get; set; }

    /// <summary>
    /// The format that is to be used for formatting, will have sensible defaults based on type and
    /// can be overridden for custom formats, e.g. not using commas or different number of decimals.
    /// </summary>
    public string DataFormat { get; set; }

    /// <summary>
    /// Given an object value, output the string representation to be shown to users.
    /// </summary>
    public string Format(object? value);

    /// <summary>
    /// Given a string entered by a user, attempt to parse it into a specific value.
    /// </summary>
    public bool TryParse(string? value, out object? result);
}
