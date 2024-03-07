using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Sample.Components.Agent;

internal class AgentService(
    ILogger<AgentService> logger,
    AgentOptions options,
    IHttpClientFactory clientFactory)
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Agent Start");
        logger.LogInformation("Configuration Option {name}: {value}", nameof(AgentOptions.Test), options.Test);

        var http = clientFactory.CreateClient("api");
        var component = new Component() {
            FullText = "text",
            Code = "code",
            Description = "description",
            Keywords = "keywords",
            Slug = "slug-123",
            Partition = "second",
            Title = "title",
            Discriminator = "Component",
        };
        var json = JsonSerializer.Serialize(component);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var _ = await http.PostAsync("/second/components", content, cancellationToken);

        await Task.Delay(1, cancellationToken);

        logger.LogInformation("Agent Stop");
    }
}
