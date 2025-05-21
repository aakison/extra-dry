using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public interface IListService<T>
{
    int PageSize { get; }


    ValueTask<ListServiceResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);
}
