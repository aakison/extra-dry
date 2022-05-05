// See https://aka.ms/new-console-template for more information
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var warehouse = new Warehouse();
var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExtraDrySample;Trusted_Connection=True;";
//var connectionString = Configuration.GetConnectionString("Sample");
var dbOptionsBuilder = new DbContextOptionsBuilder<SampleContext>().UseSqlServer(connectionString);
var context = new SampleContext(dbOptionsBuilder.Options);
warehouse.CreateSchema(context);

var options = new JsonSerializerOptions() { WriteIndented = true };
var json = JsonSerializer.Serialize(warehouse, options);
Console.WriteLine(json);
