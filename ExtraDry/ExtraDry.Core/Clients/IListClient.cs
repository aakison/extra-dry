namespace ExtraDry.Core;

public interface IListClient<T>
{
    int PageSize { get; }

    /// <summary>
    /// Indicates if the client is currently loading data.
    /// </summary>
    bool IsLoading { get; }

    /// <summary>
    /// Indicates if the client has no items.  Before the first call to GetItemsAsync, this will be null.
    /// This may also be null if the Query changes such that emptiness cannot be determined until GetItemsAsync has returned.
    /// </summary>
    bool? IsEmpty { get; }

    ValueTask<ListClientResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default);
}
