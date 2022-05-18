// See https://aka.ms/new-console-template for more information
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Sample.Shared;
using System.Text.Json;

//var warehouse = new Warehouse();
//var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
////var connectionString = Configuration.GetConnectionString("Sample");
//var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
//var context = new SampleContext(dbOptionsBuilder.Options);
//warehouse.CreateSchema(context);

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
