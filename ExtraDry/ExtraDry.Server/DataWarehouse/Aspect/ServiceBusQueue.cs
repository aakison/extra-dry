using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// A dependency injectable service bus queue abstraction.
/// This entity does not pool connections, but is safe to add as a singleton.
/// Messages on this queue must conform to the signature of `T and be JSON serializable.
/// </summary>
/// <remarks>
/// Advice on singleton:
/// https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
/// </remarks>
public class ServiceBusQueue<T> {

    public ServiceBusQueue(ServiceBusSenderOptions? options = null, IConfiguration? configuration = null)
    {
        Options = options ?? new();
        var connectionString = Options.ConnectionString
            ?? configuration?.GetConnectionString(Options.ConnectionStringKey)
            ?? throw new InvalidOperationException("Could not resolve a connection string for the service bus.");
        Client = new ServiceBusClient(connectionString);
        Sender = Client.CreateSender(Options.QueueName);
    }

    public string QueueName => Options.QueueName;

    public async Task SendBatchAsync(IEnumerable<T> items)
    {
        using var batch = await Sender.CreateMessageBatchAsync();
        foreach(var item in items) {
            var json = JsonSerializer.Serialize(item);
            var message = new ServiceBusMessage(json) { ContentType = "application/json" };
            if(batch.TryAddMessage(message)) {
                throw new Exception("Couldn't add message to batch.");
            }
        }
        await Sender.SendMessagesAsync(batch);
    }

    private ServiceBusSenderOptions Options { get; }

    private ServiceBusClient Client { get; }

    private ServiceBusSender Sender { get; }

}
