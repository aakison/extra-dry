using System.Collections.Concurrent;

namespace ExtraDry.Core;

public class MemoryCache<T>(TimeSpan Duration)
{
    /// <summary>
    /// Caches an item if caching is enabled. Also triggers periodic cleanup of expired entries.
    /// </summary>
    public void Write(string key, T item)
    {
        if(Duration <= TimeSpan.Zero) {
            return;
        }
        var cacheEntry = new CacheEntry<T>(item, DateTime.UtcNow);
        readCache.AddOrUpdate(key, cacheEntry, (_, _) => cacheEntry);

        CleanupExpiredEntries();
    }

    /// <summary>
    /// Attempts to read an item from the cache if caching is enabled and the item is still valid.
    /// Returns null if the item is not cached, expired, or caching is disabled.
    /// </summary>
    public T? Read(string key)
    {
        if(Duration <= TimeSpan.Zero) {
            return default;
        }

        if(!readCache.TryGetValue(key, out var cacheEntry)) {
            return default;
        }

        if(DateTime.UtcNow - cacheEntry.LastAccessed >= Duration) {
            readCache.TryRemove(key, out _);
            return default;
        }
        else {
            return cacheEntry.Item;
        }
    }

    /// <summary>
    /// Invalidates (removes) a cache entry for the specified key if caching is enabled.
    /// </summary>
    public void Delete(string key)
    {
        if(Duration <= TimeSpan.Zero) {
            return;
        }
        readCache.TryRemove(key, out _);
    }

    private void CleanupExpiredEntries()
    {
        // Only cleanup periodically to avoid performance impact
        if(++cleanupCounter % 100 == 0) {
            var cutoffTime = DateTime.UtcNow.Subtract(Duration);
            var expiredKeys = readCache
                .Where(kvp => kvp.Value.LastAccessed < cutoffTime)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach(var expiredKey in expiredKeys) {
                readCache.TryRemove(expiredKey, out _);
            }
        }
    }

    private readonly ConcurrentDictionary<string, CacheEntry<T>> readCache = new();

    private int cleanupCounter;

    /// <summary>
    /// Represents a cached item with sliding expiration tracking.
    /// </summary>
    private sealed class CacheEntry<TItem>(TItem item, DateTime lastAccessed)
    {
        public TItem Item { get; } = item;

        public DateTime LastAccessed { get; set; } = lastAccessed;
    }
}
