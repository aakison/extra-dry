using ExtraDry.Core.Extensions;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ExtraDry.Blazor;

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
        HierarchyType = typeof(HierarchyCollection<TItem>);
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
        if(options.HierarchyEndpoint != string.Empty) {
            if(options.HierarchyMode == HierarchyServiceMode.FilterAndPage) {
                HierarchyType = typeof(PagedHierarchyCollection<TItem>);
                HierarchyUnpacker = e => (e as PagedHierarchyCollection<TItem>)?.Items ?? [];
                HierarchyCounter = e => (e as PagedHierarchyCollection<TItem>)?.Total ?? 0;
                HierarchyMaxLevel = e => (e as PagedHierarchyCollection<TItem>)?.MaxLevels ?? 0;
                HierarchyMinLevel = e => (e as PagedHierarchyCollection<TItem>)?.MinLevels ?? 0;
            }
            else if(options.HierarchyMode == HierarchyServiceMode.Filter) {
                HierarchyUnpacker = e => (e as HierarchyCollection<TItem>)?.Items ?? [];
                HierarchyCounter = e => (e as HierarchyCollection<TItem>)?.Count ?? 0;
                HierarchyMaxLevel = e => (e as HierarchyCollection<TItem>)?.MaxLevels ?? 0;
                HierarchyMinLevel = e => (e as PagedHierarchyCollection<TItem>)?.MinLevels ?? 0;
            }
        }
    }

    public int PageSize => Options.PageSize;

    private ListServiceOptions Options { get; set; }

    private Type ListType { get; set; }

    private Func<object, ICollection<TItem>>? ListUnpacker { get; set; }

    private Func<object, int>? ListCounter { get; set; }

    private Type HierarchyType { get; set; }

    private Func<object, ICollection<TItem>>? HierarchyUnpacker { get; set; }

    private Func<object, int>? HierarchyCounter { get; set; }

    private Func<object, int>? HierarchyMaxLevel { get; set; }

    private Func<object, int>? HierarchyMinLevel { get; set; }

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

    private string HierarchyEndpoint(Query query)
    {
        try {
            var keys = new Dictionary<string, List<string>>();
            if(Options.HierarchyMethod == HttpMethod.Get) {
                AddIf(keys, Options.LevelParameterName, query.Level);
                AddIf(keys, Options.FilterParameterName, query.Filter);
                AddIf(keys, Options.SkipParameterName, query.Skip);
                AddIf(keys, Options.TakeParameterName, Options.PageSize);
                AddIf(keys, Options.ExpandParameterName, query.Expand);
                AddIf(keys, Options.CollapseParameterName, query.Collapse);
            }
            return ConstructPathAndQuery(Options.HierarchyEndpoint, keys);
        }
        catch(FormatException ex) {
            throw new DryException("Formatting problem while construction Hierarchy Endpoint", ex);
        }
    }

    private string HierarchyRequestBody(Query query)
    {
        var body = new PageHierarchyQuery {
            Level = query.Level ?? 3,
            Filter = query.Filter,
            Skip = query.Skip ?? 0,
            Take = Options.PageSize,
        };
        return JsonSerializer.Serialize(body, JsonSerializerOptions);
    }

    private static void AddIf(Dictionary<string, List<string>> keys, string key, string[]? values)
    {
        if((values?.Length ?? 0) > 0) {
            keys.Add(key, values?.ToList() ?? []);
        }
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

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(CancellationToken cancellationToken)
    {
        var query = new Query { Source = ListSource.List };
        return await GetItemsAsync(query, cancellationToken);
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string filter, CancellationToken token)
    {
        var query = new Query { Source = ListSource.List, Filter = filter };
        return await GetItemsAsync(query, token);
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var result = await GetItemsInternalAsync(query, cancellationToken);
        return new ItemsProviderResult<TItem>(result.Item2, result.Item3);
    }

    public async ValueTask<ListItemsProviderResult<TItem>> GetListItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var result = await GetItemsInternalAsync(query, cancellationToken);
        var collection = (BaseCollection<TItem>)result.Item1;
        return new ListItemsProviderResult<TItem>(collection);
    }

    public async ValueTask<(object, ICollection<TItem>, int)> GetItemsInternalAsync(Query query, CancellationToken cancellationToken)
    {
        var source = (Options.ListEndpoint, Options.HierarchyEndpoint) switch {
            ("", "") => throw new DryException(HttpStatusCode.NotFound, "No endpoints defined", "When configuring a ListService, either or both of ListEndpoint and/or HierarchyEndpoint must be provided."),
            ("", _) => ListSource.Hierarchy,
            (_, "") => ListSource.List,
            (_, _) => query.Source,
        };
        var endpoint = source == ListSource.Hierarchy ? HierarchyEndpoint(query) : ListEndpoint(query);
        if(source == ListSource.Hierarchy) {
            logger.LogEndpointCall(typeof(TItem), endpoint);
            var response = Options.HierarchyMethod == HttpMethod.Get
                ? await http.GetAsync(endpoint, cancellationToken)
                : await http.PostAsync(endpoint, new StringContent(HierarchyRequestBody(query)), cancellationToken);
            await response.AssertSuccess(logger);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogEndpointResult(typeof(TItem), endpoint, body);
            var packedResult = JsonSerializer.Deserialize(body, HierarchyType, JsonSerializerOptions)
                ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
            var items = HierarchyUnpacker!(packedResult);
            var total = HierarchyCounter!(packedResult);
            MaxLevel = HierarchyMaxLevel!(packedResult);
            MinLevel = HierarchyMinLevel!(packedResult);
            return (packedResult, items, total);
        }
        else {
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
    }

    private readonly HttpClient http;

    private readonly ILogger<ListService<TItem>> logger;
}
