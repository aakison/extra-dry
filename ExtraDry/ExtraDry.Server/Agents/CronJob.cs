using NCrontab;

namespace ExtraDry.Server.Agents;

/// <summary>
/// Represents the details of a Cron Job that is scheduled to run at specific times on the
/// <see cref="CronService"/>.  Add new jobs to the service by using the <see cref="ServiceCollectionExtensions.AddCronJob(IServiceCollection, string, string, Action)"/> extension methods.
/// </summary>
public abstract class CronJob(
    string cronSchedule, 
    string name)
    : ICronJob
{
    /// <summary>
    /// The name of the job, used for logging and diagnostics.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The Cron schedule for the job.  Uses either 5 or 6 fields, depending on the schedule and if seconds are required.
    /// </summary>
    public string CronSchedule { get; } = cronSchedule;

    /// <summary>
    /// The date and time, in UTC, of the next scheduled occurrence of the job.
    /// </summary>
    public DateTime Next { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// The next occurrences of the job, up to 1 month in the future.
    /// </summary>
    public IEnumerable<DateTime> NextOccurrences => Crontab.GetNextOccurrences(DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

    public void CalculateNext(DateTime now)
    {
        Next = Crontab.GetNextOccurrence(now);
    }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    private CrontabSchedule Crontab { get; init; } = ParseSchedule(cronSchedule);

    private static CrontabSchedule ParseSchedule(string cronSchedule)
    {
        try {
            var useSeconds = new CrontabSchedule.ParseOptions() {
                IncludingSeconds = true,
            };
            return CrontabSchedule.Parse(cronSchedule, useSeconds);
        }
        catch {
            var noSeconds = new CrontabSchedule.ParseOptions() {
                IncludingSeconds = false,
            };
            return CrontabSchedule.Parse(cronSchedule, noSeconds);
        }
    }
}
