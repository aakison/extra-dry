using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public interface IListService<T> : IOptionProvider<T> {

    [Obsolete("Move to ListServiceOptions")]
    string UriTemplate { get; set; }

    [Obsolete("Pass in Custom HttpClient")]
    object[] UriArguments { get; set; }

    int PageSize { get; }

    int MaxLevel { get; }

    ValueTask<ItemsProviderResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);
}
