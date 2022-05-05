using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseFactTests {

    [Theory]
    [InlineData(typeof(Company), "Company")]
    [InlineData(typeof(Employee), "Worker Bees")]
    public void EntityBecomesFact(Type entityType, string title)
    {
        var warehouse = new Warehouse();
        
        warehouse.CreateSchema(context);

        Assert.Contains(warehouse.Facts, e => e.EntityType == entityType && e.Title == title);
    }

    [Theory]
    [InlineData(typeof(RegionStatus))]
    [InlineData(typeof(RegionLevel))]
    public void EntityDoesNotBecomeFact(Type entityType)
    {
        var warehouse = new Warehouse();

        warehouse.CreateSchema(context);

        Assert.DoesNotContain(warehouse.Facts, e => e.EntityType == entityType);
    }

    [Theory]
    [InlineData(typeof(Company), "Company ID", ColumnType.Integer)] // Key Column naming convention
    [InlineData(typeof(Company), "Title", ColumnType.Text)] // Normal column naming convention
    [InlineData(typeof(Company), "The Code", ColumnType.Text)] // Measure renamed column explicity
    [InlineData(typeof(Company), "Company Status ID", ColumnType.Integer)] // Dimension PK naming convention.
    public void FactHasColumn(Type entityType, string title, ColumnType columnType)
    {
        var warehouse = new Warehouse();

        warehouse.CreateSchema(context);

        var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
        Assert.Contains(fact.Columns, e => e.Title == title && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData(typeof(Company))]
    public void FactHasNoData(Type entityType)
    {
        var warehouse = new Warehouse();

        warehouse.CreateSchema(context);

        var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
        Assert.Empty(fact.Data);
    }

    private readonly SampleContext context = GetDatabase();

    private static SampleContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<SampleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new SampleContext(options);
    }

}
