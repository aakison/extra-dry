using ExtraDry.Core.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ExtraDry.Core;

/// <summary>
/// A simple CRUD API service wrapper for Extra Dry service endpoints. This wrapper assumes that 4
/// endpoints exist following standard RESTful principles for endpoints. The entity of Type `T must
/// be JSON serializable and accepted by the server. On non-success (2xx) results, the service
/// endpoints should return a ProblemDetails (RFC7807) response body. This body will be unwrapped
/// and throw in the body of a DryException. If ProblemDetails are not present, then a trivial
/// attempt to unpack the arbitrary response payload will be made.
/// </summary>
/// <remarks>
/// Create a CRUD service with the specified configuration. This service should not be manually
/// added to the IServiceCollection. Instead, use the AddCrudClient`T extension method.
/// </remarks>
public class CrudClient<T> where T : notnull
{
    public CrudClient(
        HttpClient client,
        CrudClientOptions<T> options,
        ILogger<CrudClient<T>> logger)
    {
        this.client = client;
        this.options = options;
        this.logger = logger;
        Cache = new MemoryCache<T>(options.ReadCache);
    }

    /// <summary>
    /// Create a new resource in the resource. The item will be serialized to JSON and sent to the
    /// POST endpoint for the resource collection.
    /// </summary>
    public async Task<ResourceReference> CreateAsync(T item, CancellationToken cancellationToken = default)
    {
        options.OnCreate?.Invoke(item);
        if(options.OnCreateAsync != null) {
            await options.OnCreateAsync(item);
        }
        var json = JsonSerializer.Serialize(item);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var endpoint = ApiEndpoint(string.Empty);
        //logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess(logger);
        var reference = await response.Content.ReadFromJsonAsync<ResourceReference>(cancellationToken: cancellationToken);
        return reference ?? new();
    }

    /// <summary>
    /// Read an existing resource from the resource collection. The key is used to identify the
    /// resource which is retrieved from the GET endpoint for the resource. Return null if not
    /// found.
    /// </summary>
    public async Task<T?> TryReadAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);

        // Try to read from cache first
        var cachedItem = Cache.Read(endpoint);
        if(cachedItem != null) {
            return cachedItem;
        }

        var response = await client.GetAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);
        var item = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        if(options.OnRead != null && item != null) {
            options.OnRead(item);
        }
        if(options.OnReadAsync != null && item != null) {
            await options.OnReadAsync(item);
        }

        // Cache the item if it was successfully read
        if(item != null) {
            Cache.Write(endpoint, item);
        }

        return item;
    }

    /// <summary>
    /// Read an existing resource from the resource collection. The key is used to identify the
    /// resource which is retrieved from the GET endpoint for the resource.
    /// </summary>
    public async Task<T> ReadAsync(object key, CancellationToken cancellationToken = default)
    {
        return await TryReadAsync(key, cancellationToken)
            ?? throw new ArgumentOutOfRangeException(nameof(key), $"Item not found for key {key}");
    }

    /// <summary>
    /// Update an existing resource in the resource collection. The key is used to identify the
    /// resource which is updated with the PUT endpoint for the resource.
    /// </summary>
    public async Task UpdateAsync(object key, T item, CancellationToken cancellationToken = default)
    {
        options.OnUpdate?.Invoke(item);
        if(options.OnUpdateAsync != null) {
            await options.OnUpdateAsync(item);
        }
        var endpoint = ApiEndpoint(key);
        //logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.PutAsJsonAsync(endpoint, item, cancellationToken);
        await response.AssertSuccess(logger);

        // Invalidate cache entry after update
        Cache.Delete(endpoint);
    }

    /// <summary>
    /// Delete an existing resource from the resource collection. The key is used to identify the
    /// resource which is deleted using the DELETE endpoint for the resource. This operation may or
    /// may not delete the resource permanently, see the endpoint documentation for details.
    /// </summary>
    public async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);
        //logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.DeleteAsync(endpoint, cancellationToken);
        await response.AssertSuccess(logger);

        // Invalidate cache entry after delete
        Cache.Delete(endpoint);
    }

    /// <summary>
    /// Sends an RPC request to the resource identified by the key. As this is not strictly
    /// RESTful, it should be used sparingly and only when the operation is not a standard CRUD
    /// operation.
    /// </summary>
    /// <remarks>
    /// The execution endpoint is the standard resource endpoint with string operation appended
    /// after a colon.
    /// </remarks>
    public async Task<HttpResponseMessage> ExecuteAsync(object key, string operation, object? payload = null, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoint(key);
        endpoint = $"{endpoint}:{operation}";
        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        //logger.LogEndpointCall(typeof(T), endpoint);
        var response = await client.PostAsync(endpoint, content, cancellationToken);
        await response.AssertSuccess(logger);
        return response;
    }

    private MemoryCache<T> Cache { get; set; } = null!;

    private string ApiEndpoint(object key)
    {
        var formattedKey = options.KeyFormatter(key);
        var url = $"{options.CrudEndpoint.TrimEnd('/')}/{formattedKey}".TrimEnd('/');
        return url;
    }

    private readonly HttpClient client;

    private readonly CrudClientOptions<T> options;

    private readonly ILogger<CrudClient<T>> logger;
}

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
