namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A filter builder used by the PageQueryBuilder that supports select lists of enums.
/// </summary>
public class EnumFilterBuilder : FilterBuilder
{
    /// <summary>
    /// The list of string values with the names of the enums that have been selected.
    /// </summary>
    public List<string> Values { get; } = [];

    /// <inheritdoc cref="FilterBuilder.Build" />
    public override string Build() => Values.Count != 0 ? $"{FilterName}:{QuotedValues}" : "";

    /// <inheritdoc cref="FilterBuilder.Reset" />
    public override void Reset() => Values.Clear();

    private string QuotedValues => string.Join('|', Values.Where(e => !string.IsNullOrWhiteSpace(e)).Select(QuotedValue));

    private string QuotedValue(string value) => value.Contains(' ') || value.Contains('|') ? $"\"{value}\"" : value;
}
