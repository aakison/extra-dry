namespace ExtraDry.Core;

public interface IListClient<T>
{
    int PageSize { get; }


    ValueTask<ListClientResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);
}
