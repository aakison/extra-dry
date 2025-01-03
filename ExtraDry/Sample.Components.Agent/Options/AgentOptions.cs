namespace Sample.Components.Agent;

public partial class AgentOptions : IValidatableObject
{
    public const string SectionName = "AgentOptions";

    public ServiceBusTransport ServiceBus { get; set; } = ServiceBusTransport.InMemory;

    public RabbitMQOptions? RabbitMQ { get; set; }

    public AzureServiceBusOptions? AzureServiceBus { get; set; }

    public ApiOptions Api { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(ServiceBus == ServiceBusTransport.AzureServiceBus) {
            if(AzureServiceBus == null) {
                yield return new ValidationResult("AzureServiceBus Options are required when Transport is AzureServiceBus.");
            }
        }
        if(ServiceBus == ServiceBusTransport.RabbitMQ) {
            if(RabbitMQ == null) {
                yield return new ValidationResult("RabbitMQ Options are required when Transport is RabbitMQ.");
            }
        }
    }
}
