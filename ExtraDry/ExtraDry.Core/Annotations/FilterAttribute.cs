namespace ExtraDry.Core;

/// <summary>
/// Defines a property that can be used with <see cref="FilterQuery"/> and the extenisions to 
/// <see cref="IQueryable"/> that construct filtered queries.  The default behavior for 
/// properties is that they are not filtereable unless this attribute is applied.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterAttribute : Attribute
{

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
