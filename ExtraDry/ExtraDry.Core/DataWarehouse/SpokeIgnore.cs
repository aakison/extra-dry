namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// Prevents a property from being a data warehouse spoke targetting a dimension table. Overrides
/// [Spoke] if both are included on the same property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SpokeIgnoreAttribute : Attribute
{
    /// <summary>
    /// Declares a property should not be a data warehouse spoke.
    /// </summary>
    public SpokeIgnoreAttribute()
    { }
}
