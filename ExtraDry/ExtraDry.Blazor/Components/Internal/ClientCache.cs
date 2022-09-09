namespace ExtraDry.Blazor.Components.Internal;

internal class ClientCache<TKey, TItem> where TKey : notnull {

    public ClientCache(TimeSpan timeoutWindow)
    {
        window = timeoutWindow;
    }

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
        var cacheEntry = new CacheEntry(DateTime.UtcNow.Add(window), entry);
        return entries.TryAdd(key, cacheEntry);
    }

    public class CacheEntry {
        public CacheEntry(DateTime expiry, TItem entry)
        {
            Expiry = expiry;
            Entry = entry;
        }
        public DateTime Expiry { get; set; }
        public TItem Entry { get; set; }
    }

    private void CleanCache()
    {
        var now = DateTime.UtcNow;
        var toDelete = entries.Where(e => e.Value.Expiry < now).Select(e => e.Key).ToList();
        foreach(var key in toDelete) {
            entries.Remove(key);
        }
    }

    private readonly TimeSpan window;

    private readonly Dictionary<TKey, CacheEntry> entries = new();

}
