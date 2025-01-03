namespace ExtraDry.Blazor.Components.Internal;

internal class ClientCache<TKey, TItem>(
    TimeSpan timeoutWindow)
    where TKey : notnull
{
    public bool TryGetValue(TKey key, out TItem? entry)
    {
        entry = default;
        if(entries.TryGetValue(key, out var cacheEntry)) {
            if(cacheEntry.Expiry > DateTime.UtcNow) {
                entry = cacheEntry.Entry;
                return true;
            }
            else {
                CleanCache();
            }
        }
        return false;
    }

    public bool TryAdd(TKey key, TItem entry)
    {
        var cacheEntry = new CacheEntry(DateTime.UtcNow.Add(timeoutWindow), entry);
        return entries.TryAdd(key, cacheEntry);
    }

    public class CacheEntry(DateTime expiry, TItem entry)
    {
        public DateTime Expiry { get; set; } = expiry;

        public TItem Entry { get; set; } = entry;
    }

    private void CleanCache()
    {
        var now = DateTime.UtcNow;
        var toDelete = entries.Where(e => e.Value.Expiry < now).Select(e => e.Key).ToList();
        foreach(var key in toDelete) {
            entries.Remove(key);
        }
    }

    private readonly Dictionary<TKey, CacheEntry> entries = [];
}
