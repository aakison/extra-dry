using ExtraDry.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json;

namespace ExtraDry.Core;

public class ListClient<TItem> : IListClient<TItem>
{
    public ListClient(HttpClient client, string entitiesEndpointTemplate, ILogger<ListClient<TItem>> iLogger, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        throw new NotImplementedException();
    }

    public ListClient(HttpClient client, ListClientOptions options, ILogger<ListClient<TItem>> iLogger)
    {
        http = client;
        logger = iLogger;
        Options = options;
        // Make default json to ignore case, most non-.NET "RESTful" services use camelCase...
        JsonSerializerOptions = options.JsonSerializerOptions ?? new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        ListType = typeof(Collection<TItem>);
        if(options.ListEndpoint != string.Empty) {
            if(options.ListMode == ListClientMode.Paged) {
                ListType = typeof(PagedCollection<TItem>);
                ListUnpacker = e => (e as PagedCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as PagedCollection<TItem>)?.Total ?? 0;
            }
            else if(options.ListMode == ListClientMode.Filtered) {
                ListType = typeof(FilteredCollection<TItem>);
                ListUnpacker = e => (e as FilteredCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as FilteredCollection<TItem>)?.Count ?? 0;
            }
            else {
                ListUnpacker = e => e as ICollection<TItem> ?? [];
                ListCounter = e => ListUnpacker(e)?.Count ?? 0;
            }
        }
    }

    public int PageSize => Options.PageSize;

    public bool IsLoading { get; private set; }

    public bool? IsEmpty { get; private set; }

    private ListClientOptions Options { get; set; }

    private Type ListType { get; set; }

    private Func<object, ICollection<TItem>>? ListUnpacker { get; set; }

    private Func<object, int>? ListCounter { get; set; }

    public int MaxLevel { get; private set; }

    public int MinLevel { get; private set; }

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    internal string ConstructApiEndpoint(Query query)
    {
        try {
            var queryParams = new Dictionary<string, List<string>>();
            AddIf(queryParams, Options.FilterParameterName, query.Filter);
            AddIf(queryParams, Options.SortParameterName, query.Sort);
            AddIf(queryParams, Options.SkipParameterName, query.Skip);
            AddIf(queryParams, Options.TakeParameterName, query.Take ?? Options.PageSize);
            return ConstructPathAndQuery(queryParams);
        }
        catch(FormatException ex) {
            throw new DryException("Formatting problem while construction List Endpoint", ex);
        }
    }

    private string ConstructPathAndQuery(Dictionary<string, List<string>> queryParams)
    {
        var endpoint = BaseApiEndpoint();
        if(queryParams.Count != 0) {
            var queries = queryParams.SelectMany(e => e.Value.Select(v => $"{e.Key}={Uri.EscapeDataString(v)}"));
            var query = string.Join("&", queries);
            endpoint = $"{endpoint}?{query}";
        }
        return endpoint;
    }

    private string BaseApiEndpoint()
    {
        var url = Options.ListEndpoint;
        if(EndpointKey is not null) {
            foreach(var formatter in Options.EndpointFormatters) {
                var parameterValue = formatter.Formatter(EndpointKey);
                url = formatter.Mode switch {
                    EndpointMode.Append => $"{url.TrimEnd('/')}/{parameterValue}",
                    EndpointMode.Replace => url.Replace($"{{{formatter.ParmeterName}}}", parameterValue),
                    EndpointMode.Generate => parameterValue,
                    _ => url
                };
                Console.WriteLine($"FUNC: {formatter.ParmeterName} {parameterValue}");
            }
        }
        Console.WriteLine($"FUNC: Result: {url}");
        return url.TrimEnd('/');
    }

    private static void AddIf(Dictionary<string, List<string>> keys, string key, string? value)
    {
        if(!string.IsNullOrWhiteSpace(value)) {
            keys.Add(key, [value]);
        }
    }

    private static void AddIf(Dictionary<string, List<string>> keys, string key, int? value)
    {
        if(value.HasValue && value.Value != 0) {
            keys.Add(key, [$"{value.Value}"]);
        }
    }

    public async ValueTask<ListClientResult<TItem>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        return await GetItemsAsync(new Query(), cancellationToken);
    }

    public async ValueTask<ListClientResult<TItem>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var result = await GetItemsInternalAsync(query, cancellationToken);
        return new ListClientResult<TItem>(result.Item2, result.Item2.Count, result.Item3);
    }

    internal async ValueTask<(object, ICollection<TItem>, int)> GetItemsInternalAsync(Query query, CancellationToken cancellationToken)
    {
        IsLoading = true;
        if(string.IsNullOrWhiteSpace(Options.ListEndpoint)) {
            throw new DryException(HttpStatusCode.NotFound, "No endpoints defined", "When configuring a ListService, must define ListEndpoint");
        }
        if(Options.EndpointFormatters.Count > 0 && EndpointKey is null) {
            throw new DryException($"Endpoint formatters defined but no EndpointKey was provided for endpoint template: {Options.ListEndpoint}");
        }
        var endpoint = ConstructApiEndpoint(query);
        logger.LogEndpointCall(typeof(TItem), endpoint);
        var response = await http.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogEndpointResult(typeof(TItem), endpoint, body, 100);
        var packedResult = JsonSerializer.Deserialize(body, ListType, JsonSerializerOptions)
            ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
        var items = ListUnpacker!(packedResult);
        var total = ListCounter!(packedResult);
        total = Math.Max(items.Count, total); // If deserializing a FilterCollection as a PagedCollection, use count as total.
        OnItemsLoaded?.Invoke(this, EventArgs.Empty);
        IsLoading = false;
        IsEmpty = total == 0;
        return (packedResult, items, total);
    }

    /// <summary>
    /// Use with Endpoint Formatters to provide contextual information for the endpoint.
    /// Required if formatters are used, ignored if not.
    /// </summary>
    public object? EndpointKey { get; set; }

    /// <summary>
    /// Event to subscribe to be notified when a list has returned results.
    /// </summary>
    public event EventHandler? OnItemsLoaded;

    private readonly HttpClient http;

    private readonly ILogger<ListClient<TItem>> logger;
}
