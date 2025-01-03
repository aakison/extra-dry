using ExtraDry.Server.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server;

/// <summary>
/// Extensions for adding authorization handlers to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register the extensions that allows using ABAC rules from options.
    /// </summary>
    public static IServiceCollection AddAbacExtensions(this IServiceCollection services, Action<AbacOptions>? config = null)
    {
        services.AddSingleton(provider => {
            var options = new AbacOptions();
            var configuration = provider.GetRequiredService<IConfiguration>();
            configuration.GetSection(AbacOptions.SectionName).Bind(options);
            config?.Invoke(options);
            return options;
        });
        services.AddSingleton<IAuthorizationHandler, RbacRouteMatchesClaimRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, AbacRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, RbacRequirementHandler>();
        return services;
    }
}
