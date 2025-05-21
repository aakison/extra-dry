using ExtraDry.Core.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;

namespace ExtraDry.Core;

public class ListService<TItem> : IListService<TItem>
{
    public ListService(HttpClient client, string entitiesEndpointTemplate, ILogger<ListService<TItem>> iLogger, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        throw new NotImplementedException();
    }

    public ListService(HttpClient client, ListServiceOptions options, ILogger<ListService<TItem>> iLogger)
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
            if(options.ListMode == ListServiceMode.FilterSortAndPage) {
                ListType = typeof(PagedCollection<TItem>);
                ListUnpacker = e => (e as PagedCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as PagedCollection<TItem>)?.Total ?? 0;
            }
            else if(options.ListMode == ListServiceMode.FilterAndSort) {
                ListType = typeof(SortedCollection<TItem>);
                ListUnpacker = e => (e as SortedCollection<TItem>)?.Items ?? [];
                ListCounter = e => (e as SortedCollection<TItem>)?.Count ?? 0;
            }
            else if(options.ListMode == ListServiceMode.Filter) {
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

    private ListServiceOptions Options { get; set; }

    private Type ListType { get; set; }

    private Func<object, ICollection<TItem>>? ListUnpacker { get; set; }

    private Func<object, int>? ListCounter { get; set; }

    public int MaxLevel { get; private set; }

    public int MinLevel { get; private set; }

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

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

    public async ValueTask<ListServiceResult<TItem>> GetItemsAsync(Query query, CancellationToken cancellationToken)
    {
        var result = await GetItemsInternalAsync(query, cancellationToken);
        return new ListServiceResult<TItem>(result.Item2, result.Item2.Count, result.Item3);
    }

    internal async ValueTask<(object, ICollection<TItem>, int)> GetItemsInternalAsync(Query query, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(Options.ListEndpoint)) {
            throw new DryException(HttpStatusCode.NotFound, "No endpoints defined", "When configuring a ListService, either or both of ListEndpoint and/or HierarchyEndpoint must be provided.");
        }
        var endpoint = ListEndpoint(query);
        logger.LogEndpointCall(typeof(TItem), endpoint);
        var response = await http.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogEndpointResult(typeof(TItem), endpoint, body);
        var packedResult = JsonSerializer.Deserialize(body, ListType, JsonSerializerOptions)
            ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
        var items = ListUnpacker!(packedResult);
        var total = ListCounter!(packedResult);
        return (packedResult, items, total);
    }

    private readonly HttpClient http;

    private readonly ILogger<ListService<TItem>> logger;
}
