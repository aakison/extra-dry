namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse measure of a fact table.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class MeasureAttribute : Attribute
{

    /// <summary>
    /// Declares a property as a data warehouse measure with the name inferred from the property name.
    /// </summary>
    public MeasureAttribute() { }

    /// <summary>
    /// Declares a property as a data warehouse measure with the name explicity defined.
    /// </summary>
    public MeasureAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name to use for the data warehouse measure column.  
    /// If `null`, then the name is inferred from the property's name.
    /// </summary>
    public string? Name { get; set; }

}
