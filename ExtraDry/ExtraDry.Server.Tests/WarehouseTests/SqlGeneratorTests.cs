using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class SqlGeneratorTests {

    [Fact]
    public void ColumnTableSyntax()
    {
        var table = new Table(this.GetType(), "Test");

        var sql = new SqlServerSqlGenerator().CreateTable(table);

        Assert.Contains("CREATE TABLE [Test] (", sql);
    }

    [Theory]
    [InlineData(ColumnType.Integer, "Foo", false, "[Foo] INT NOT NULL,")]
    [InlineData(ColumnType.Integer, "Foo", true, "[Foo] INT,")]
    [InlineData(ColumnType.Decimal, "Foo", false, "[Foo] DECIMAL(18,2) NOT NULL,")]
    [InlineData(ColumnType.Decimal, "Foo", true, "[Foo] DECIMAL(18,2),")]
    [InlineData(ColumnType.Text, "Foo", false, "[Foo] NVARCHAR(Max) NOT NULL,")]
    [InlineData(ColumnType.Text, "Foo", true, "[Foo] NVARCHAR(Max),")]
    [InlineData(ColumnType.Key, "Foo", false, "[Foo] INT NOT NULL PRIMARY KEY,")] // nullable is ignored.
    [InlineData(ColumnType.Key, "Foo", true, "[Foo] INT NOT NULL PRIMARY KEY,")]
    [InlineData(ColumnType.Double, "Foo", false, "[Foo] REAL NOT NULL,")]
    [InlineData(ColumnType.Double, "Foo", true, "[Foo] REAL,")]
    [InlineData(ColumnType.Integer, "Multi Part Name", true, "[Multi Part Name] INT,")]
    [InlineData(ColumnType.Integer, "Property Formerly Known '!~@#%", true, "[Property Formerly Known '!~@#%] INT,")]
    public void ColumnTypeIsCorrect(ColumnType type, string name, bool nullable, string expected)
    {
        var table = new Table(this.GetType(), "Test");

        table.Columns.Add(new Column(type, name) {
            Nullable = nullable,
        });
        table.Columns.Add(new Column(ColumnType.Integer, "Second"));  // force comma at end of first line.

        var sql = new SqlServerSqlGenerator().CreateTable(table);
        Assert.Contains(expected, sql);
    }

    [Theory]
    [InlineData(ColumnType.Integer, "Foo", 12, "[Foo] INT NOT NULL,")] // ignore length
    [InlineData(ColumnType.Decimal, "Foo", 12, "[Foo] DECIMAL(18,2) NOT NULL,")] // ignore length
    [InlineData(ColumnType.Double, "Foo", 12, "[Foo] REAL NOT NULL,")] // ignore length
    [InlineData(ColumnType.Text, "Foo", 12, "[Foo] NVARCHAR(12) NOT NULL,")]
    [InlineData(ColumnType.Text, "Foo", 8000, "[Foo] NVARCHAR(8000) NOT NULL,")]
    [InlineData(ColumnType.Text, "Foo", 9000, "[Foo] NVARCHAR(Max) NOT NULL,")] // To big for column, wrap to max.
    public void ColumnLengthsAreCorrect(ColumnType type, string name, int length, string expected)
    {
        var table = new Table(this.GetType(), "Test");

        table.Columns.Add(new Column(type, name) {
            Length = length,
        });
        table.Columns.Add(new Column(ColumnType.Integer, "Second")); // force comma at end of first line.

        var sql = new SqlServerSqlGenerator().CreateTable(table);
        Assert.Contains(expected, sql);
    }

    [Fact]
    public void DataInsertsRows()
    {
        var table = new Table(this.GetType(), "Test");
        table.Columns.Add(new Column(ColumnType.Key, "Id"));
        table.Columns.Add(new Column(ColumnType.Text, "Value"));

        table.Data.Add(new Dictionary<string, object>() {
            { "Id",  1 }, {"Value", "name" }
        });
        var sql = new SqlServerSqlGenerator().InsertData(table);

        // ignore CR and double space for test...
        sql = sql.Replace("\n", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
        Assert.Contains("INSERT INTO [Test] ([Id], [Value]) VALUES (1, 'name')", sql);
    }

    [Fact]
    public void ForeignKeyConstraints()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();
        var sql = warehouse.ToSql();

        Assert.DoesNotContain("CONSTRAINT", sql);
    }

    public class TestContext : DbContext {

        [FactTable]
        public class Source {
            public int Id { get; set; }

            public Target? Target { get; set; }
        }

        [DimensionTable]
        public class Target {
            public int Id { get; set; }
        }

        public DbSet<Source> Sources { get; set; } = null!;

        public DbSet<Target> Targets { get; set; } = null!;
    }

}

