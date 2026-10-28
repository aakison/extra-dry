namespace ExtraDry.Core;

/// <summary>
/// Defines a property that can be used with <see cref="FilterQuery" /> and the extensions to <see
/// cref="IQueryable" /> that construct filtered queries. The default behavior for properties is
/// that they are not filtereable unless this attribute is applied.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterAttribute(FilterType type = FilterType.Equals) : Attribute
{
    /// <summary>
    /// The name to use for the filter, if not provided the name of the property will be used.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional dot-separated path of properties, relative to the declaring type, that the filter
    /// is applied to when querying. Use for properties that are surfaced from a joined entity,
    /// e.g. `Contact.Email`. If not provided, the property itself is used.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Indicates the type of the filter when searching for items. Only applies to string
    /// properties.
    /// </summary>
    public FilterType Type { get; set; } = type;
}
