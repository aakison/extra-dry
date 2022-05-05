namespace ExtraDry.Core.DataWarehouse;

/// <summary>
/// A data warehouse column is an attribute of a dimension table.
/// </summary>
/// <remarks>
/// Not named 'attribute' as this is too closely overloaded with C# attributes, this class would have been called "AttributeAttribute"!
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class ColumnAttribute : Attribute {

    public ColumnAttribute() { }

    public ColumnAttribute(string name)
    {
        Name = name;
    }

    public string? Name { get; set; }
}
