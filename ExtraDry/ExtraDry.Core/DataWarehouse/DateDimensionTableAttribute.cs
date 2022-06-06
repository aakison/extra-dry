namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse date dimension table.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false)]
public class DateDimensionTableAttribute : Attribute {

    /// <summary>
    /// Declares a `class` as a data warehouse date dimension table with the name inferred from the `class` name.
    /// </summary>
    public DateDimensionTableAttribute() { }

    /// <summary>
    /// Declares a `class` as a data warehouse date dimension table with the name explicity defined.
    /// </summary>
    public DateDimensionTableAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name to use for the data warehouse table.  
    /// If `null`, then the name is inferred from the `class` name.
    /// </summary>
    public string? Name { get; set; }

}
