using ExtraDry.Core.Parser.Internal;
using ExtraDry.Server.DataWarehouse.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

public class DataFactory(
    WarehouseModel model,
    DbContext source,
    WarehouseContext target,
    ILogger<DataFactory> logger,
    DataFactoryOptions? options = null)
{
    public async Task MigrateAsync()
    {
        logger.LogTextInfo("Checking for necessary migrations.");
        await target.Database.MigrateAsync(); // EF Schema
        foreach(var table in model.Dimensions.Union(model.Facts)) {
            var schema = JsonSerializer.Serialize(table);
            var updateInfo = await target.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name);
            if(updateInfo == null) {
                logger.LogTableChange("No table, creating new.", table.Name);
                await target.Database.BeginTransactionAsync();
                updateInfo = new DataTableSync { Schema = schema, Table = table.Name, SyncTimestamp = DateTime.MinValue };
                target.TableSyncs.Add(updateInfo);
                await CreateTargetTable(table, updateInfo);
                await target.Database.CommitTransactionAsync();
            }
            else if(updateInfo.Schema != schema) {
                logger.LogTableChange("Table obsolete, dropping and creating new.", table.Name);
                await target.Database.BeginTransactionAsync();
                updateInfo.Schema = schema;
                updateInfo.SyncTimestamp = DateTime.MinValue;
                await DropTargetTable(table);
                await CreateTargetTable(table, updateInfo);
                await target.Database.CommitTransactionAsync();
            }
            else {
                logger.LogTableChange("No changes detected", table.Name);
            }
        }
    }

    /// <summary>
    /// For a given entity, process the changes for all warehouse table that have the entity as a
    /// source.
    /// </summary>
    public async Task<int> ProcessBatchAsync(string entity)
    {
        var tables = model.Dimensions.Union(model.Facts).Where(e => e.SourceProperty != null);
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
        var tables = model.Dimensions.Union(model.Facts).Where(e => e.SourceProperty != null);
        var count = 0;
        foreach(var table in tables) {
            count += await ProcessTableBatchAsync(table);
        }
        var dateTables = model.Dimensions.Where(e => e.Generator != null);
        foreach(var table in dateTables) {
            count += await ProcessGeneratorBatchAsync(table);
        }
        return count;
    }

    private async Task<int> ProcessGeneratorBatchAsync(Table table)
    {
        logger.LogTableChange("Processing date records", table.Name);

        if(table.Generator == null) {
            throw new DryException("Can't use method when no generator is defined.");
        }

        var batchStats = await target.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");

        var batch = await table.Generator.GetBatchAsync(table, source, target, Sql);

        if(batch.Count != 0) {
            await UpsertBatch(table, batchStats, batch);
        }
        else {
            logger.LogTableChange("No new dates required on, batch completed with no changes.", table.Name);
        }
        return batch.Count;
    }

    private async Task<int> ProcessTableBatchAsync(Table table)
    {
        logger.LogTableChange("Processing batch.", table.Name);
        if(table.SourceProperty == null) {
            logger.LogTableChange("Table not dynamic, batch load aborted.", table.Name);
            return 0; // can't process changes on enums without a source property.
        }

        var batchStats = await target.TableSyncs.FirstOrDefaultAsync(e => e.Table == table.Name)
            ?? throw new DryException("Unable to process batch, stats missing, run MigrateAsync() first.");

        var batch = await GetBatchAfterTimestampAsync(table.SourceProperty, batchStats);

        if(batch.Count != 0) {
            await UpsertBatch(table, batchStats, batch);

            var duplicateTimestamps = await GetBatchExactTimestamp(table.SourceProperty, batchStats);
            duplicateTimestamps = duplicateTimestamps.Where(e => !batch.Contains(e)).ToList();
            if(duplicateTimestamps.Count != 0) {
                await UpsertBatch(table, batchStats, duplicateTimestamps);
            }
        }
        else {
            logger.LogTableChange("No entities modified, batch completed with no changes.", table.Name);
        }
        return batch.Count;
    }

    private async Task UpsertBatch(Table table, DataTableSync batchStats, List<object> batch)
    {
        logger.LogTableChange("Modified entities, processing upserts.", table.Name);
        foreach(var item in batch) {
            var sql = Upsert(table, item);
            logger.LogTextVerbose($"Executing Upsert SQL: {sql}"); 
            await target.Database.ExecuteSqlRawAsync(sql);
        }
        batchStats.SyncTimestamp = table.Generator?.GetSyncTimestamp()
            ?? batch.Max(e => GetVersionInfo(e)?.DateModified ?? DateTime.MinValue);
        await target.SaveChangesAsync();
        logger.LogTextVerbose($"Processed {batch.Count} upserts on [{table.Name}], updating sync timestamp to {batchStats.SyncTimestamp}."); 
    }

    private async Task<List<object>> GetBatchAfterTimestampAsync(PropertyInfo entitiesDbSet, DataTableSync batchStats)
    {
        // Dynamically build up something comparable to the following: var batchIncoming = await
        // Oltp.Companies .Where(e => e.Version.DateModified > batchStats.SyncTimestamp) .OrderBy(e
        // => e.Version.DateModified) .Take(Options.BatchSize) .ToListAsync(); Dynamic used to
        // access extensions methods manually, then once batch received get out of dynamic hell.
        var dbSet = (dynamic?)entitiesDbSet.GetValue(source)
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
        // Dynamically build up something comparable to the following: var batchIncoming = await
        // Oltp.Companies .Where(e => e.Version.DateModified == batchStats.SyncTimestamp)
        // .ToListAsync(); Dynamic used to access extensions methods manually, then once batch
        // received get out of dynamic hell.
        var dbSet = (dynamic?)entitiesDbSet.GetValue(source)
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

    private Dictionary<Type, PropertyInfo> VersionInfoProperties { get; } = [];

    private string Upsert(Table table, object entity)
    {
        var values = new Dictionary<string, object>();
        var key = table.KeyColumn.PropertyInfo?.GetValue(entity, null) ?? throw new DryException("No key value defined");
        foreach(var column in table.ValueColumns) {
            var sourceValue = column.PropertyInfo?.GetValue(entity, null);
            var destinationValue = sourceValue == null ? column.Default : column.Converter(sourceValue);
            if(sourceValue != null && column.ColumnType == ColumnType.Integer && (column.PropertyInfo?.PropertyType?.IsClass ?? false)) {
                var referencedEntity = model.Dimensions.FirstOrDefault(e => e.EntityType == column.PropertyInfo.PropertyType);
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
        logger.LogTableChange("Creating warehouse table", table.Name);
        var sqlTable = Sql.CreateTable(table);
        logger.LogTextVerbose($"Executing Create Table SQL: {sqlTable}");
        await target.Database.ExecuteSqlRawAsync(sqlTable);
        var sqlData = Sql.InsertData(table);
        if(!string.IsNullOrWhiteSpace(sqlData)) {
            logger.LogTextVerbose($"Executing Insert Data SQL: {sqlData}");
            // Enums have static data
            await target.Database.ExecuteSqlRawAsync(sqlData);
            updateInfo.SyncTimestamp = DateTime.UtcNow;
        }
        await target.SaveChangesAsync();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Remove when tested.")]
    private async Task DropTargetTable(Table table)
    {
        var sqlDrop = Sql.DropTable(table);
        logger.LogDebug("Executing Drop Table SQL: {Sql}", sqlDrop);
        await target.Database.ExecuteSqlRawAsync(sqlDrop);
        await target.SaveChangesAsync();
    }

    private async Task OptionallyApplyMigrationsAsync()
    {
        if(Options.AutoMigrations && !migrationsApplied) {
            await MigrateAsync();
            migrationsApplied = true;
        }
    }

    private bool migrationsApplied;

    private DataFactoryOptions Options { get; } = options ?? new();

    private SqlServerSqlGenerator Sql { get; } = new SqlServerSqlGenerator();
}

public class DataFactory<TModel, TOltpContext, TOlapContext>(
    TModel model,
    TOltpContext source,
    TOlapContext target,
    ILogger<DataFactory> logger,
    DataFactoryOptions? options = null)
    : DataFactory(model, source, target, logger, options)
    where TModel : WarehouseModel
    where TOltpContext : DbContext
    where TOlapContext : WarehouseContext
{
}
