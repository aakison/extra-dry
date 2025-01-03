namespace Sample.Components.Agent;

public class AzureServiceBusOptions
{
    /// <summary>
    /// The namespace of the service bus.
    /// </summary>
    /// <remarks>
    /// <seealso
    /// cref="https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules"
    /// />
    /// </remarks>
    [Required, StringLength(50, MinimumLength = 6), RegularExpression(@"[a-zA-Z0-9\-]")]
    public string Namespace { get; set; } = "";

    [Required]
    public string KeyName { get; set; } = "";

    [Required, Secret]
    public string Key { get; set; } = "";

    public string ConnectionString => $"Endpoint=sb://{Namespace}.servicebus.windows.net/;SharedAccessKeyName={KeyName};SharedAccessKey={Key}";
}
