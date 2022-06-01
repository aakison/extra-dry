// See https://aka.ms/new-console-template for more information
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Sample.Shared;
using System.Text.Json;

var options = new JsonSerializerOptions() { WriteIndented = true };
//var json = JsonSerializer.Serialize(warehouse, options);
//Console.WriteLine(json);
//Console.WriteLine(warehouse.GenerateSql());

var builder = new WarehouseModelBuilder();
builder.LoadSchema<SampleContext>();

builder.Fact<Company>().Measure(e => e.AnnualRevenue).HasName("Big Bucks");

var model = builder.Build();
var compareJson = JsonSerializer.Serialize(model, options);
Console.WriteLine(compareJson);
Console.WriteLine(model.ToSql());


var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
var context = new SampleContext(dbOptionsBuilder.Options);


var warehouseConnectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDryWarehouse;Trusted_Connection=True;";
var warehouseOptionsBuilder = new DbContextOptionsBuilder<WarehouseContext>().UseSqlServer(warehouseConnectionString);
var warehouseContext = new WarehouseContext(warehouseOptionsBuilder.Options);

var factoryOptions = new DataFactoryOptions() {
    BatchSize = 10,
};

var factory = new DataFactory(model, context, warehouseContext, factoryOptions);
await factory.MigrateAsync();



var targetTableName = "Company Details";
var batchStats = await warehouseContext.TableSyncs.FirstOrDefaultAsync(e => e.Table == targetTableName);
if(batchStats == null) {
    batchStats = new DataTableSync { Table = targetTableName, SyncTimestamp = DateTime.MinValue };
    warehouseContext.TableSyncs.Add(batchStats);
    await warehouseContext.SaveChangesAsync();
}
var batchIncoming = await context.Companies
    .Where(e => e.Version.DateModified > batchStats.SyncTimestamp)
    .OrderBy(e => e.Version.DateModified)
    .Take(10).ToListAsync();
var target = model.Dimensions.First(e => e.Name == targetTableName);

foreach(var item in batchIncoming) {
    var sql = factory.Upsert(target, item);
    Console.WriteLine(sql);
    await warehouseContext.Database.ExecuteSqlRawAsync(sql);
}
batchStats.SyncTimestamp = batchIncoming.Max(e => e.Version.DateModified);
await warehouseContext.SaveChangesAsync();



//var x = new DbSet<Company>();
//var lastModifiedInWarehouse = DateTime.UtcNow;
//var lastCreatedInWarehouse = DateTime.UtcNow;
//var itemsToLoad = x.Where(e => e.Version.DateModified > lastModifiedInWarehouse)
//    .OrderBy(e => e.Version.DateModified)
//    .Take(100);

//foreach(var item in itemsToLoad) {
//    if(item.Version.DateCreated > lastCreatedInWarehouse) {
//        // create insert
//    }
//    else {
//        // create update
//    }
//}
//var lastModifiedInBatch = 
