using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Data;
using Sample.Shared;

var services = new ServiceCollection();
services.AddLogging(configure => {
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Debug);
});
services.AddScoped<SampleContext>(services => {
    var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
    var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
    var databaseContext = new SampleContext(dbOptionsBuilder.Options);
    return databaseContext;
});
services.AddScoped<WarehouseContext>(services => {
    var warehouseConnectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDryWarehouse;Trusted_Connection=True;";
    var warehouseOptionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(warehouseConnectionString);
    var warehouseContext = new WarehouseContext(warehouseOptionsBuilder.Options);
    return warehouseContext;
});
services.AddScoped<WarehouseModel>(services => 
    new WarehouseModel<SampleContext>(builder => {
        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
        builder.Dimension<Date>().HasDateGenerator(options => {
            options.StartDate = new DateOnly(2020, 1, 1);
            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
            options.FiscalYearEndingMonth = 6;
        });
        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
        builder.Dimension<Time>().HasTimeGenerator();
    }));
services.AddDataFactory<WarehouseModel, SampleContext, WarehouseContext>(options => {
    options.BatchSize = 10;
    options.AutoMigrations = true;
});

var provider = services.BuildServiceProvider();
var factory = provider.GetRequiredService<DataFactory<WarehouseModel, SampleContext, WarehouseContext>>();
while(await factory.ProcessBatchesAsync() > 0) {
    // no-op
}













// Today and Tomorrow:
//services.AddScoped<FinanceWarehouseContext>();
// Tomorrow:
//services.AddWarehouseContext<FinanceWarehouseContext>(options => {
//    options.NamingScheme = NamingScheme.KebabCase;
//});

//public class ContextlessWarehouseModel : WarehouseModel {

//    public ContextlessWarehouseModel() : base(typeof(SampleContext)) { }

//    protected override void OnCreating(WarehouseModelBuilder builder)
//    {
//        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
//        builder.Dimension<Date>().HasDateGenerator(options => {
//            options.StartDate = new DateOnly(2020, 1, 1);
//            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
//            options.FiscalYearEndingMonth = 6;
//        });
//        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
//        builder.Dimension<Time>().HasTimeGenerator();
//    }

//}

//public class SampleWarehouseModel : WarehouseModel<SampleContext> {

//    protected override void OnCreating(WarehouseModelBuilder builder)
//    {
//        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
//        builder.Dimension<Date>().HasDateGenerator(options => {
//            options.StartDate = new DateOnly(2020, 1, 1);
//            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
//            options.FiscalYearEndingMonth = 6;
//        });
//        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
//        builder.Dimension<Time>().HasTimeGenerator();
//    }

//}

//public class FinanceWarehouseContext : WarehouseModel<SampleContext> {

//    public FinanceWarehouseContext() : base("Finance") { }

//    protected override void OnCreating(WarehouseModelBuilder builder)
//    {
//        builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");
//        builder.Dimension<Date>().HasDateGenerator(options => {
//            options.StartDate = new DateOnly(2020, 1, 1);
//            options.EndDate = new DateOnly(DateTime.UtcNow.Year, 12, 31);
//            options.FiscalYearEndingMonth = 6;
//        });
//        builder.Dimension<Date>().Attribute(e => e.DayOfWeekName).IsIncluded(false);
//        builder.Dimension<Time>().HasTimeGenerator();
//    }

//}
