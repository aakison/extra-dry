#nullable enable

using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    public class RestfulListService<TCollection, TItem> : IListService<TItem> {

        /// <summary>
        /// 
        /// </summary>
        public RestfulListService(HttpClient client, string entitiesEndpointTemplate)
        {
            http = client;
            UriTemplate = entitiesEndpointTemplate;
            if(typeof(TCollection).IsAssignableTo(typeof(ICollection<TItem>))) {
                Unpacker = e => e as ICollection<TItem> ?? new Collection<TItem>();
                Counter = e => Unpacker(e)?.Count ?? 0;
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

        public RestfulListService(HttpClient client, string entitiesEndpointTemplate, 
            Func<TCollection, ICollection<TItem>> unpacker, Func<TCollection, int> counter)
        {
            http = client;
            UriTemplate = entitiesEndpointTemplate;
            Unpacker = unpacker;
            Counter = counter;
        }

        public string UriTemplate { get; set; }

        public int FetchSize { get; set; } = 100;

        public string SkipQueryParam { get; set; } = "skip";

        public string TakeQueryParam { get; set; } = "take";

        public string SearchQueryParam { get; set; } = "search";

        private Func<TCollection, ICollection<TItem>> Unpacker { get; set; }

        private Func<TCollection, int> Counter { get; set; }

        public object[] UriArguments { get; set; } = Array.Empty<object>();

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
                if(skip.HasValue) {
                    keys.Add("skip", skip.Value.ToString());
                }
                if(take.HasValue) {
                    keys.Add("take", take.Value.ToString());
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

        public async ValueTask<ItemsProviderResult<TItem>> GetItemsAsync(string? sort, bool? ascending, int? skip, int? take, CancellationToken token)
        {
            var result = await http.GetFromJsonAsync<TCollection>(ListEndpoint(sort, ascending, skip, take), token);
            if(result == null) {
                throw new DryException($"Call to endpoint returned nothing or couldn't be converted to a result.");
            }
            var items = Unpacker(result);
            var total = Counter(result);
            return new ItemsProviderResult<TItem>(items, total);
        }

        private readonly HttpClient http;

    }
}
