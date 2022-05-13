//using ExtraDry.Server.DataWarehouse;
//using Microsoft.EntityFrameworkCore;

//namespace ExtraDry.Server.Tests.WarehouseTests;

//public class WarehouseDimensionTests {

//    [Theory]
//    [InlineData(typeof(CompanyStatus))]
//    [InlineData(typeof(RegionStatus))]
//    public void EntityBecomesDimension(Type entityType)
//    {
//        var warehouse = new Warehouse();
        
//        warehouse.CreateSchema(context);

//        Assert.Contains(warehouse.Dimensions, e => e.EntityType == entityType);
//    }

//    [Theory]
//    [InlineData(typeof(RegionLevel))]
//    [InlineData(typeof(Company))]
//    public void EntityDoesNotBecomeDimension(Type entityType)
//    {
//        var warehouse = new Warehouse();

//        warehouse.CreateSchema(context);

//        Assert.DoesNotContain(warehouse.Dimensions, e => e.EntityType == entityType);
//    }



//    private readonly SampleContext context = GetDatabase();

//    private static SampleContext GetDatabase()
//    {
//        var options = new DbContextOptionsBuilder<SampleContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString())
//            .Options;
//        return new SampleContext(options);
//    }

//}
