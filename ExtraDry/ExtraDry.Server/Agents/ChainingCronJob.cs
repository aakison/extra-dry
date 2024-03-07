namespace ExtraDry.Server.Agents;

/// <summary>
/// Internal Cron Job that wraps another ICronJob to allow chaining of jobs, used to attach the 
/// concrete CronJob implementation to an interface that is easier for user to consume.
/// </summary>
internal class ChainingCronJob(
    string cronSchedule, 
    string name, 
    ICronJob job) 
    : CronJob(cronSchedule, name)
{
    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await job.ExecuteAsync(cancellationToken);
    }
}
