namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// Prevents a property from being a data warehouse attribute of a dimension table. Overrides
/// [Attribute] if both are included on the same property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class AttributeIgnoreAttribute : Attribute
{
    /// <summary>
    /// Declares a property should not be a data warehouse attribute.
    /// </summary>
    public AttributeIgnoreAttribute()
    { }
}
