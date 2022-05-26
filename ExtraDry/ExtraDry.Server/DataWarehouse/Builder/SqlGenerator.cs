namespace ExtraDry.Server.DataWarehouse.Builder;

internal static class SqlGenerator {

    public static string Generate(WarehouseModel warehouse) =>
        string.Join("\n", warehouse.Dimensions.Union(warehouse.Facts).Select(e => SqlTable(e))) +
        string.Join("\n", warehouse.Dimensions.Union(warehouse.Facts).Select(e => SqlData(e)));

    internal static string SqlTable(Table table) =>
        $"CREATE TABLE [{table.Name}] (\n    {SqlColumns(table.Columns)}\n    {SqlConstraints(table)}\n)\nGO\n";

    private static string SqlConstraints(Table table) =>
        string.Join(",\n    ", table.Columns.Where(e => e.Reference != null).Select(e => SqlFKConstraint(table, e)));

    private static string SqlFKConstraint(Table table, Column column) =>
        $"CONSTRAINT [FK_{table.EntityType.Name}_{column.PropertyInfo!.Name}] FOREIGN KEY ([{column.Name}]) REFERENCES [{column.Reference!.Table}]([{column.Reference!.Column}])";

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

    internal static string SqlData(Table table) => !table.Data.Any() ? "" :
        $"{SqlInsertInto(table)}";

    private static string SqlInsertInto(Table table) =>
        $"INSERT INTO [{table.Name}] \n    ({SqlInsertColumnNames(table)})\nVALUES\n    {SqlInsertColumnValues(table)}\nGO\n";

    private static string SqlInsertColumnNames(Table table) =>
        string.Join(", ", table.Columns.Select(e => $"[{e.Name}]"));

    private static string SqlInsertColumnValues(Table table) =>
        string.Join(",\n    ", table.Data.Select(e => SqlInsertColumnValues(table, e)));

    private static string SqlInsertColumnValues(Table table, Dictionary<string, object> values) =>
        $"({SqlInsertColumnValues(table.Columns.Select(e => values[e.Name]))})";

    private static string SqlInsertColumnValues(IEnumerable<object> values) =>
        string.Join(", ", values.Select(e => SqlInsertColumnValue(e)));

    private static string SqlInsertColumnValue(object value)
    {
        if(value is string sValue) {
            var escapedValue = sValue.Replace("'", "''");
            return $"'{escapedValue}'";
        }
        else {
            return value.ToString()!;
        }
    }

}
