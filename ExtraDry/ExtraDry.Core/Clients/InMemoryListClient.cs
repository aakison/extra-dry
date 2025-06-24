namespace ExtraDry.Core;

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


    public ValueTask<ListClientResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken)
    {
        var adjusted = FilterAndSort(query);
        return ValueTask.FromResult(new ListClientResult<T>(adjusted, adjusted.Count, adjusted.Count));
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
