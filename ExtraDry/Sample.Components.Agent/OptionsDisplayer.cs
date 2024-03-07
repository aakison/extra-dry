using ExtraDry.Server.Agents;
using Microsoft.Extensions.Logging;

namespace Sample.Components.Agent;

internal class OptionsDisplayer(
    AgentOptions options,
    ILogger<OptionsDisplayer> logger)
    : ICronJob
{
    public Task ExecuteAsync(CancellationToken _)
    {
        logger.LogInformation("Option View: {option}", options.Test);
        return Task.CompletedTask;
    }
}
