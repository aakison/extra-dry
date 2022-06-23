using ExtraDry.Server.DataWarehouse;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sample.Data;

namespace Sample.WebJob;

public class DataFactoryJobs {

    public DataFactoryJobs(DataFactory<SampleWarehouseModel, SampleContext, WarehouseContext> dataFactory, ILogger<DataFactoryJobs> iLogger)
    {
        factory = dataFactory;
        logger = iLogger;
    }

    /// <summary>
    /// A routine processing step that runs a single batch until all batches are complete, chance to catch up if tons of updates.
    /// Cron is set to run at 2pm UTC, which is midnight AEST.
    /// </summary>
    /// <remarks>
    /// See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/ (0 0 9 * * MON)
    /// </remarks>
    [FunctionName("IntradayProcessing")]
    public async Task IntradayProcessing([TimerTrigger(EveryMinuteCron)] TimerInfo _)
    {
        logger.LogInformation("Started 'IntradayProcessing' Web Job");
        var count = await factory.ProcessBatchesAsync();
        logger.LogInformation("Completed 'IntradayProcessing' Web Job, {count} records processed.", count);
    }

    /// <summary>
    /// A nightly processing step that runs until all batches are complete, chance to catch up if tons of updates.
    /// Cron is set to run at 2pm UTC, which is midnight AEST.
    /// </summary>
    /// <remarks>
    /// See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/ (0 0 9 * * MON)
    /// </remarks>
    [FunctionName("NightlyProcessing")]
    public async Task NightlyProcessing([TimerTrigger(AestMidnightCron)] TimerInfo _)
    {
        logger.LogInformation("Started 'NightlyProcessing' Web Job");
        int count = 0;
        while(await factory.ProcessBatchesAsync() > 0) {
            ++count;
        }
        logger.LogInformation("Completed 'NightlyProcessing' Web Job, {count} batches processed.", count);
    }

    private const string EveryMinuteCron = "0 * * * * *";

    private const string AestMidnightCron = "0 0 14 * * *";

    private readonly DataFactory<SampleWarehouseModel, SampleContext, WarehouseContext> factory;

    private readonly ILogger<DataFactoryJobs> logger;

}
