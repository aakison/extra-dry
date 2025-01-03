using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server.Agents;

/// <summary>
/// Extensions for adding cron jobs to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a cron job to the service collection.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the class to be registered that implements ICronJob.
    /// </typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="name">The name of the job for logging.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, string cronSchedule, string name = "anonymous")
        where T : class, ICronJob
    {
        services.AddHostedService<CronService>();
        services.AddSingleton<T>();
        services.AddSingleton<CronJob>(provider => {
            var agentJob = provider.GetRequiredService<T>();
            return new ChainingCronJob(cronSchedule, name, agentJob);
        });
        return services;
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="function">The lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, Action function)
    {
        return AddCronJob(services, cronSchedule, "anonymous", function);
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="name">The name of the job for logging.</param>
    /// <param name="function">The lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, string name, Action function)
    {
        void Trigger(CancellationToken _)
        {
            function.Invoke();
        }
        AddCronJob(services, cronSchedule, name, Trigger);
        return services;
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="function">The cancellable lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, Action<CancellationToken> function)
    {
        return AddCronJob(services, cronSchedule, "anonymous", function);
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="name">The name of the job for logging.</param>
    /// <param name="function">The cancellable lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, string name, Action<CancellationToken> function)
    {
        services.AddHostedService<CronService>();
        services.AddSingleton<CronJob>(provider => {
            return new AnonymousCronJob(cronSchedule, name, function);
        });
        return services;
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="function">The awaitable, cancellable lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, Func<CancellationToken, Task> function)
    {
        return AddCronJob(services, cronSchedule, "anonymous", function);
    }

    /// <summary>
    /// Add a cron job to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> for DI.</param>
    /// <param name="cronSchedule">The Cron expression for the schedule.</param>
    /// <param name="name">The name of the job for logging.</param>
    /// <param name="function">The awaitable, cancellable lambda function that is run.</param>
    /// <returns>The <see cref="IServiceCollection" /> for chaining calls.</returns>
    public static IServiceCollection AddCronJob(this IServiceCollection services, string cronSchedule, string name, Func<CancellationToken, Task> function)
    {
        services.AddHostedService<CronService>();
        services.AddSingleton<CronJob>(provider => {
            return new AnonymousCronJob(cronSchedule, name, function);
        });
        return services;
    }
}
