namespace Sample.Components.Api.Options;

/// <summary>
/// All options for the API.
/// </summary>
public partial class ApiOptions
{
    /// <summary>
    /// The name of the section in the configuration.
    /// </summary>
    public const string SectionName = "ApiOptions";

    /// <inheritdoc cref="CosmosDbOptions" />
    public CosmosDbOptions CosmosDb { get; set; } = new();
}
