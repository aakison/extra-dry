using ExtraDry.Server.Security;
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

    public static IServiceCollection AddAttributeAuthorization(this IServiceCollection services, Action<AttributeAuthorizationOptions>? configure = null)
    {
        var options = new AttributeAuthorizationOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
        services.AddSingleton<AttributeAuthorization>();
        return services;
    }

}

public class AttributeAuthorizationOptions
{
    //public bool Enable { get; set; }
}
