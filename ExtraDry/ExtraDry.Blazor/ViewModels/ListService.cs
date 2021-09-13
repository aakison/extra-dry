#nullable enable

using ExtraDry.Blazor.Components.Internal;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExtraDry.Blazor {

    public class ListService<TCollection, TItem> : IListService<TItem> {

        public ListService(HttpClient client, string entitiesEndpointTemplate, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            http = client;
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

        public string SkipQueryParam { get; set; } = "skip";

        public string TakeQueryParam { get; set; } = "take";

        public string FilterQueryParam { get; set; } = "filter";

        private Func<TCollection, ICollection<TItem>> Unpacker { get; set; }

        private Func<TCollection, int> Counter { get; set; }

        public object[] UriArguments { get; set; } = Array.Empty<object>();

        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        public string ListEndpoint(string? sort, bool? ascending, int? skip, int? take)
        {
            try {
                var path = string.Format(CultureInfo.InvariantCulture, UriTemplate, UriArguments);
                var keys = new Dictionary<string, string>();
                if(!string.IsNullOrWhiteSpace(sort)) {
                    keys.Add("sort", sort);
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
            return await GetItemsAsync(null, null, 0, int.MaxValue, token);
        }

        public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string? sort, bool? ascending, int? skip, int? take, CancellationToken cancellationToken)
        {
            var endpoint = ListEndpoint(sort, ascending, skip, take);
            var body = await http.GetStringAsync(endpoint, cancellationToken);
            Console.WriteLine($"Got {body}");
            Console.WriteLine($"Deserialize into {typeof(TCollection).Name}");
            var packedResult = JsonSerializer.Deserialize<TCollection>(body, JsonSerializerOptions);
            if(packedResult == null) {
                throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
            }
            var items = Unpacker(packedResult);
            var total = Counter(packedResult);
            return new ItemsProviderResult<TItem>(items, total);
        }

        private readonly HttpClient http;

    }
}
