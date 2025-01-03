namespace GettingStarted.Consumers;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class GettingStartedConsumer(
    ILogger<GettingStartedConsumer> logger)
    : IConsumer<GenericEvent>
{
    public Task Consume(ConsumeContext<GenericEvent> context)
    {
        logger.LogInformation("Received Text: {Text}", context.Message.Value);
        return Task.CompletedTask;
    }
}
