using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace Sample.Components.Agent;

public class AgentHealthCheck : IHealthCheck
{
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
