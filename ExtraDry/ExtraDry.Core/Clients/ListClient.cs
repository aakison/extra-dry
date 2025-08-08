using ExtraDry.Core.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
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
            if(options.ListMode == ListClientMode.FilterSortAndPage) {
                ListType = typeof(PagedCollection<TItem>);
                ListUnpacker = e => (e as PagedCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as PagedCollection<TItem>)?.Total ?? 0;
            }
            else if(options.ListMode == ListClientMode.FilterAndSort) {
                ListType = typeof(SortedCollection<TItem>);
                ListUnpacker = e => (e as SortedCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as SortedCollection<TItem>)?.Count ?? 0;
            }
            else if(options.ListMode == ListClientMode.Filter) {
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

    private ListClientOptions Options { get; set; }

    private Type ListType { get; set; }

    private Func<object, ICollection<TItem>>? ListUnpacker { get; set; }

    private Func<object, int>? ListCounter { get; set; }

    public int MaxLevel { get; private set; }

    public int MinLevel { get; private set; }

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    public Dictionary<string, string> Variables { get; } = [];

    internal string ListEndpoint(Query query)
    {
        try {
            var keys = new Dictionary<string, List<string>>();
            AddIf(keys, Options.FilterParameterName, query.Filter);
            AddIf(keys, Options.SortParameterName, query.Sort);
            AddIf(keys, Options.SkipParameterName, query.Skip);
            AddIf(keys, Options.TakeParameterName, Options.PageSize);
            return ConstructPathAndQuery(Options.ListEndpoint, keys);
        }
        catch(FormatException ex) {
            throw new DryException("Formatting problem while construction List Endpoint", ex);
        }
    }

    private static string ConstructPathAndQuery(string path, Dictionary<string, List<string>> keys)
    {
        if(keys.Count != 0) {
            var queries = keys.SelectMany(e => e.Value.Select(v => $"{e.Key}={Uri.EscapeDataString(v)}"));
            var query = string.Join("&", queries);
            path = $"{path}?{query}";
        }
        return path;
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
        if(string.IsNullOrWhiteSpace(Options.ListEndpoint)) {
            throw new DryException(HttpStatusCode.NotFound, "No endpoints defined", "When configuring a ListService, either or both of ListEndpoint and/or HierarchyEndpoint must be provided.");
        }
        var endpoint = ListEndpoint(query);
        foreach(var variable in Variables) {
            endpoint = endpoint.Replace($"{{{variable.Key}}}", variable.Value, StringComparison.OrdinalIgnoreCase);
        }
        logger.LogEndpointCall(typeof(TItem), endpoint);
        var response = await http.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogEndpointResult(typeof(TItem), endpoint, body);
        var packedResult = JsonSerializer.Deserialize(body, ListType, JsonSerializerOptions)
            ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
        var items = ListUnpacker!(packedResult);
        var total = ListCounter!(packedResult);
        OnItemsLoaded?.Invoke(this, EventArgs.Empty);
        return (packedResult, items, total);
    }

    /// <summary>
    /// Event to subscribe to be notified when a list has returned results.
    /// </summary>
    public event EventHandler? OnItemsLoaded;

    private readonly HttpClient http;

    private readonly ILogger<ListClient<TItem>> logger;
}
