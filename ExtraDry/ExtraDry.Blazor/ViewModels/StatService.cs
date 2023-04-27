using ExtraDry.Blazor.Extensions;
using System.Net.Http.Json;

namespace ExtraDry.Blazor;

/// <summary>
/// A simple Stats API service wrapper for Extra Dry service endpoints.
/// The entity of Type `T must be JSON serializable and accepted by the server.
/// The return object will be a Statistics object of type `T
/// On non-success (2xx) results, the service endpoints should return a ProblemDetails (RFC7807)
/// response body.  This body will be unwrapped and throw in the body of a DryException.  If 
/// ProblemDetails are not present, then a trivial attempt to unpacke the arbitrary response 
/// payload will be made.
/// </summary>
public class StatService<T> {

    /// <summary>
    /// Create a stat service with the specified configuration. This service should not be 
    /// manually added to the IServiceCollection.  Instead, use the AddCrudService`T 
    /// extension method.
    /// </summary>
    /// <param name="client">A HttpClient object, typically from DI</param>
    /// <param name="collectionEndpointTemplate">
    /// The template for the API.  This is that path portion of the URI as the app can only call
    /// the server that it came from.  The endpoint may include placeholders for any number of 
    /// replacements, e.g. "{0}".  During construction of the final endpoint, these placeholders
    /// are used with `args` provided to each method to resolve the final endpoint.
    /// This allows for version numbers, tenant names, etc. to be added.
    /// </param>
    /// <param name="iLogger">An optional Logger</param>
    public StatService(HttpClient client, string collectionEndpointTemplate, ILogger<StatService<T>>? iLogger = null)
    {
        http = client;
        ApiTemplate = collectionEndpointTemplate;
        logger = iLogger;
    }

    /// <summary>
    /// The API Template used to determine the final endpoint URI.
    /// </summary>
    public string ApiTemplate { get; set; }

    /// <summary>
    /// Retrieves the Statistics of the Entity
    /// </summary>
    /// <param name="filter">The entity specific text filter for the collection.</param>
    /// <param name="args">The values to replace the placeholders in the collectionEndpointTemplate.</param>
    public async Task<Statistics<T>?> RetrieveAsync(string? filter, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(RetrieveAsync), filter, args);
        logger?.LogInformation("Retrieving '{entity}' from '{endpoint}'", nameof(T), endpoint);
        var response = await http.GetAsync(endpoint);
        await response.AssertSuccess();
        var item = await response.Content.ReadFromJsonAsync<Statistics<T>>();
        logger?.LogDebug("Retrieved '{entity}' from '{endpoint}' with content: {content}", nameof(T), endpoint, item);
        return item;
    }

    private string ApiEndpoint(string method, string? filter, params object[] args)
    {
        try {
            
            var baseUrl = string.Format(ApiTemplate, args);
            var url = $"{baseUrl}".TrimEnd('/');
            if(filter != null) {
                url += string.Format("?Filter={0}", filter);
            }
            return url;
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            logger?.LogWarning("Formatting problem while constructing endpoint for `StatService.{method}`.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({ApiTemplate}), could not be satisifed with arguments ({argsFormatted}).  Inner Exception was:  {ex.Message}", method, ApiTemplate, argsFormatted, ex.Message);
            throw new DryException("Error occurred connecting to server", "This is a mis-configuration and not a user error, please see the console output for more information.");
        }
    }
    private readonly HttpClient http;

    private readonly ILogger<StatService<T>>? logger;
}
