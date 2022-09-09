using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;

namespace ExtraDry.Blazor;

public class ListService<TCollection, TItem> : IListService<TItem> {

    public ListService(HttpClient client, string entitiesEndpointTemplate, ILogger<ListService<TCollection, TItem>> iLogger, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        http = client;
        logger = iLogger;
        UriTemplate = entitiesEndpointTemplate;
        // Make default json to ignore case, most non-.NET "RESTful" services use camelCase...
        JsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        if(typeof(TCollection).IsAssignableTo(typeof(ICollection<TItem>))) {
            Unpacker = e => e as ICollection<TItem> ?? new Collection<TItem>();
            Counter = e => Unpacker(e)?.Count ?? 0;
        }
        else if(typeof(TCollection).IsAssignableTo(typeof(FilteredCollection<TItem>))) {
            Unpacker = e => (e as FilteredCollection<TItem>)?.Items ?? new Collection<TItem>();
            Counter = e => (e as FilteredCollection<TItem>)?.Count ?? 0;
        }
        else if(typeof(TCollection).IsAssignableTo(typeof(PagedCollection<TItem>))) {
            Unpacker = e => (e as PagedCollection<TItem>)?.Items ?? new Collection<TItem>();
            Counter = e => (e as PagedCollection<TItem>)?.Total ?? 0;
        }
        else {
            Unpacker = e => new Collection<TItem>();
            Counter = e => 0;
        }
    }

    public string UriTemplate { get; set; }

    public int FetchSize { get; set; } = 100;

    public string FilterQueryParam { get; set; } = "filter";

    public string SortQueryParam { get; set; } = "sort";

    public string SkipQueryParam { get; set; } = "skip";

    public string TakeQueryParam { get; set; } = "take";


    private Func<TCollection, ICollection<TItem>> Unpacker { get; set; }

    private Func<TCollection, int> Counter { get; set; }

    public object[] UriArguments { get; set; } = Array.Empty<object>();

    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    public string ListEndpoint(string? filter, string? sort, bool? ascending, int? skip, int? take)
    {
        try {
            var path = string.Format(CultureInfo.InvariantCulture, UriTemplate, UriArguments);
            var keys = new Dictionary<string, string>();
            if(!string.IsNullOrWhiteSpace(filter)) {
                keys.Add(FilterQueryParam, filter);
            }
            if(!string.IsNullOrWhiteSpace(sort)) {
                keys.Add(SortQueryParam, sort);
                if(ascending.HasValue) {
                    keys.Add("ascending", ascending.Value.ToString());
                }
            }
            if(skip.HasValue && skip.Value > 0) {
                keys.Add(SkipQueryParam, skip.Value.ToString());
            }
            if(take.HasValue && take.Value > 0 && take != int.MaxValue) {
                keys.Add(TakeQueryParam, take.Value.ToString());
            }
            if(keys.Any()) {
                var queries = keys.Select(e => $"{e.Key}={Uri.EscapeDataString(e.Value)}");
                var query = string.Join("&", queries);
                path = $"{path}?{query}";
            }
            return path;
        }
        catch(FormatException ex) {
            throw new DryException(ex.Message, $"Formatting problem while construction List/Create Endpoint: {ex.Message}");
        }
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(CancellationToken token)
    {
        return await GetItemsAsync(null, null, null, 0, int.MaxValue, token);
    }

    [Obsolete("Use the version that contains a filter.")]
    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string? sort, bool? ascending, int? skip, int? take, CancellationToken cancellationToken)
    {
        return await GetItemsAsync(null, sort, ascending, skip, take, cancellationToken);
    }

    public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string? filter, string? sort, bool? ascending, int? skip, int? take, CancellationToken cancellationToken)
    {
        var endpoint = ListEndpoint(filter, sort, ascending, skip, take);
        logger.LogInformation("ListService.GetItems from {endpoint}", endpoint);
        var body = await http.GetStringAsync(endpoint, cancellationToken);
        logger.LogInformation("ListService.GetItems retrieved {body}", body); 
        var packedResult = JsonSerializer.Deserialize<TCollection>(body, JsonSerializerOptions);
        if(packedResult == null) {
            throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
        }
        var items = Unpacker(packedResult);
        var total = Counter(packedResult);
        return new ItemsProviderResult<TItem>(items, total);
    }

    private readonly HttpClient http;

    private readonly ILogger<ListService<TCollection, TItem>> logger;

}
