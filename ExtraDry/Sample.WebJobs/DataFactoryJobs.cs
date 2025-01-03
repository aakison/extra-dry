using ExtraDry.Core.Models;
using ExtraDry.Server.DataWarehouse;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sample.Data;
using System.Text.Json;

namespace Sample.WebJob;

public class DataFactoryJobs(
    DataFactory<SampleWarehouseModel, SampleContext, WarehouseContext> dataFactory,
    ILogger<DataFactoryJobs> iLogger)
{
    /// <summary>
    /// Triggered from a message on a queue, this will process updates to a specific entity only.
    /// </summary>
    [FunctionName("EventProcessing")]
    public async Task EventProcessing([ServiceBusTrigger("warehouse-update")] string item)
    {
        var message = JsonSerializer.Deserialize<EntityMessage>(item);
        if(message == null) {
            iLogger.LogError("Unable to process event, message on queue not of type `EntityMessage`, instead '{content}'", item);
            return;
        }
        iLogger.LogInformation("Processing ad-hoc updates for {EntityName}", message.EntityName);
        var count = await dataFactory.ProcessBatchAsync(message.EntityName);
        iLogger.LogInformation("Processed {count} ad-hoc records", count);
    }

    /// <summary>
    /// A routine processing step that runs a single batch until all batches are complete, chance
    /// to catch up if tons of updates. Cron is set to run hourly and only process a single batch.
    /// </summary>
    [FunctionName("IntradayProcessing")]
    public async Task IntradayProcessing([TimerTrigger(HourlyCron)] TimerInfo _)
    {
        iLogger.LogInformation("Started 'IntradayProcessing' Web Job");
        var count = await dataFactory.ProcessBatchesAsync();
        iLogger.LogInformation("Completed 'IntradayProcessing' Web Job, {count} records processed.", count);
    }

    /// <summary>
    /// A nightly processing step that runs until all batches are complete, chance to catch up if
    /// tons of updates. Cron is set to run at 2pm UTC, which is midnight AEST.
    /// </summary>
    /// <remarks>
    /// See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/
    /// </remarks>
    [FunctionName("NightlyProcessing")]
    public async Task NightlyProcessing([TimerTrigger(AestMidnightCron)] TimerInfo _)
    {
        iLogger.LogInformation("Started 'NightlyProcessing' Web Job");
        int count = 0;
        while(await dataFactory.ProcessBatchesAsync() > 0) {
            ++count;
        }
        iLogger.LogInformation("Completed 'NightlyProcessing' Web Job, {count} batches processed.", count);
    }

    private const string HourlyCron = "0 0 * * * *";

    private const string AestMidnightCron = "0 0 14 * * *";
}
