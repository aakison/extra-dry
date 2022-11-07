namespace ExtraDry.Core.DataWarehouse;

public abstract class WarehouseTableAttribute : Attribute {

    /// <summary>
    /// The name to use for the data warehouse table.  
    /// If `null`, then the name is inferred from the `class` or `enum` name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// When partitioning data warehouses, defines the group for this fact table.
    /// May use comma-separated list of groups.  By default will match all groups.
    /// </summary>
    public string? Group { get; set; }

    public bool MatchesGroup(string? group) => 
        (group == null && Group == null) ||
        (Group?.Split(',')?.Any(e => string.Equals(e, group, StringComparison.OrdinalIgnoreCase)) ?? true);

}
