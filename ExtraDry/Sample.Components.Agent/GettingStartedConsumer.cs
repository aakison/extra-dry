namespace GettingStarted.Consumers;

using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

public class GettingStartedConsumer(
    ILogger<GettingStartedConsumer> logger)
    : IConsumer<GettingStarted>
{
    public Task Consume(ConsumeContext<GettingStarted> context)
    {
        logger.LogInformation("Received Text: {Text}", context.Message.Value);
        return Task.CompletedTask;
    }
}
