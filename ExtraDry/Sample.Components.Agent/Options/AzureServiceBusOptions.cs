using ExtraDry.Server.Agents;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Sample.Components.Agent;

public class AzureServiceBusOptions
{
    [Required]
    public string Namespace { get; set; } = "";

    [Required]
    public string KeyName { get; set; } = "";

    [Required, Secret]
    public string Key { get; set; } = "";

    public string ConnectionString => $"Endpoint=sb://{Namespace}.servicebus.windows.net/;SharedAccessKeyName={KeyName};SharedAccessKey={Key}";
}
