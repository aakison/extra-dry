namespace ExtraDry.Core;

/// <summary>
/// A display-only formatter that renders a <see cref="DateTime" /> or nullable
/// <see cref="DateTime" /> as a human-readable relative time string (e.g. "2 hours ago",
/// "Yesterday"). Parsing is not supported — <see cref="TryParse" /> returns the original value
/// unchanged.
/// </summary>
/// <remarks>
/// Intended for use with <c>[ColumnFormatter(typeof(RelativeTimeFormatter))]</c> on model
/// properties that should show relative time in table columns.
/// </remarks>
public class RelativeTimeFormatter : IValueFormatter
{
    /// <inheritdoc />
    public string RegexPattern { get; set; } = @".*";

    /// <inheritdoc />
    public string DataFormat { get; set; } = "";

    /// <inheritdoc />
    public string Format(object? value)
    {
        if(value is DateTime dateTime) {
            return DataConverter.DateToRelativeTime(dateTime);
        }
        return "";
    }

    /// <inheritdoc />
    public bool TryParse(string? value, out object? result)
    {
        result = value;
        return true;
    }
}
