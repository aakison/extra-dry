using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.DataWarehouse;

public class DataFactory {

    public DataFactory(WarehouseModel model, DbContext source, WarehouseContext target, DataFactoryOptions? options = null)
    {
        Model = model;
        Oltp = source;
        Olap = target;
        Options = options ?? new();
    }

    public async Task MigrateAsync()
    {
        await Olap.Database.MigrateAsync(); // EF Schema
        foreach(var table in Model.Dimensions.Union(Model.Facts)) {
            var schema = JsonSerializer.Serialize(table);
            var updateInfo = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name);
            if(updateInfo == null) {
                await Olap.Database.BeginTransactionAsync();
                updateInfo = new DataTableSync { Schema = schema, Table = table.Name, SyncTimestamp = DateTime.MinValue };
                Olap.TableSyncs.Add(updateInfo);
                await CreateTargetTable(table, updateInfo);
                await Olap.Database.CommitTransactionAsync();
            }
            else if(updateInfo.Schema != schema) {
                await Olap.Database.BeginTransactionAsync();
                updateInfo.Schema = schema;
                updateInfo.SyncTimestamp = DateTime.MinValue;
                await DropTargetTable(table);
                await CreateTargetTable(table, updateInfo);
                await Olap.Database.CommitTransactionAsync();
            }

            await ProcessTableBatch(table);
        }
    }

    public async Task ProcessTableBatch(Table table)
    {
        if(table.SourceProperty == null) {
            return; // can't process changers
        }

        var batchStats = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");

        var dbSet = table.SourceProperty.GetValue(Oltp) as IQueryable
            ?? throw new DryException("Source context for model must match the source DbContext for the factory.");

        var dyn = (dynamic)dbSet;
        var where = LinqBuilder.WhereVersionModifiedAfter(dyn, batchStats.SyncTimestamp);
        var ordered = LinqBuilder.OrderBy(where, "Id");
        var taken = Queryable.Take(ordered, Options.BatchSize);
        var batch = await EntityFrameworkQueryableExtensions.ToListAsync(taken);


        //var batchIncoming = await Oltp.Companies
        //    .Where(e => e.Version.DateModified > batchStats.SyncTimestamp)
        //    .OrderBy(e => e.Version.DateModified)
        //    .Take(10).ToListAsync();
        //var target = model.Dimensions.First(e => e.Name == targetTableName);

        //foreach(var item in batchIncoming) {
        //    var sql = factory.Upsert(target, item);
        //    Console.WriteLine(sql);
        //    await warehouseContext.Database.ExecuteSqlRawAsync(sql);
        //}
        //batchStats.SyncTimestamp = batchIncoming.Max(e => e.Version.DateModified);
        //await warehouseContext.SaveChangesAsync();

    }

    public string Upsert(Table table, object entity)
    {
        var values = new Dictionary<string, object>();
        var key = table.KeyColumn.PropertyInfo?.GetValue(entity, null) ?? throw new DryException("No key value defined");
        foreach(var column in table.ValueColumns) {
            var value = column.PropertyInfo?.GetValue(entity, null) ?? column.Default;
            values.Add(column.Name, value);
        }
        return Sql.Upsert(table, (int)key, values);
    }


    private async Task CreateTargetTable(Table table, DataTableSync updateInfo)
    {
        var sqlTable = Sql.CreateTable(table);
        await Olap.Database.ExecuteSqlRawAsync(sqlTable);
        var sqlData = Sql.InsertData(table);
        if(!string.IsNullOrWhiteSpace(sqlData)) {
            // Enums have static data
            await Olap.Database.ExecuteSqlRawAsync(sqlData);
            updateInfo.SyncTimestamp = DateTime.UtcNow;
        }
        await Olap.SaveChangesAsync();
    }

    private async Task DropTargetTable(Table table)
    {
        var sqlDrop = Sql.DropTable(table);
        await Olap.Database.ExecuteSqlRawAsync(sqlDrop);
        await Olap.SaveChangesAsync();
    }

    private WarehouseModel Model { get; set; }

    private DbContext Oltp { get; set; }

    private WarehouseContext Olap { get; set; }

    private DataFactoryOptions Options { get; set; }

    private ISqlGenerator Sql { get; } = new SqlServerSqlGenerator();

}
