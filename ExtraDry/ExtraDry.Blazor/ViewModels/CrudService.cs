using ExtraDry.Blazor.Extensions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExtraDry.Blazor;

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
    /// <param name="client">A HttpClient object, typically from DI</param>
    /// <param name="collectionEndpointTemplate">
    /// The template for the API.  This is that path portion of the URI as the app can only call
    /// the server that it came from.  This is used directly for Create methods and is the stem
    /// for Retrieve/Update/Delete methods where the Id is appended to the template.
    /// Additionally the endpoint may include placeholders for any number of 
    /// replacements, e.g. "{0}".  During construction of the final endpoint, these placeholders
    /// are used with `args` provided to each method to resolve the final endpoint.
    /// This allows for version numbers, tenant names, etc. to be added.
    /// </param>
    /// <param name="iLogger">An optional Logger</param>
    [Obsolete("Use Options")]
    public CrudService(HttpClient client, string collectionEndpointTemplate, ILogger<CrudService<T>>? iLogger = null)
    {
        http = client;
        Options = new CrudServiceOptions { CrudEndpoint = collectionEndpointTemplate };
        ApiTemplate = collectionEndpointTemplate;
        logger = iLogger;
    }

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
        ApiTemplate = options.CrudEndpoint;
    }

    /// <summary>
    /// The API Template used to determine the final endpoint URI.
    /// </summary>
    [Obsolete("Use from Options")]
    public string ApiTemplate { get; set; }

    public CrudServiceOptions Options { get; }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task CreateAsync(T item, params object[] args)
    {
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(nameof(CreateAsync), string.Empty, args);
        logger?.LogInformation("Created '{entity}' at '{endpoint}'", nameof(T), endpoint);
        var response = await http.PostAsync(endpoint, content);
        await response.AssertSuccess();
        logger?.LogDebug("Created '{entity}' on '{endpoint}' with content: {content}", nameof(T), endpoint, json);
    }

    public async Task CreateAsync(T item, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(nameof(CreateAsync), string.Empty);
        logger?.LogInformation("Created '{entity}' at '{endpoint}'", nameof(T), endpoint);
        var response = await http.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess();
        logger?.LogDebug("Created '{entity}' on '{endpoint}' with content: {content}", nameof(T), endpoint, json);
    }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task<T?> RetrieveAsync(object key, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(RetrieveAsync), key, args);
        logger?.LogInformation("Retrieving '{entity}' from '{endpoint}'", nameof(T), endpoint);
        var response = await http.GetAsync(endpoint);
        await response.AssertSuccess();
        var item = await response.Content.ReadFromJsonAsync<T>();
        logger?.LogDebug("Retrieved '{entity}' from '{endpoint}' with content: {content}", nameof(T), endpoint, item);
        return item;
    }

    public async Task<T?> RetrieveAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(RetrieveAsync), key);
        logger?.LogInformation("Retrieving '{entity}' from '{endpoint}'", nameof(T), endpoint);
        var response = await http.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess();
        var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        logger?.LogDebug("Retrieved '{entity}' from '{endpoint}' with content: {content}", nameof(T), endpoint, item);
        return item;
    }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task UpdateAsync(object key, T item, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(UpdateAsync), key, args);
        logger?.LogInformation("Updating '{entity}' on '{endpoint}'", nameof(T), endpoint);
        var response = await http.PutAsJsonAsync(endpoint, item);
        await response.AssertSuccess();
        logger?.LogDebug("Updated '{entity}' on '{endpoint}' with content: {content}", nameof(T), endpoint, item);
    }

    public async Task UpdateAsync(object key, T item, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(UpdateAsync), key);
        logger?.LogInformation("Updating '{entity}' on '{endpoint}'", nameof(T), endpoint);
        var response = await http.PutAsJsonAsync(endpoint, item, cancellationToken);
        await response.AssertSuccess();
        logger?.LogDebug("Updated '{entity}' on '{endpoint}' with content: {content}", nameof(T), endpoint, item);
    }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public async Task DeleteAsync(object key, params object[] args)
    {
        var endpoint = ApiEndpoint(nameof(DeleteAsync), key, args);
        logger?.LogInformation("Deleting '{entity}' at '{endpoint}'", nameof(T), endpoint);
        var response = await http.DeleteAsync(endpoint);
        await response.AssertSuccess();
        logger?.LogDebug("Deleted '{entity}' at '{endpoint}'", nameof(T), endpoint);
    }

    public async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(nameof(DeleteAsync), key);
        logger?.LogInformation("Deleting '{entity}' at '{endpoint}'", nameof(T), endpoint);
        var response = await http.DeleteAsync(endpoint, cancellationToken);
        await response.AssertSuccess();
        logger?.LogDebug("Deleted '{entity}' at '{endpoint}'", nameof(T), endpoint);
    }

    private string ApiEndpoint(string method, object key, params object[] args)
    {
        try {
            var baseUrl = string.Format(Options.CrudEndpoint, args);
            var url = $"{baseUrl}/{key}".TrimEnd('/');
            return url;
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            logger?.LogWarning("Formatting problem while constructing endpoint for `CrudService.{method}`.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({ApiTemplate}), could not be satisfied with arguments ({argsFormatted}).  Inner Exception was:  {ex.Message}", method, ApiTemplate, argsFormatted, ex.Message);
            throw new DryException("Error occurred connecting to server", "This is a mis-configuration and not a user error, please see the console output for more information.");
        }
    }

    private readonly HttpClient http;

    private readonly ILogger<CrudService<T>>? logger;
}
