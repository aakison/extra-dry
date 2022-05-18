namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse dimension table.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false)]
public class DimensionTableAttribute : Attribute {

    /// <summary>
    /// Declares a `class` or `enum` as a data warehouse dimension table with the name inferred from the `class` or `enum` name.
    /// </summary>
    public DimensionTableAttribute() { }

    /// <summary>
    /// Declares a `class` or `enum` as a data warehouse dimension table with the name explicity defined.
    /// </summary>
    public DimensionTableAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name to use for the data warehouse table.  
    /// If `null`, then the name is inferred from the `class` or `enum` name.
    /// </summary>
    public string? Name { get; set; }

}
