namespace ExtraDry.Core;

public interface IListService<T>
{
    int PageSize { get; }


    ValueTask<ListServiceResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);
}
