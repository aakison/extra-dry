namespace ExtraDry.Server.DataWarehouse.Builder;

public class SqlServerSqlGenerator : ISqlGenerator {

    public bool IncludeConstraints { get; set; } = false;

    public string Generate(WarehouseModel warehouse) =>
        string.Join("\nGO\n", warehouse.Dimensions.Union(warehouse.Facts).Select(e => CreateTable(e))) +
        "\nGO\n" + 
        string.Join("\nGO\n", warehouse.Dimensions.Union(warehouse.Facts).Select(e => InsertData(e)));

    /// <summary>
    /// Generate the SQL to create a table.
    /// When overloading, avoid foreign key constraints as they cause performance bottlenecks in data warehouses.
    /// </summary>
    public string CreateTable(Table table) =>
        $"CREATE TABLE [{table.Name}] (\n    {SqlColumns(table.Columns)}\n    {SqlConstraints(table)}\n)\n";

    public string DropTable(Table table) =>
        $"DROP TABLE IF EXISTS [{table.Name}]\n";

    public string InsertData(Table table) => !table.Data.Any() ? "" :
        $"{SqlInsertInto(table)}";

    public string Upsert(Table table, int keyValue, Dictionary<string, object> values)
    {
        var keyName = table.KeyColumn.Name;
        return @$"MERGE INTO [{table.Name}] AS [Target]
USING (SELECT {keyValue}) AS [Source]([Id])
    ON [Target].[{table.KeyColumn.Name}] = [Source].[Id]
WHEN MATCHED THEN
    UPDATE SET {UpdateExpressions()}
WHEN NOT MATCHED THEN
    INSERT ([{keyName}], {InsertColumns()})
    VALUES ({keyValue}, {InsertValues()})
;
";

        string UpdateExpressions() => string.Join(", ", table.ValueColumns.Select(e => $"[{e.Name}] = {SqlQuotedValue(values[e.Name])}"));

        string InsertColumns() => string.Join(", ", table.ValueColumns.Select(e => $"[{e.Name}]"));

        string InsertValues() => string.Join(", ", table.ValueColumns.Select(e => $"{SqlQuotedValue(values[e.Name])}"));
    }

    public string SelectMinimum(Table table, string column) =>
        $"SELECT Min([{column}]) AS [Min {column}] FROM [{table.Name}]";

    public string SelectMaximum(Table table, string column) =>
        $"SELECT Max([{column}]) AS [Max {column}] FROM [{table.Name}]";

    private string SqlConstraints(Table table) =>
        string.Join(",\n    ", table.Columns.Where(e => e.Reference != null).Select(e => SqlFKConstraint(table, e)));

    private string SqlFKConstraint(Table table, Column column) =>
        IncludeConstraints ? $"CONSTRAINT [FK_{table.EntityType.Name}_{column.PropertyInfo!.Name}] FOREIGN KEY ([{column.Name}]) REFERENCES [{column.Reference!.Table}]([{column.Reference!.Column}])" : "";

    private static string SqlColumns(IEnumerable<Column> columns) =>
        string.Join(",\n    ", columns.Select(e => SqlColumn(e)));

    private static string SqlColumn(Column column) =>
        $"[{column.Name}] {SqlColumnType(column)}";

    private static string SqlColumnType(Column column) =>
        (column.ColumnType, column.Nullable) switch {
            (ColumnType.Key, _) => "INT NOT NULL PRIMARY KEY",
            (ColumnType.Double, false) => "REAL NOT NULL",
            (ColumnType.Double, true) => "REAL",
            (ColumnType.Integer, false) => "INT NOT NULL",
            (ColumnType.Integer, true) => "INT",
            (ColumnType.Decimal, false) => $"DECIMAL({column.Precision}) NOT NULL",
            (ColumnType.Decimal, true) => $"DECIMAL({column.Precision})",
            (ColumnType.Date, false) => "DATE NOT NULL",
            (ColumnType.Date, true) => "DATE",
            (_, false) => $"{SqlVarchar(column.Length)} NOT NULL",
            (_, _) => SqlVarchar(column.Length),
        };

    private static string SqlVarchar(int? length)
    {
        if(length == null || length > 8000 || length <= 0) {
            return "NVARCHAR(Max)";
        }
        else {
            return $"NVARCHAR({length})";
        }
    }

    private static string SqlInsertInto(Table table) =>
        $"INSERT INTO [{table.Name}] \n    ({SqlInsertColumnNames(table)})\nVALUES\n    {SqlInsertColumnValues(table)}\n";

    private static string SqlInsertColumnNames(Table table) =>
        string.Join(", ", table.Columns.Select(e => $"[{e.Name}]"));

    private static string SqlInsertColumnValues(Table table) =>
        string.Join(",\n    ", table.Data.Select(e => SqlInsertColumnValues(table, e)));

    private static string SqlInsertColumnValues(Table table, Dictionary<string, object> values) =>
        $"({SqlInsertColumnValues(table.Columns.Select(e => values[e.Name]))})";

    private static string SqlInsertColumnValues(IEnumerable<object> values) =>
        string.Join(", ", values.Select(e => SqlQuotedValue(e)));

    private static string SqlQuotedValue(object value)
    {
        if(value is string sValue) {
            var escapedValue = sValue.Replace("'", "''");
            return $"'{escapedValue}'";
        }
        else if(value is Enum eValue) {
            return $"'{DataConverter.DisplayEnum(eValue)}'";
        }
        else if(value is Guid gValue) {
            return $"'{gValue}'";
        }
        else if(value is DateOnly dateValue) {
            return $"'{dateValue}'";
        }
        else {
            return value.ToString()!;
        }
    }

}
