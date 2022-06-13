namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse fact table.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FactTableAttribute : WarehouseTableAttribute {

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

}
