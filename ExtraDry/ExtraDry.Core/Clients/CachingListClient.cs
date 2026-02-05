using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Core;

/// <summary>
/// A <see cref="IListClient{T}"/> that wrap another list client and adds caching.
/// </summary>
public class CachingListClient<TItem>(
    IListClient<TItem> itemsClient)
    : IListClient<TItem>
{

    /// <inheritdoc />
    public int PageSize => 100;

    /// <inheritdoc />
    public bool IsLoading => itemsClient.IsLoading;

    /// <inheritdoc />
    public bool? IsEmpty => itemsClient.IsEmpty;

    [SuppressMessage("Reliability", "CA2016:Forward the 'CancellationToken' parameter to methods", Justification = "Custom cancellation logic.")]
    public async ValueTask<ListClientResult<TItem>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
    {
        if(query.Filter != currentCacheFilter || query.Sort != currentCacheSort) {
            ClearCache();
            currentCacheFilter = query.Filter;
            currentCacheSort = query.Sort;
        }
        var skip = query.Skip ?? 0;
        var take = query.Take ?? PageSize;
        var cacheMisses = new List<int>();
        foreach(var i in Enumerable.Range(skip, take)) {
            if(!cache.ContainsKey(i)) {
                cacheMisses.Add(i);
            }
        }
        var missingPages = new List<int>();
        foreach(var miss in cacheMisses) {
            var pageNumber = miss / PageSize;
            if(!missingPages.Contains(pageNumber)) {
                missingPages.Add(pageNumber);
            }
        }
        if(missingPages.Count == 0) {
            // All cached, nothing to do.
            Console.WriteLine("All items cached, returning items.");
        }
        foreach(var page in missingPages) {
            var pageStartIndex = page * PageSize;
            var pageQuery = new Query {
                Filter = query.Filter,
                Sort = query.Sort,
                Skip = pageStartIndex,
                Take = PageSize
            };
            // CancellationToken not sent to client, cancellations usually request the same page so go ahead and fill the cache.
            var pageItems = await itemsClient.GetItemsAsync(pageQuery);
            total = pageItems.Total;
            var index = pageStartIndex;
            foreach(var item in pageItems.Items) {
                if(!cache.TryAdd(index, item)) {
                    cache[index] = item;
                }
                ++index;
            }
            // Then, finish the cancellation with a refreshed cache, next trip through will not have a duplicate request.
            cancellationToken.ThrowIfCancellationRequested();
        }
        // All cached, collect results.
        var items = new List<TItem>();
        for(int i = skip; i < skip + take; ++i) {
            if(cache.TryGetValue(i, out var item)) {
                items.Add(item);
            }
        }
        return new ListClientResult<TItem>(items, items.Count, total);
    }

    public void ClearCache()
    {
        cache.Clear();
        Console.WriteLine("Cache invalidated, clearing.");
    }

    public bool TryRefreshItem(TItem updatedItem, Func<TItem, bool>? matchPredicate = null)
    {
        if(matchPredicate is null && updatedItem is IUniqueIdentifier uniqueItem) {
            matchPredicate = e => (e as IUniqueIdentifier)?.Uuid == uniqueItem.Uuid;
        }
        if(matchPredicate is null) {
            throw new InvalidOperationException("Either a match predicate must be provided, or TItem must implement IUniqueIdentifier.");
        }
        var keyToReplace = cache
            .Where(kvp => matchPredicate(kvp.Value))
            .FirstOrDefault();
        if(keyToReplace.Value is not null) {
            cache[keyToReplace.Key] = updatedItem;
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// Invalidates the cache for a single item, such as one that has been updated and needs to be re-fetched.
    /// </summary>
    public bool TryRemoveItem(TItem item)
    {
        var keyToRemove = cache
            .Where(kvp => EqualityComparer<TItem>.Default.Equals(kvp.Value, item))
            .FirstOrDefault();
        if(keyToRemove.Value is not null) {
            cache.Remove(keyToRemove.Key);
            return true;
        }
        else {
            return false;
        }
    }

    /// <summary>
    /// Invalidates the cache for all items after the indicated item, such as when an item has been removed and everything gets re-indexed.
    /// </summary>
    public bool TryRemoveFromItem(TItem item)
    {
        var keyToRemove = cache
            .Where(kvp => EqualityComparer<TItem>.Default.Equals(kvp.Value, item))
            .FirstOrDefault();
        if(keyToRemove.Value is not null) {
            var firstKey = keyToRemove.Key;
            foreach(var key in cache.Keys.Where(k => k >= firstKey).ToList()) {
                cache.Remove(key);
            }
            return true;
        }
        else {
            return false;
        }
    }

    private readonly List<int> missingPages = [];

    private string? currentCacheFilter = "not-set";
    private string? currentCacheSort = "not-set";
    private int total = -1;
    private readonly Dictionary<int, TItem> cache = [];
}

