using Microsoft.AspNetCore.Components.Web.Virtualization;
using ExtraDry.Core;

namespace ExtraDry.Blazor;

public class InMemoryListService<T>(
    IList<T> items,
    Func<T, bool>? filter = null)
    : IListService<T>
{
    /// <inheritdoc />
    /// <remarks>
    /// Page size isn't applicable as entire set returned. But, must be positive to avoid division
    /// by zero when calculating page index in DryTable.
    /// </remarks>
    public int PageSize => Math.Max(1, items.Count);


    public ValueTask<ListServiceResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken)
    {
        var adjusted = FilterAndSort(query);
        return ValueTask.FromResult(new ListServiceResult<T>(adjusted, adjusted.Count, adjusted.Count));
    }

    public ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken _ = default)
    {
        return ValueTask.FromResult(new ItemsProviderResult<T>(items, items.Count));
    }

    public ValueTask<ListItemsProviderResult<T>> GetListItemsAsync(Query query, CancellationToken _ = default)
    {
        var adjusted = FilterAndSort(query);
        var collection = new BaseCollection<T> {
            Items = adjusted
        };
        return ValueTask.FromResult(new ListItemsProviderResult<T>(collection));
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
