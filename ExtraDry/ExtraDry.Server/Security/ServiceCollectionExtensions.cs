using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server;

/// <summary>
/// Extensions for adding authorization handlers to the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Register the common authorization handlers for use in the application.
    /// </summary>
    public static IServiceCollection AddAuthorizationExtensions(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, RouteMatchesClaimRequirementHandler>();
        return services;
    }

}
