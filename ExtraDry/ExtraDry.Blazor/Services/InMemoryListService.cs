using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public class InMemoryListService<T>(
    IList<T> items)
    : IListService<T>
{
    /// <inheritdoc />
    /// <remarks>
    /// Page size isn't applicable as entire set returned. But, must be positive to avoid division
    /// by zero when calculating page index in DryTable.
    /// </remarks>
    public int PageSize => Math.Max(1, items.Count);

    public int MaxLevel => 1;

    public int MinLevel => 1;

    public ValueTask<ItemsProviderResult<T>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var adjusted = FilterAndSort(query);
        return ValueTask.FromResult(new ItemsProviderResult<T>(adjusted, adjusted.Count));
    }

    public ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new ItemsProviderResult<T>(items, items.Count));
    }

    public ValueTask<ListItemsProviderResult<T>> GetListItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        var adjusted = FilterAndSort(query);
        var collection = new BaseCollection<T> {
            Items = adjusted
        };
        return ValueTask.FromResult(new ListItemsProviderResult<T>(collection));
    }

    public IList<T> FilterAndSort(Query query)
    {
        IEnumerable<T> result = items;
        if(!string.IsNullOrWhiteSpace(query.Sort)) {
            var descending = query.Sort.StartsWith('-');
            var sortProperty = query.Sort.TrimStart('-', '+');
            result = Sort(result, sortProperty);
            if(descending) {
                result = result.Reverse();
            }
        }
        // TODO: Apply filter.
        return result.ToList();
    }

    public IEnumerable<T> Sort(IEnumerable<T> collection, string propertyName)
    {
        // Get the PropertyInfo for the named property
        var propertyInfo = typeof(T).GetProperty(propertyName) 
            ?? throw new ArgumentException($"Property '{propertyName}' not found on type {typeof(T).Name}");

        // Create a comparer that uses the property
        var comparer = Comparer<T>.Create((lhs, rhs) => {
            var lhv = propertyInfo.GetValue(lhs);
            var rhv = propertyInfo.GetValue(rhs);
            return (lhv, rhv) switch {
                (null, null) => 0,
                (null, _) => -1,
                (_, null) => 1,
                (IComparable comparableLhv, _) => comparableLhv.CompareTo(rhv),
                _ => string.Compare(lhv.ToString(), rhv.ToString(), StringComparison.Ordinal)
            };
        });

        return collection.OrderBy(e => e!, comparer);
    }
}
