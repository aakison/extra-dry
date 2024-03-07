using ExtraDry.Server.Agents;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Sample.Components.Agent;

public partial class AgentOptions : IValidatableObject
{
    public const string SectionName = "AgentOptions";

    [Secret]
    public string Test { get; set; } = "";

    public ServiceBusTransport ServiceBus { get; set; } = ServiceBusTransport.InMemory;

    public RabbitMQOptions? RabbitMQOptions { get; set; } 

    public AzureServiceBusOptions? AzureServiceBusOptions { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(ServiceBus == ServiceBusTransport.AzureServiceBus) {
            if(AzureServiceBusOptions == null) {
                yield return new ValidationResult("Azure AzureServiceBusOptions are required when Transport is AzureServiceBus.");
            }
        }
        if(ServiceBus == ServiceBusTransport.RabbitMQ) { 
            if(RabbitMQOptions == null) {
                yield return new ValidationResult("Azure RabbitMQOptions are required when Transport is RabbitMQ.");
            }
        }
    }
}
