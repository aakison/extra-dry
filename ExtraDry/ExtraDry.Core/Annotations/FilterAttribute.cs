namespace ExtraDry.Core;

/// <summary>
/// Defines a property that can be used with `FilterQuery` and the `PartialQueryable` extensions 
/// to `IQueryable`.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterAttribute : Attribute {

    /// <inheritdoc cref="FilterAttribute" />
    public FilterAttribute(FilterType type = FilterType.Equals)
    {
        Type = type;
    }

    /// <summary>
    /// Indicates the type of the filter when searching for items. Only applies to 
    /// string properties.
    /// </summary>
    public FilterType Type { get; set; }

}
