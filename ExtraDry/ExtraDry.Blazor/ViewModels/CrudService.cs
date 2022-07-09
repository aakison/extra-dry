#nullable enable

using ExtraDry.Blazor.Components;
using ExtraDry.Core.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExtraDry.Blazor;

public class CrudService<T> {

    public CrudService(HttpClient client, string entityEndpointTemplate, ILogger<CrudService<T>>? iLogger = null)
    {
        http = client;
        ApiTemplate = entityEndpointTemplate;
        logger = iLogger;
    }

    public string ApiTemplate { get; set; }

    public async Task CreateAsync(T item, params object[] args)
    {
        // TODO: Map DryException on server to DryException in Blazor for better messaging...
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint("POST", args);
        try {
            var response = await http.PostAsync(endpoint, content);
            await response.AssertSuccess();
        }
        catch(Exception ex) {
            Console.Error.WriteLine($"Server Side Error while processing CrudService.CreateAsync.\n" +
                $"API Endpoint: POST {endpoint}\n" +
                $"Error Response: {ex.Message}");
            throw;
        }
    }

    public async Task<T?> RetrieveAsync(object key, params object[] args)
    {
        var endpoint = ApiEndpoint("RetrieveAsync", key, args);
        logger?.LogInformation("Retrieving '{entity}' from '{endpoint}'", nameof(T), endpoint);
        Console.WriteLine(endpoint);
        var response = await http.GetAsync(endpoint);
        if(response.IsSuccessStatusCode) {
            var item = await response.Content.ReadFromJsonAsync<T>();
            logger?.LogDebug("Retrieved '{entity}' from '{endpoint}' with content: {content}", nameof(T), endpoint, item);
            return item;
        }
        else {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            logger?.LogDebug("Retrieve '{entity}' from '{endpoint}' failed with problem: {problem}", nameof(T), endpoint, problem);
            throw new DryException(problem);
        }
    }

    public async Task UpdateAsync(object key, T item, params object[] args)
    {
        // TODO: Map DryException on server to DryException in Blazor for better messaging...
        var endpoint = ApiEndpoint("UpdateAsync", key, args);
        Console.WriteLine(endpoint);
        try {
            var response = await http.PutAsJsonAsync(endpoint, item);
            await response.AssertSuccess();
        }
        catch(Exception ex) {
            Console.Error.WriteLine($" {ex.Message}");
        }
    }

    public async Task DeleteAsync(object key, params object[] args)
    {
        // TODO: Map DryException on server to DryException in Blazor for better messaging...
        var endpoint = ApiEndpoint("DeleteAsync", key, args);
        Console.WriteLine(endpoint);
        var response = await http.DeleteAsync(endpoint);
        await response.AssertSuccess();
    }

    private string ApiEndpoint(string method, object key, params object[] args)
    {
        try {
            var formatArgs = new List<object>(args);
            formatArgs.Insert(0, key);
            args = formatArgs.ToArray();
            return string.Format(ApiTemplate, args).TrimEnd('/');
        }
        catch(FormatException ex) {
            var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
            var message = $"Formatting problem while constructing endpoint for `CrudService.{method}`.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({ApiTemplate}), could not be satisifed with arguments ({argsFormatted}).  Inner Exception was:  {ex.Message}";
            Console.Error.WriteLine(message);
            throw new DryException(message, "Error occurred connecting to server.");
        }
    }

    private readonly HttpClient http;

    private readonly ILogger<CrudService<T>>? logger;
}


public static class HttpResponseMessageExtensions {

    internal static async Task AssertSuccess(this HttpResponseMessage response)
    {
        try {
            await response.EnsureSuccessStatusCodeAsync();
            // Handle success
        }
        catch(SimpleHttpResponseException exc) {
            // Handle failure
            var x2 = exc.Data;
        }

        if(!response.IsSuccessStatusCode) {
            var userMessage = "An unspecified error has occurred.";
            try {
                // Map server side validation messages.
                var str = await response.Content.ReadAsStringAsync();
                var message = await response.Content.ReadFromJsonAsync<ErrorContent>();
                if(message == null) {
                    userMessage = await response.Content.ReadAsStringAsync();
                }
                else {
                    var individualMessages = message.Errors.SelectMany(e => e.Value, (e, f) => f);
                    userMessage = string.Join("; ", individualMessages);
                }
            }
            catch(Exception ex) {
                // Just eat it.
                var x = ex;
            }
            throw new DryException(response?.ReasonPhrase ?? "Response failed.", userMessage);
        }
    }

    public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
    {
        if(response.IsSuccessStatusCode) {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();

        if(response.Content != null) {
            response.Content.Dispose();
        }

        var error = JsonSerializer.Deserialize<ErrorContent>(content);

        if(error == null) {
            throw new SimpleHttpResponseException(response.StatusCode, content);
        }
        else {
            var message = $"Validation issues: {string.Join(',', error.Errors.Keys)}";
            throw new DryException(message);
        }
    }
}

public class SimpleHttpResponseException : Exception {
    public HttpStatusCode StatusCode { get; private set; }

    public SimpleHttpResponseException(HttpStatusCode statusCode, string content) : base(content)
    {
        StatusCode = statusCode;
    }
}
