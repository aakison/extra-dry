namespace ExtraDry.Server.DataWarehouse.Builder;

/// <summary>
/// Interface for data warehouse system to generate SQL from data warehouse concepts.
/// </summary>
public interface ISqlGenerator {

    /// <summary>
    /// Generate the SQL to create a table.
    /// When overloading, avoid foreign key constraints as they cause performance bottlenecks in data warehouses.
    /// </summary>
    string CreateTable(Table table);

    /// <summary>
    /// Generate the SQL to drop a table, along with any necessary constraints.
    /// </summary>
    string DropTable(Table table);

    /// <summary>
    /// Generate the SQL to insert the base data that is associated with a table, for example, when the table represents an enum.
    /// </summary>
    string InsertData(Table table);

    /// <summary>
    /// Generate the SQL to find the smallest key (i.e. ID) in the table.
    /// </summary>
    string SelectMaximumKey(Table table);

    /// <summary>
    /// Generate the SQL to find the largest key (i.e. ID) in the table.
    /// </summary>
    string SelectMinimumKey(Table table);

    /// <summary>
    /// Generate the SQL for an Upsert statement for the given table and row, with the values provided.
    /// </summary>
    string Upsert(Table table, int keyValue, Dictionary<string, object> values);

}
