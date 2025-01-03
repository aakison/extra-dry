using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server.EF;

/// <summary>
/// Extensions for adding database aspects to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to add the <see cref="AuditAspect" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddAuditAspect(this IServiceCollection services, Action<AuditAspectOptions>? config = null)
    {
        var options = new AuditAspectOptions();
        config?.Invoke(options);
        new DataValidator().ValidateObject(options);

        services.AddSingleton(options);
        services.AddScoped<IDbAspect, AuditAspect>();

        return services;
    }

    /// <summary>
    /// Extension method to add the <see cref="RevisionAspect" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddRevisionAspect(this IServiceCollection services, Action<RevisionAspectOptions>? config = null)
    {
        var options = new RevisionAspectOptions();
        config?.Invoke(options);
        new DataValidator().ValidateObject(options);

        services.AddSingleton(options);
        services.AddScoped<IDbAspect, RevisionAspect>();

        return services;
    }
}
