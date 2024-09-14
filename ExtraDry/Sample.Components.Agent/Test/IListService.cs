using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public interface IListService<T> : IOptionProvider<T> {

    int PageSize { get; }

    int MaxLevel { get; }

    int MinLevel { get; }

    ValueTask<ItemsProviderResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);

    //ValueTask<ListItemsProviderResult<T>> GetListItemsAsync(Query query, CancellationToken cancellationToken = default);
}
