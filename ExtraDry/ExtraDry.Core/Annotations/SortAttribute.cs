namespace ExtraDry.Core;

/// <summary>
/// Defines a property that can be used with <see cref="SortQuery" /> and the <see
/// cref="IQueryable" /> extensions. The default behavior for properties is that they are sortable,
/// this attribute typically only suppresses sorting. However, some properties (e.g.
/// UUIDs) are not sortable by default and this can override that behavior.
/// </summary>
/// <inheritdoc cref="SortAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class SortAttribute(SortType type) : Attribute
{
    /// <summary>
    /// Indicates the type of the sort that can be applied to this property.
    /// </summary>
    public SortType Type { get; } = type;
}
