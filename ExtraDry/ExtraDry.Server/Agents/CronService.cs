using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace ExtraDry.Server.Agents;

/// <summary>
/// The CronService is a background service that runs on the server and triggers jobs at specific
/// times. Avoid direct use of this class and instead use the `AddCronJob` extensions when
/// configuring services.
/// </summary>
public class CronService(
    ILogger<CronService> logger,
    IServiceProvider services)
    : BackgroundService
{
    /// <summary>
    /// The colleciton of jobs that are managed by this service.
    /// </summary>
    public ReadOnlyCollection<CronJob> CronJobs => new(Jobs);

    /// <summary>
    /// Executes all the jobs that are due to run at the given time. This method is called by the
    /// BackgroundService and should not be called directly.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Jobs.AddRange(services.GetServices<CronJob>());

        logger.LogCronServiceStarted(Jobs.Count);

        if(Jobs.Count == 0) {
            logger.LogTextWarning("No Cron Jobs registered.  Use AddCronJobs to register.");
            logger.LogCronServiceStopped();
            return;
        }

        logger.LogServerTime(DateTime.UtcNow.ToString("O"));
        foreach(var job in Jobs) {
            var times = job.NextOccurrences.Take(3).Select(e => e.ToString("O")).ToList();
            logger.LogCronJobNexts(job.Name, job.CronSchedule, times[0], times[1], times[2]);
        }
        CalculateNextOccurrences();
        while(true) {
            var next = Jobs.Min(e => e.Next);
            await WaitUntilNextAsync(next, stoppingToken);
            if(stoppingToken.IsCancellationRequested) {
                logger.LogCronServiceStopped();
                return;
            }
            await ProcessJobsAsync(next, stoppingToken);
            if(stoppingToken.IsCancellationRequested) {
                logger.LogCronServiceStopped();
                return;
            }
        }
    }

    private List<CronJob> Jobs { get; } = [];

    private void CalculateNextOccurrences()
    {
        var now = DateTime.UtcNow;
        foreach(var job in Jobs) {
            job.CalculateNext(now);
        }
    }

    private async Task WaitUntilNextAsync(DateTime next, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        logger.LogCronJobNextInfo(next.ToString("O"));
        var delay = next - now;
        // count down the minutes so Task.Delay never waits too long, could be up to 24 days.
        while(delay.TotalMinutes > 1.5 && !cancellationToken.IsCancellationRequested) {
            await Task.Delay(60_000, cancellationToken);
            now = DateTime.UtcNow;
            delay = next - now;
        }
        if(!cancellationToken.IsCancellationRequested) {
            var milliseconds = (int)delay.TotalMilliseconds;
            if(milliseconds < 0) {
                milliseconds = 0;
            }
            await Task.Delay(milliseconds, cancellationToken);
        }
    }

    private async Task ProcessJobsAsync(DateTime next, CancellationToken cancellationToken)
    {
        var jobs = Jobs.Where(e => e.Next == next);
        var now = DateTime.UtcNow;
        foreach(var job in jobs) {
            logger.LogCronJobTrigger(job.Name, now.ToString("O"));
            await Task.Delay(1, cancellationToken); // small attempt to help logging appear in logical order.
            await job.ExecuteAsync(cancellationToken);
            job.CalculateNext(now);
        }
    }
}
