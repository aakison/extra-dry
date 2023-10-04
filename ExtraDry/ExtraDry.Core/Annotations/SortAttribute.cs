namespace ExtraDry.Core;

/// <summary>
/// Defines a property that can be used with `SortQuery` and the `PartialQueryable` extensions 
/// to `IQueryable`.  The default for database properties is sortable, this attribute typically
/// only suppresses sorting.  However, some properties (e.g. UUIDs) are not sortable by default and
/// this can override that behavior.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SortAttribute : Attribute {

    /// <inheritdoc cref="SortAttribute" />
    public SortAttribute(SortType type)
    {
        Type = type;
    }

    /// <summary>
    /// Indicates the type of the sort that can be applied to this property.
    /// </summary>
    public SortType Type { get; set; }

}
