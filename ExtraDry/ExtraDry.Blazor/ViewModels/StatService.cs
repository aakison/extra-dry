using ExtraDry.Blazor.Extensions;
using System.Text.Json;

namespace ExtraDry.Blazor;

/// <summary>
/// A simple Stats API service wrapper for Extra Dry service endpoints.  The entity of Type `T 
/// must be JSON serializable and accepted by the server. The return object will be a Statistics 
/// object of type `T On non-success (2xx) results, the service endpoints should return a 
/// ProblemDetails (RFC7807) response body. This body will be unwrapped and throw in the body of a 
/// DryException. If ProblemDetails are not present, then a trivial attempt to unpack the arbitrary
/// response payload will be made.
/// </summary>
/// <remarks>
/// Create a stat service with the specified configuration. This service should not be manually 
/// added to the IServiceCollection.  Instead, use the AddCrudService`T extension method.
/// </remarks>
public class StatService<T>(
    HttpClient client,
    StatServiceOptions options,
    ILogger<StatService<T>> logger)
{
    public StatServiceOptions Options { get; } = options;

    /// <summary>
    /// Retrieves the Statistics of the Entity
    /// </summary>
    /// <param name="filter">The entity specific text filter for the collection.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    public async Task<Statistics<T>?> RetrieveAsync(string? filter, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(filter);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogEndpointResult(typeof(T), endpoint, body);
        var item = JsonSerializer.Deserialize<Statistics<T>>(body, Options.JsonSerializerOptions);
        return item;
    }

    private string ApiEndpoint(string? filter)
    {
        var url = Options.StatEndpoint;
        if(filter != null) {
            url += $"?Filter={filter}";
        }
        return url;
    }
}
