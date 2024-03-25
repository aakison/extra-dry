using ExtraDry.Server.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExtraDry.Server;

/// <summary>
/// Extensions for adding authorization handlers to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Register the common authorization handlers for use in the application.
    /// </summary>
    public static IServiceCollection AddRbacExtensions(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, RbacRouteMatchesClaimRequirementHandler>();
        return services;
    }

    /// <summary>
    /// Register the extensions that allows using ABAC rules from options, enabling the 
    /// <see cref="AbacAuthorization"/> service for simple ABAC checks in controllers.
    /// </summary>
    public static IServiceCollection AddAbacExtensions(this IServiceCollection services, Action<AbacOptions>? config = null)
    {
        services.AddSingleton(services => {
            var options = new AbacOptions();
            var configuration = services.GetRequiredService<IConfiguration>();
            configuration.GetSection(AbacOptions.SectionName).Bind(options);
            config?.Invoke(options);
            return options;
        });
        services.AddSingleton<AbacAuthorization>();
        return services;
    }

}
