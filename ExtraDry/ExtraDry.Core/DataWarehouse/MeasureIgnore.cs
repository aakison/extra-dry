namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// Prevents a property from being a data warehouse measure of a fact table. Overrides [Measure] if
/// both are included on the same property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MeasureIgnoreAttribute : Attribute
{
    /// <summary>
    /// Declares a property should not be a data warehouse measure.
    /// </summary>
    public MeasureIgnoreAttribute()
    { }
}
