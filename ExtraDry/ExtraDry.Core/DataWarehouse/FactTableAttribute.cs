namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse fact table.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class FactTableAttribute : Attribute {

    /// <summary>
    /// Declares a `class` as a data warehouse fact table with the name inferred from the `class` name.
    /// </summary>
    public FactTableAttribute() { }

    /// <summary>
    /// Declares a `class` as a data warehouse fact table with the name explicity defined.
    /// </summary>
    public FactTableAttribute(string name) { 
        Name = name;
    }

    /// <summary>
    /// The name to use for the data warehouse table.  
    /// If `null`, then the name is inferred from the `class` name.
    /// </summary>
    public string? Name { get; set; }

}
