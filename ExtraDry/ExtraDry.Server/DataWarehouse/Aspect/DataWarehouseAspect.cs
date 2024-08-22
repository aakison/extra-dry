using ExtraDry.Core.Models;
using ExtraDry.Server.EF;
using Microsoft.Extensions.Logging;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// A lightweight aspect that sends the names of updated entities during an EF update to a queue.
/// For performance reasons, ensure that ServiceBusQueue`T is registered as a singleton.
/// </summary>
public class DataWarehouseAspect {

    public DataWarehouseAspect(AspectDbContext _, ServiceBusQueue<EntityMessage> queue, ILogger<DataWarehouseAspect> iLogger)
    {
        Queue = queue;
        logger = iLogger;
        ExceptionTypes.Add(typeof(VersionInfo));
        //context.EntitiesChangedEvent += Context_EntitiesChanged;
    }

    private async void Context_EntitiesChanged(object _, EntitiesChanged args)
    {
        var entities = args.EntitiesAdded.Union(args.EntitiesModified);
        var types = entities.Select(e => e.GetType()).Distinct().Except(ExceptionTypes);
        var messages = types.Select(e => new EntityMessage(e.Name));
        try {
            await Queue.SendBatchAsync(messages);
        }
        catch(Exception) {
            logger.LogTextWarning("Could not send batch of messages to service bus.  Warning only as this will prevent the warehouse from dynamically being updated but it should still be updated on a CRON schedule.");
            // eat exception, don't want to tank EF.
        }
    }

    /// <summary>
    /// Some types don't make sense, currently for example is VersionInfo.
    /// This might be expanded to allow for user selection of types, or by assembly declared, DbSet usage, etc.
    /// For now, just use for VersionInfo...
    /// </summary>
    private List<Type> ExceptionTypes { get; } = [];

    private ServiceBusQueue<EntityMessage> Queue { get; }

    private readonly ILogger<DataWarehouseAspect> logger;

}
