namespace ExtraDry.Server.Agents;

/// <summary>
/// Internal Cron Job that wraps an anonymous function that was set using a lambda expression
/// passed to the AddCronJob method.
/// </summary>
internal class AnonymousCronJob : CronJob
{
    public AnonymousCronJob(string cronSchedule, string name, Action action) : base(cronSchedule, name)
    {
        Task Wrapper(CancellationToken _)
        {
            action.Invoke();
            return Task.CompletedTask;
        }
        Callback = Wrapper;
    }

    public AnonymousCronJob(string cronSchedule, string name, Action<CancellationToken> action) : base(cronSchedule, name)
    {
        Task Wrapper(CancellationToken cancellationToken)
        {
            action.Invoke(cancellationToken);
            return Task.CompletedTask;
        }
        Callback = Wrapper;
    }

    public AnonymousCronJob(string cronSchedule, string name, Func<Task> function) : base(cronSchedule, name)
    {
        async Task Wrapper(CancellationToken _)
        {
            await function.Invoke();
        }
        Callback = Wrapper;
    }

    public AnonymousCronJob(string cronSchedule, string name, Func<CancellationToken, Task> function) : base(cronSchedule, name)
    {
        Callback = function;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Callback.Invoke(cancellationToken);
    }

    private Func<CancellationToken, Task> Callback { get; set; }
}
