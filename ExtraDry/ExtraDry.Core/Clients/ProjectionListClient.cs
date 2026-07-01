namespace ExtraDry.Core;

public class ProjectionListClient<TItem, TWrapper>(
    IListClient<TItem> itemsClient,
    Func<TItem, TWrapper> projection)
    : IListClient<TWrapper>
{
    /// <inheritdoc />
    public int PageSize => itemsClient.PageSize;

    /// <inheritdoc />
    public bool IsLoading => itemsClient.IsLoading;

    /// <inheritdoc />
    public bool? IsEmpty => itemsClient.IsEmpty;

    /// <inheritdoc />
    public async ValueTask<ListClientResult<TWrapper>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"PROJECTIONLISTCLIENT: Fetching items with query: {query}");
        var items = await itemsClient.GetItemsAsync(query, cancellationToken);
        var wrappers = items.Items.Select(e => projection(e)).ToList();
        Console.WriteLine($"PROJECTIONLISTCLIENT: Fetched {wrappers.Count} items, total: {items.Total}");
        return new ListClientResult<TWrapper>(wrappers, items.Count, items.Total);
    }
}
