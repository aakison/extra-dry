namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// Options for configuring a `ServiceBusSender-T`.
/// </summary>
public class ServiceBusSenderOptions
{
    /// <summary>
    /// The name of the key into the configuration to determine the connection string. This
    /// defaults to 'AzureWebJobsServiceBus' which is the default of the service bus trigger. This
    /// can be overridden by explicitly setting `ConnectionString`.
    /// </summary>
    public string ConnectionStringKey { get; set; } = "AzureWebJobsServiceBus";

    /// <summary>
    /// If provided, the connection string for the service bus connection.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// The name of the service bus queue to send the messages to.
    /// </summary>
    public string QueueName { get; set; } = "queue";
}
