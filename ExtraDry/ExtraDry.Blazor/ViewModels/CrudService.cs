using ExtraDry.Blazor.Extensions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExtraDry.Blazor;

/// <summary>
/// A simple CRUD API service wrapper for Extra Dry service endpoints.  This wrapper assumes that 4
/// endpoints exist following standard RESTful principles for endpoints.  The entity of Type `T 
/// must be JSON serializable and accepted by the server.  On non-success (2xx) results, the 
/// service endpoints should return a ProblemDetails (RFC7807)  response body.  This body will be 
/// unwrapped and throw in the body of a DryException.  If ProblemDetails are not present, then a 
/// trivial attempt to unpack the arbitrary response payload will be made.
/// </summary>
/// <remarks>
/// Create a CRUD service with the specified configuration.  This service should not be 
/// manually added to the IServiceCollection.  Instead, use the AddCrudService`T 
/// extension method.
/// </remarks>
public class CrudService<T>(
    HttpClient client,
    CrudServiceOptions options,
    ILogger<CrudService<T>> logger)
{
    public CrudServiceOptions Options { get; } = options;

    public async Task CreateAsync(T item, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(string.Empty);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess(logger);
    }

    public async Task<T?> TryRetrieveAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        return item;
    }

    public async Task<T> RetrieveAsync(object key, CancellationToken cancellationToken = default)
    {
        return await TryRetrieveAsync(key, cancellationToken)
            ?? throw new ArgumentOutOfRangeException(nameof(key), $"Item not found for key {key}");
    }

    public async Task UpdateAsync(object key, T item, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.PutAsJsonAsync(endpoint, item, cancellationToken);
        await response.AssertSuccess(logger);
    }

    public async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);
        logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.DeleteAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
    }

    private string ApiEndpoint(object key)
    {
        var url = $"{Options.CrudEndpoint.TrimEnd('/')}/{key}".TrimEnd('/');
        return url;
    }
}
