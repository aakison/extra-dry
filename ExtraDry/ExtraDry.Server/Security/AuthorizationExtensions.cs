using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server.Security;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddRbacAuthorization(this IServiceCollection services, Action<AbacOptionsBuilder> builder)
    {
        var abacOptions = new AbacOptions();
        services.AddSingleton(abacOptions);
        services.AddSingleton<IAuthorizationHandler, RbacRouteMatchesClaimRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, AbacRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, RbacRequirementHandler>();
        var buildTarget = new AbacOptionsBuilder(abacOptions);
        builder.Invoke(buildTarget);
        var policyBuilder = services.AddAuthorizationBuilder();
        foreach(var policy in abacOptions.Policies) {
            if(!string.IsNullOrWhiteSpace(policy.Name)) {
                policyBuilder.AddPolicy(policy.Name, e => e.AddRbacRequirement(policy.Name));
            }
        }
        return services;
    }
}
