using System.Text;
using System.Text.Json;

namespace Sample.Components.Agent;

internal class AgentService(
    ILogger<AgentService> logger,
    IHttpClientFactory clientFactory)
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Agent Start");

        var http = clientFactory.CreateClient("api");
        var component = new Component() {
            Description = "description",
            Slug = "slug-123",
            Tenant = "second",
            Title = "title",
            Type = "Component",
        };
        var json = JsonSerializer.Serialize(component);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var _ = await http.PostAsync("/second/components", content, cancellationToken);

        await Task.Delay(1, cancellationToken);

        logger.LogInformation("Agent Stop");
    }
}
