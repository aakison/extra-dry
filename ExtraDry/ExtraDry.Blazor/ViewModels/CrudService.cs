using ExtraDry.Blazor.Extensions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExtraDry.Blazor;

using Microsoft.Extensions.Logging;

/// <summary>
/// A simple CRUD API service wrapper for Extra Dry service endpoints.
/// This wrapper assumes that 4 endpoints exist following standard RESTful principles for endpoints.
/// The entity of Type `T must be JSON serializable and accepted by the server.
/// On non-success (2xx) results, the service endpoints should return a ProblemDetails (RFC7807)
/// response body.  This body will be unwrapped and throw in the body of a DryException.  If 
/// ProblemDetails are not present, then a trivial attempt to unpack the arbitrary response 
/// payload will be made.
/// </summary>
public class CrudService<T> {

    /// <summary>
    /// Create a CRUD service with the specified configuration.  This service should not be 
    /// manually added to the IServiceCollection.  Instead, use the AddCrudService`T 
    /// extension method.
    /// </summary>
    public CrudService(HttpClient client, CrudServiceOptions options, ILogger<CrudService<T>> logger)
    {
        http = client;
        Options = options;
        this.logger = logger;
    }

    public CrudServiceOptions Options { get; }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task CreateAsync(T item, params object[] args)
    {
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(nameof(CreateAsync), string.Empty, args);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.PostAsync(endpoint, content);
        await response.AssertSuccess(logger);
    }

    public async Task CreateAsync(T item, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(nameof(CreateAsync), string.Empty);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess(logger);
    }

    [Obsolete("Inject arguments into HttpClient derived type")]
    public async Task<T?> RetrieveAsync(object key, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(RetrieveAsync), key, args);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.GetAsync(endpoint);
        await response.AssertSuccess(logger);
        var item = await response.Content.ReadFromJsonAsync<T>();
        return item;
    }

    public async Task<T?> RetrieveAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(RetrieveAsync), key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        return item;
    }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task UpdateAsync(object key, T item, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(UpdateAsync), key, args);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.PutAsJsonAsync(endpoint, item);
        await response.AssertSuccess(logger);
    }

    public async Task UpdateAsync(object key, T item, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(UpdateAsync), key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.PutAsJsonAsync(endpoint, item, cancellationToken);
        await response.AssertSuccess(logger);
    }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task DeleteAsync(object key, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(DeleteAsync), key, args);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.DeleteAsync(endpoint);
        await response.AssertSuccess(logger);
    }

    public async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(DeleteAsync), key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await http.DeleteAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
    }

    private string ApiEndpoint(string method, object key, params object[] args)
    {
        try {
            var baseUrl = string.Format(CultureInfo.InvariantCulture, Options.CrudEndpoint, args);
            var url = $"{baseUrl}/{key}".TrimEnd('/');
            return url;
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            logger.LogFormattingError(typeof(T), Options.CrudEndpoint, argsFormatted, ex, method);
            throw new DryException("Error occurred connecting to server", "This is a mis-configuration and not a user error, please see the console output for more information.");
        }
    }

    private readonly HttpClient http;

    private readonly ILogger<CrudService<T>> logger;
}
