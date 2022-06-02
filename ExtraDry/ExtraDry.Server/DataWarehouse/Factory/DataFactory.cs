using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            return; // can't process changes on enums without a source property.
        }

        var batchStats = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");

        // Dynamically build up something comparable to the following:
        // var batchIncoming = await Oltp.Companies
        //      .Where(e => e.Version.DateModified > batchStats.SyncTimestamp)
        //      .OrderBy(e => e.Version.DateModified)
        //      .Take(Options.BatchSize)
        //      .ToListAsync();
        // Dynamic used to access extensions methods manually, then once batch received get out of dynamic hell.
        var dbSet = (dynamic?)table.SourceProperty.GetValue(Oltp)
            ?? throw new DryException("Source context for model must match the source DbContext for the factory.");
        var where = LinqBuilder.WhereVersionModifiedAfter(dbSet, batchStats.SyncTimestamp);
        var ordered = LinqBuilder.OrderBy(where, "Id");
        var taken = Queryable.Take(ordered, Options.BatchSize);
        var batch = new List<object>();
        batch.AddRange(await EntityFrameworkQueryableExtensions.ToListAsync(taken));

        if(batch.Any()) {
            var target = Model.Dimensions.First(e => e.Name == table.Name);
            foreach(var item in batch) {
                var sql = Upsert(target, item);
                await Olap.Database.ExecuteSqlRawAsync(sql);
            }
            batchStats.SyncTimestamp = batch.Max(e => GetVersionInfo(e)?.DateModified ?? DateTime.MinValue);
            await Olap.SaveChangesAsync();
        }
    }

    private VersionInfo? GetVersionInfo(object item)
    {
        var type = item.GetType();
        if(!VersionInfoProperties.ContainsKey(type)) {
            var property = type.GetProperties().FirstOrDefault(e => e.PropertyType == typeof(VersionInfo))
                ?? throw new DryException("Object does not have a VersionInfo property.");
            VersionInfoProperties[type] = property;
        }
        return VersionInfoProperties[type].GetValue(item) as VersionInfo;
    }
    private Dictionary<Type, PropertyInfo> VersionInfoProperties { get; } = new();

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
