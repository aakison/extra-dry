using ExtraDry.Server.DataWarehouse.Builder;
using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class DataFactory {

    public DataFactory(WarehouseModel model, DbContext source, WarehouseContext target, ILogger<DataFactory>? logger, DataFactoryOptions? options = null)
    {
        Model = model;
        Oltp = source;
        Olap = target;
        Logger = logger;
        Options = options ?? new();
    }

    public async Task MigrateAsync()
    {
        Logger?.LogInformation("Checking for necessary migrations.");
        await Olap.Database.MigrateAsync(); // EF Schema
        foreach(var table in Model.Dimensions.Union(Model.Facts)) {
            var schema = JsonSerializer.Serialize(table);
            var updateInfo = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name);
            if(updateInfo == null) {
                Logger?.LogInformation("No table for [{TableName}], creating new.", table.Name);
                await Olap.Database.BeginTransactionAsync();
                updateInfo = new DataTableSync { Schema = schema, Table = table.Name, SyncTimestamp = DateTime.MinValue };
                Olap.TableSyncs.Add(updateInfo);
                await CreateTargetTable(table, updateInfo);
                await Olap.Database.CommitTransactionAsync();
            }
            else if(updateInfo.Schema != schema) {
                Logger?.LogInformation("Table for [{TableName}] obsolete, dropping and creating new.", table.Name);
                await Olap.Database.BeginTransactionAsync();
                updateInfo.Schema = schema;
                updateInfo.SyncTimestamp = DateTime.MinValue;
                await DropTargetTable(table);
                await CreateTargetTable(table, updateInfo);
                await Olap.Database.CommitTransactionAsync();
            }
            else {
                Logger?.LogInformation("No changes detected for [{TableName}].", table.Name);
            }
        }
    }

    /// <summary>
    /// For a given entity, process the changes for all warehouse table that have the entity as a source.
    /// </summary>
    public async Task<int> ProcessBatchAsync(string entity)
    {
        var tables = Model.Dimensions.Union(Model.Facts).Where(e => e.SourceProperty != null);
        var count = 0;
        foreach(var table in tables) {
            if(table.EntityType.Name.Equals(entity, StringComparison.OrdinalIgnoreCase)) {
                count += await ProcessTableBatchAsync(table);
            }
        }
        return count;
    }

    /// <summary>
    /// Process changes for all entities.
    /// </summary>
    public async Task<int> ProcessBatchesAsync()
    {
        await OptionallyApplyMigrationsAsync();
        var tables = Model.Dimensions.Union(Model.Facts).Where(e => e.SourceProperty != null);
        var count = 0;
        foreach(var table in tables) {
            count += await ProcessTableBatchAsync(table);
        }
        // TODO: Expand for custom date tables
        var dateTables = Model.Dimensions.Where(e => e.Generator != null);
        foreach(var table in dateTables) {
            count += await ProcessGeneratorBatchAsync(table);
        }
        return count;
    }

    private async Task<int> ProcessGeneratorBatchAsync(Table table)
    {
        Logger?.LogDebug("Processing date records for [{TableName}]", table.Name);

        if(table.Generator == null) {
            throw new DryException("Can't use method when no generator is defined.");
        }

        var batchStats = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");
        Logger?.LogDebug("Most recent record for [{TableName}] was modified on {Timestamp}", table.Name, batchStats.SyncTimestamp);

        var batch = await table.Generator.GetBatchAsync(table, Oltp, Olap, Sql);

        if(batch.Any()) {
            await UpsertBatch(table, batchStats, batch);
        }
        else {
            Logger?.LogDebug("No new dates required on [{TableName}], batch completed with no changes.", table.Name);
        }
        return batch.Count;
    }

    private async Task<int> ProcessTableBatchAsync(Table table)
    {
        Logger?.LogDebug("Processing batch for [{TableName}]", table.Name);
        if(table.SourceProperty == null) {
            Logger?.LogDebug("Table [{TableName}] not dynamic, batch load aborted", table.Name);
            return 0; // can't process changes on enums without a source property.
        }

        var batchStats = await Olap.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");
        Logger?.LogDebug("Most recent record for [{TableName}] was modified on {Timestamp}", table.Name, batchStats.SyncTimestamp);

        var batch = await GetBatchAfterTimestampAsync(table.SourceProperty, batchStats);

        if(batch.Any()) {
            await UpsertBatch(table, batchStats, batch);

            var duplicateTimestamps = await GetBatchExactTimestamp(table.SourceProperty, batchStats);
            duplicateTimestamps = duplicateTimestamps.Where(e => !batch.Contains(e)).ToList();
            if(duplicateTimestamps.Any()) {
                Logger?.LogInformation("Duplicate entities with same modified timestamp {Timestamp}, extending batch.", batchStats.SyncTimestamp);
                await UpsertBatch(table, batchStats, duplicateTimestamps);
            }
        }
        else {
            Logger?.LogDebug("No entities modified on [{TableName}], batch completed with no changes.", table.Name);
        }
        return batch.Count;
    }

    private async Task UpsertBatch(Table table, DataTableSync batchStats, List<object> batch)
    {
        Logger?.LogInformation("Modified entities on [{TableName}], processing {BatchCount} upserts.", table.Name, batch.Count);
        foreach(var item in batch) {
            var sql = Upsert(table, item);
            Logger?.LogTrace("Executing Upsert SQL: {Sql}", sql);
            await Olap.Database.ExecuteSqlRawAsync(sql);
        }
        batchStats.SyncTimestamp =  table.Generator?.GetSyncTimestamp()
            ?? batch.Max(e => GetVersionInfo(e)?.DateModified ?? DateTime.MinValue);
        await Olap.SaveChangesAsync();
        Logger?.LogInformation("Processed {BatchCount} upserts on [{TableName}], updating sync timestamp to {Timestamp}.", batch.Count, table.Name, batchStats.SyncTimestamp);
    }

    private async Task<List<object>> GetBatchAfterTimestampAsync(PropertyInfo entitiesDbSet, DataTableSync batchStats)
    {
        // Dynamically build up something comparable to the following:
        // var batchIncoming = await Oltp.Companies
        //      .Where(e => e.Version.DateModified > batchStats.SyncTimestamp)
        //      .OrderBy(e => e.Version.DateModified)
        //      .Take(Options.BatchSize)
        //      .ToListAsync();
        // Dynamic used to access extensions methods manually, then once batch received get out of dynamic hell.
        var dbSet = (dynamic?)entitiesDbSet.GetValue(Oltp)
            ?? throw new DryException("Source context for model must match the source DbContext for the factory.");
        var where = LinqBuilder.WhereVersionModified(dbSet, LinqBuilder.EqualityType.GreaterThan, batchStats.SyncTimestamp);
        var ordered = LinqBuilder.OrderBy(where, "Id");
        var taken = Queryable.Take(ordered, Options.BatchSize);
        var results = await EntityFrameworkQueryableExtensions.ToListAsync(taken);
        var ret = new List<object>();
        ret.AddRange(results);
        return ret;
    }

    private async Task<List<object>> GetBatchExactTimestamp(PropertyInfo entitiesDbSet, DataTableSync batchStats)
    {
        // Dynamically build up something comparable to the following:
        // var batchIncoming = await Oltp.Companies
        //      .Where(e => e.Version.DateModified == batchStats.SyncTimestamp)
        //      .ToListAsync();
        // Dynamic used to access extensions methods manually, then once batch received get out of dynamic hell.
        var dbSet = (dynamic?)entitiesDbSet.GetValue(Oltp)
            ?? throw new DryException("Source context for model must match the source DbContext for the factory.");
        var where = LinqBuilder.WhereVersionModified(dbSet, LinqBuilder.EqualityType.EqualTo, batchStats.SyncTimestamp);
        var results = await EntityFrameworkQueryableExtensions.ToListAsync(where);
        var ret = new List<object>();
        ret.AddRange(results);
        return ret;
    }

    private VersionInfo? GetVersionInfo(object item)
    {
        var type = item.GetType();
        if(!VersionInfoProperties.TryGetValue(type, out PropertyInfo? value)) {
            var property = type.GetProperties().FirstOrDefault(e => e.PropertyType == typeof(VersionInfo))
                ?? throw new DryException("Object does not have a VersionInfo property.");
            value = property;
            VersionInfoProperties[type] = value;
        }
        return value.GetValue(item) as VersionInfo;
    }
    private Dictionary<Type, PropertyInfo> VersionInfoProperties { get; } = new();

    private string Upsert(Table table, object entity)
    {
        var values = new Dictionary<string, object>();
        var key = table.KeyColumn.PropertyInfo?.GetValue(entity, null) ?? throw new DryException("No key value defined");
        foreach(var column in table.ValueColumns) {
            var sourceValue = column.PropertyInfo?.GetValue(entity, null);
            var destinationValue = sourceValue == null ? column.Default : column.Converter(sourceValue);
            if(sourceValue != null && column.ColumnType == ColumnType.Integer && (column.PropertyInfo?.PropertyType?.IsClass ?? false)) {
                var referencedEntity = Model.Dimensions.FirstOrDefault(e => e.EntityType == column.PropertyInfo.PropertyType);
                var valueKey = referencedEntity?.KeyColumn?.PropertyInfo?.GetValue(sourceValue, null);
                values.Add(column.Name, valueKey ?? destinationValue);
            }
            else {
                values.Add(column.Name, destinationValue);
            }
        }
        return Sql.Upsert(table, (int)key, values);
    }


    private async Task CreateTargetTable(Table table, DataTableSync updateInfo)
    {
        Logger?.LogInformation("Creating warehouse table [{TableName}]", table.Name);
        var sqlTable = Sql.CreateTable(table);
        Logger?.LogTrace("Executing Create Table SQL: {Sql}", sqlTable);
        await Olap.Database.ExecuteSqlRawAsync(sqlTable);
        var sqlData = Sql.InsertData(table);
        if(!string.IsNullOrWhiteSpace(sqlData)) {
            Logger?.LogTrace("Executing Insert Data SQL: {Sql}", sqlData);
            // Enums have static data
            await Olap.Database.ExecuteSqlRawAsync(sqlData);
            updateInfo.SyncTimestamp = DateTime.UtcNow;
        }
        await Olap.SaveChangesAsync();
    }

    private async Task DropTargetTable(Table table)
    {
        var sqlDrop = Sql.DropTable(table);
        Logger?.LogTrace("Executing Drop Table SQL: {Sql}", sqlDrop);
        await Olap.Database.ExecuteSqlRawAsync(sqlDrop);
        await Olap.SaveChangesAsync();
    }

    private async Task OptionallyApplyMigrationsAsync()
    {
        if(Options.AutoMigrations && !migrationsApplied) {
            await MigrateAsync();
            migrationsApplied = true;
        }
    }

    private bool migrationsApplied;

    private WarehouseModel Model { get; }

    private DbContext Oltp { get; }

    private WarehouseContext Olap { get; }

    private DataFactoryOptions Options { get; }

    private ILogger<DataFactory>? Logger { get; }

    private ISqlGenerator Sql { get; } = new SqlServerSqlGenerator();

}

public class DataFactory<TModel, TOltpContext, TOlapContext> : DataFactory
    where TModel : WarehouseModel
    where TOltpContext : DbContext
    where TOlapContext : WarehouseContext  {

    public DataFactory(TModel model, TOltpContext source, TOlapContext target, ILogger<DataFactory>? logger, DataFactoryOptions? options = null)
        : base(model, source, target, logger, options)
    {
    }
}
