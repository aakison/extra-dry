using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace ExtraDry.Blazor;

public class ListService<TItem> : IListService<TItem> {

    public ListService(HttpClient client, string entitiesEndpointTemplate, ILogger<ListService<TItem>> iLogger, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        throw new NotImplementedException();
    }

    public ListService(HttpClient client, ListServiceOptions options, ILogger<ListService<TItem>> iLogger)
    {
        http = client;
        logger = iLogger;
        Options = options;
        UriTemplate = options.ListEndpoint;
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
                ListUnpacker = e => (e as PagedCollection<TItem>)?.Items ?? new Collection<TItem>();
                ListCounter = e => (e as PagedCollection<TItem>)?.Total ?? 0;
            }
            else if(options.ListMode == ListServiceMode.FilterAndSort) {
                ListType = typeof(SortedCollection<TItem>);
                ListUnpacker = e => (e as SortedCollection<TItem>)?.Items ?? new Collection<TItem>();
                ListCounter = e => (e as SortedCollection<TItem>)?.Count ?? 0;
            }
            else if(options.ListMode == ListServiceMode.Filter) {
                ListType = typeof(FilteredCollection<TItem>);
                ListUnpacker = e => (e as FilteredCollection<TItem>)?.Items ?? new Collection<TItem>();
                ListCounter = e => (e as FilteredCollection<TItem>)?.Count ?? 0;
            }
            else {
                ListUnpacker = e => e as ICollection<TItem> ?? new Collection<TItem>();
                ListCounter = e => ListUnpacker(e)?.Count ?? 0;
            }
        }
        if(options.HierarchyEndpoint != string.Empty) {
            if(options.HierarchyMode == HierarchyServiceMode.FilterAndPage) {
                HierarchyType = typeof(PagedHierarchyCollection<TItem>);
                HierarchyUnpacker = e => (e as PagedHierarchyCollection<TItem>)?.Items ?? new Collection<TItem>();
                HierarchyCounter = e => (e as PagedHierarchyCollection<TItem>)?.Total ?? 0;
                HierarchyMaxLevel = e => (e as PagedHierarchyCollection<TItem>)?.MaxLevels ?? 0;
            }
            else if(options.HierarchyMode == HierarchyServiceMode.Filter) {
                HierarchyUnpacker = e => (e as HierarchyCollection<TItem>)?.Items ?? new Collection<TItem>();
                HierarchyCounter = e => (e as HierarchyCollection<TItem>)?.Count ?? 0;
                HierarchyMaxLevel = e => (e as HierarchyCollection<TItem>)?.MaxLevels ?? 0;
            }
        }
    }

    public int PageSize => Options.PageSize;

    private ListServiceOptions Options { get; set; }

    [Obsolete("Use from Options")]
    public string UriTemplate { get; set; }

    private Type ListType { get; set; }

    private Func<object, ICollection<TItem>>? ListUnpacker { get; set; }

    private Func<object, int>? ListCounter { get; set; }

    private Type HierarchyType { get; set; }

    private Func<object, ICollection<TItem>>? HierarchyUnpacker { get; set; }

    private Func<object, int>? HierarchyCounter { get; set; }

    private Func<object, int>? HierarchyMaxLevel { get; set; }

    public int MaxLevel { get; private set; }

    [Obsolete("Inject arguments into HtttpClient derived type")]
    public object[] UriArguments { get; set; } = Array.Empty<object>();

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    internal string ListEndpoint(Query query)
    {
        try {
            var keys = new Dictionary<string, string>();
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

    private static string ConstructPathAndQuery(string path, Dictionary<string, string> keys)
    {
        if(keys.Any()) {
            var queries = keys.Select(e => $"{e.Key}={Uri.EscapeDataString(e.Value)}");
            var query = string.Join("&", queries);
            path = $"{path}?{query}";
        }
        return path;
    }

    private string HierarchyEndpoint(Query query)
    {
        try {
            var keys = new Dictionary<string, string>();
            if(Options.HierarchyMethod == HttpMethod.Get) {
                AddIf(keys, Options.LevelParameterName, query.Level);
                AddIf(keys, Options.FilterParameterName, query.Filter);
                AddIf(keys, Options.SkipParameterName, query.Skip);
                AddIf(keys, Options.TakeParameterName, Options.PageSize);
                // TODO: Expand Collapse Nodes
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

    private static void AddIf(Dictionary<string, string> keys, string key, string? value)
    {
        if(!string.IsNullOrWhiteSpace(value)) {
            keys.Add(key, value);
        }
    }

    private static void AddIf(Dictionary<string, string> keys, string key, int? value)
    {
        if(value.HasValue && value.Value != 0) {
            keys.Add(key, value.Value.ToString());
        }
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(CancellationToken token)
    {
        var query = new Query { Source = ListSource.List };
        return await GetItemsAsync(query, token);
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string filter, CancellationToken token)
    {
        var query = new Query { Source = ListSource.List, Filter = filter };
        return await GetItemsAsync(query, token);
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var source = (Options.ListEndpoint, Options.HierarchyEndpoint) switch {
            ("", "") => throw new Exception("No endpoints defined"),
            ("", _) => ListSource.Hierarchy,
            (_, "") => ListSource.List,
            (_, _) => query.Source,
        };
        var endpoint = source == ListSource.Hierarchy ? HierarchyEndpoint(query) : ListEndpoint(query);
        if(source == ListSource.Hierarchy) {
            logger.LogInformation("ListService.GetItemsAsync from {endpoint}", endpoint);
            var response = Options.HierarchyMethod == HttpMethod.Get
                ? await http.GetAsync(endpoint, cancellationToken)
                : await http.PostAsync(endpoint, new StringContent(HierarchyRequestBody(query)), cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogInformation("ListService.GetItemsAsync retrieved {body}", body);
            var packedResult = JsonSerializer.Deserialize(body, HierarchyType, JsonSerializerOptions)
                ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
            var items = HierarchyUnpacker!(packedResult);
            var total = HierarchyCounter!(packedResult);
            MaxLevel = HierarchyMaxLevel!(packedResult);
            return new ItemsProviderResult<TItem>(items, total);
        }
        else {
            logger.LogInformation("ListService.GetItemsAsync from {endpoint}", endpoint);
            var body = await http.GetStringAsync(endpoint, cancellationToken);
            logger.LogInformation("ListService.GetItemsAsync retrieved {body}", body);
            var packedResult = JsonSerializer.Deserialize(body, ListType, JsonSerializerOptions)
                ?? throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
            var items = ListUnpacker!(packedResult);
            var total = ListCounter!(packedResult);
            return new ItemsProviderResult<TItem>(items, total);
        }
    }

    private readonly HttpClient http;

    private readonly ILogger<ListService<TItem>> logger;

}
