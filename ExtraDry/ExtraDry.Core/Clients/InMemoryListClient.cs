namespace ExtraDry.Core;

/// <summary>
/// Creates an IListClient from an in-memory list of items.  This can be used sets of data that
/// are small enough to fit in memory and are disconnected from a remote API or database.
/// </summary>
public class InMemoryListClient<T>(
    IList<T> items,
    Func<T, bool>? filter = null)
    : IListClient<T>
{
    /// <inheritdoc />
    /// <remarks>
    /// Page size isn't applicable as entire set returned. But, must be positive to avoid division
    /// by zero when calculating page index in DryTable.
    /// </remarks>
    public int PageSize => Math.Max(1, items.Count);

    /// <summary>
    /// Indicates if the client is currently loading data.  As this is an in-memory client, it is never loading.
    /// </summary>
    public bool IsLoading => false;

    /// <inheritdoc />
    public bool? IsEmpty => items.Count == 0;

    /// <inheritdoc />
    public ValueTask<ListClientResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken)
    {
        var adjusted = FilterAndSort(query);
        return ValueTask.FromResult(new ListClientResult<T>(adjusted, adjusted.Count, adjusted.Count));
    }

    public bool TryRefreshItem(T updatedItem, Func<T, bool>? matchPredicate = null)
    {
        if(matchPredicate is null && updatedItem is IUniqueIdentifier uniqueItem) {
            matchPredicate = e => (e as IUniqueIdentifier)?.Uuid == uniqueItem.Uuid;
        }
        if(matchPredicate is null) {
            throw new InvalidOperationException("Either a match predicate must be provided, or TItem must implement IUniqueIdentifier.");
        }
        var existingItem = items.FirstOrDefault(matchPredicate);
        var index = items.IndexOf(existingItem);
        if(index == -1) {
            // Item not found, cannot refresh.
            return false;
        }
        items[index] = updatedItem;
        return true;
    }

    public IList<T> FilterAndSort(Query query)
    {
        var result = items
            .AsQueryable()
            .Where(e => filter == null || filter.Invoke(e))
            .Filter(query.Filter, StringComparison.OrdinalIgnoreCase)
            .Sort(query.Sort, SortStabilization.PrimaryKey)
            .ToList();
        return result;
    }

}
