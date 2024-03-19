using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Reflection;

namespace Sample.Components.Api;

/// <summary>
/// Health check for the API.  Contains simply logic to indicate if the system is running and to 
/// provide version information - useful for blue-green deployments.  
/// </summary>
public class ApiHealthCheck : IHealthCheck
{

    /// <summary>
    /// Check just the internal workings of the system, not dependencies.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // Any healthly logic not related to dependencies...

        var data = new Dictionary<string, object> {
            { "process", Assembly.GetExecutingAssembly().GetName().Name ?? "unnamed" },
            { "version", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.9" },
        };

        var result = new HealthCheckResult(isHealthy ? HealthStatus.Healthy : HealthStatus.Unhealthy, data: data);

        return Task.FromResult(result);
    }
}
