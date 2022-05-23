using ExtraDry.Server.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseFactTests {

    [Theory]
    [InlineData(typeof(Company), "Company")]
    [InlineData(typeof(Employee), "Worker Bees")]
    public void EntityBecomesFact(Type entityType, string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.Contains(warehouse.Facts, e => e.EntityType == entityType && e.Name == title);
    }

    [Theory]
    [InlineData(typeof(RegionStatus))]
    [InlineData(typeof(RegionLevel))]
    public void EntityDoesNotBecomeFact(Type entityType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.DoesNotContain(warehouse.Facts, e => e.EntityType == entityType);
    }

    [Theory]
    [InlineData(typeof(Company), "Company ID", ColumnType.Key)] // Key Column naming convention
    [InlineData(typeof(Company), "Gross", ColumnType.Decimal)] // Simple property, no decoration or special handling
    [InlineData(typeof(Company), "Sales Margin", ColumnType.Decimal)] // Simple property, name title cased by default.
    [InlineData(typeof(Company), "Big Bucks", ColumnType.Decimal)] // Measure attribute rename
    [InlineData(typeof(Company), "Company Status ID", ColumnType.Integer)] // Dimension PK naming convention.
    public void FactHasColumn(Type entityType, string title, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
        Assert.Contains(fact.Columns, e => e.Name == title && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData(typeof(Company), "Id")] // Should be key, not column.
    [InlineData(typeof(Company), "Title")] // Regular string column
    [InlineData(typeof(Company), "Code")] // String column with Measure attribute.
    public void FactDoesNotHaveColumn(Type entityType, string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
        Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    }

    [Theory]
    [InlineData(typeof(Company))]
    public void FactHasNoData(Type entityType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
        Assert.Empty(fact.Data);
    }

    [Fact]
    public void MethodExpressionThrowsException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Fact<Company>().Measure(e => e.GetHashCode()));
    }

    [Fact]
    public void FieldMemberExpressionThrowsException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Fact<Company>().Measure(e => e.field));
    }

}
