using ExtraDry.Core;
using ExtraDry.Server.Agents;
using System.ComponentModel.DataAnnotations;

namespace Sample.Components.Api.Options;

/// <summary>
/// The CosmosDB options for the data connection.
/// </summary>
public class CosmosDbOptions
{
    /// <summary>
    /// The name of the server that CosmosDB is running on.
    /// </summary>
    [Required, ServerName]
    public string Server { get; set; } = "localhost";

    /// <summary>
    /// The port that the CosmosDB server is listening on.
    /// </summary>
    [Required, Range(1, 65535)]
    public int Port { get; set; } = 8081;

    /// <summary>
    /// The authorization key for the Cosmos DB account.
    /// </summary>
    /// <remarks>
    /// The default key here is not a secret.  It is the well-known key for the CosmosDB emulator for testing purposes.
    /// </remarks>
    [Secret]
    [Required, Base64String]
    public string AuthKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    /// <summary>
    /// The name of the database within the CosmosDB account for this microservice.
    /// </summary>
    [Required]
    public string DatabaseName { get; set; } = "Components";

    /// <summary>
    /// The fully qualified endpoint that the CosmosDB server is listening on, constructed from other settings.
    /// </summary>
    public string Endpoint => $"https://{Server}:{Port}/";

    /// <summary>
    /// The fully qualified connection string for the CosmosDB account, constructed from other settings.
    /// </summary>
    public string ConnectionString => $"AccountEndpoint={Endpoint};AccountKey={AuthKey};";

}
