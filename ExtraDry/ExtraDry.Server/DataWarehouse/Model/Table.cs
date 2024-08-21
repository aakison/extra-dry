using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// Represents a table in the Data Warehouse, tables will match to some source data in the system.  E.g.
///   * Enums decorated with [DimensionTable] will create static dimension tables
///   * DbSet`T properties in the DbContext will create tables if `T is decorated with [DimensionTable] or [FactTable]
/// Multiple tables might match back to the same source, such as fact and dimensions tables for the same entity.
/// </summary>
public class Table {

    public Table(Type type, string name)
    {
        EntityType = type;
        Name = name;
    }

    /// <summary>
    /// The underlying data type for the source.
    /// </summary>
    [JsonIgnore]
    public Type EntityType { get; private init; }

    /// <summary>
    /// The name of the table as it will appear in the data warehouse.
    /// This is constructed from the property name of the DbSet`T or as overridden.
    /// </summary>
    public string Name { get; private init; }

    /// <summary>
    /// The set of columns for this table.
    /// </summary>
    public List<Column> Columns { get; } = [];

    public IDataGenerator? Generator { get; init; }

    /// <summary>
    /// If the table is linked to a DbSet`T on the source DbContext, the PropertyInfo for that property.
    /// </summary>
    [JsonIgnore]
    public PropertyInfo? SourceProperty { get; internal init; }

    public string SourcePropertyName => $"{SourceProperty?.Name}";

    /// <summary>
    /// The base data, if any, for this table.
    /// Enums will create all of there data here and won't change while the program is running.
    /// Dimensions and facts will typically have no base data.
    /// </summary>
    public List<Dictionary<string, object>> Data { get; } = [];

    /// <summary>
    /// An accessor to easily fetch the key column from the list of columns.
    /// </summary>
    public Column KeyColumn => Columns.First(e => e.ColumnType == ColumnType.Key);

    /// <summary>
    /// An accessor to easily fetch all non-key columns.
    /// </summary>
    public IEnumerable<Column> ValueColumns => Columns.Where(e => e.ColumnType != ColumnType.Key);

}
