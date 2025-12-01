using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;

namespace ExtraDry.Core;

/// <summary>
/// A serializer for blobs to serialize and deserialize to and from HTTP requests and responses.
/// For the client-side, this is used to serialize the blob to a Request and deserialize the blob
/// from a Response. For the server-side, this is used to serialize the blob to a Response and
/// deserialize the blob from a Request. There is a similar class in ExtraDry.Blazor for the
/// client-side.
/// </summary>
public static class BlobSerializer
{
    /// <summary>
    /// Serialize a Blob to a <see cref="ByteArrayContent" /> object suitable for sending as the
    /// payload of an HTTP request.
    /// </summary>
    public static ByteArrayContent SerializeBlob<T>(T blob) where T : IBlob
    {
        var bytes = new ByteArrayContent(blob.Content ?? throw new ArgumentException("Blob must have content."));
        AddProperties(blob, bytes.Headers.Add);
        return bytes;
    }

    /// <summary>
    /// Deserialized a Blob from a <see cref="HttpResponseMessage" />.
    /// </summary>
    public static async Task<T> DeserializeBlobAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default) where T : IBlob, new()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var blob = Activator.CreateInstance<T>();
        foreach(var property in properties) {
            var headerAttribute = property.GetCustomAttribute<HttpHeaderAttribute>();
            var headerName = headerAttribute?.Name ?? $"X-Blob-{property.Name}";
            var headerValue = HeaderValue(headerName);
            if(headerValue != null) {
                if(property.PropertyType == typeof(string)) {
                    property.SetValue(blob, headerValue);
                }
                else if(property.PropertyType == typeof(Guid)) {
                    property.SetValue(blob, Guid.Parse(headerValue));
                }
                else if(property.PropertyType == typeof(int)) {
                    if(int.TryParse(headerValue, CultureInfo.InvariantCulture, out var intValue)) {
                        property.SetValue(blob, intValue);
                    }
                    else {
                        Console.WriteLine($"Unable to deserialize length: {headerValue}");
                    }
                }
                else if(headerAttribute != null) {
                    throw new NotImplementedException("HttpHeaderAttribute only supports string, int, and Guid types.");
                }
                else {
                    // silently ignore other types unless the developer tries to force them with
                    // HttpHeaderAttribute.
                }
            }
        }
        if(blob is IRevisioned revisioned) {
            var userHeader = HeaderValue("X-Blob-Revision-User");
            var timestampHeader = HeaderValue("X-Blob-Revision-Timestamp");
            if(userHeader != null && timestampHeader != null && DateTime.TryParse(timestampHeader, out var timestamp)) {
                revisioned.Revision = new UserTimestamp { User = userHeader, Timestamp = timestamp };
            }
        }
        blob.Content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        blob.Length = blob.Content.Length;
        return blob;

        string HeaderValue(string headerName)
        {
            response.Headers.TryGetValues(headerName, out var values);
            response.Content.Headers.TryGetValues(headerName, out var moreValues);
            return (values ?? moreValues)?.FirstOrDefault() ?? "";
        }
    }

    /// <summary>
    /// Serialize a Blob to a <see cref="HttpResponse" />.
    /// </summary>
    public static async Task SerializeBlobAsync<T>(HttpResponse response, T blob, CancellationToken cancellationToken = default) where T : IBlob
    {
        AddProperties(blob, (key, value) => response.Headers.Append(key, value));
        await response.Body.WriteAsync(blob.Content, cancellationToken);
    }

    /// <summary>
    /// Deserialize a Blob from a <see cref="HttpRequest" />.
    /// </summary>
    public static async Task<T> DeserializeBlobAsync<T>(HttpRequest request, CancellationToken cancellationToken = default) where T : IBlob, new()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var blob = Activator.CreateInstance<T>();
        foreach(var property in properties) {
            var headerAttribute = property.GetCustomAttribute<HttpHeaderAttribute>();
            var headerName = headerAttribute?.Name ?? $"X-Blob-{property.Name}";
            var headerValue = request.Headers[headerName].FirstOrDefault();
            if(headerValue != null) {
                if(property.PropertyType == typeof(string)) {
                    property.SetValue(blob, headerValue);
                }
                else if(property.PropertyType == typeof(Guid)) {
                    property.SetValue(blob, Guid.Parse(headerValue));
                }
                else if(property.PropertyType == typeof(int)) {
                    property.SetValue(blob, int.Parse(headerValue, CultureInfo.InvariantCulture));
                }
                else if(property.PropertyType.IsEnum) {
                    var enumValue = Enum.Parse(property.PropertyType, headerValue.ToString());
                    property.SetValue(blob, enumValue);
                }
                else if(headerAttribute != null) {
                    throw new NotImplementedException("HttpHeaderAttribute only supports string, int, and Guid types.");
                }
                else {
                    // silently ignore other types unless the developer tries to force them with
                    // HttpHeaderAttribute.
                }
            }
        }
        if(blob is IRevisioned revisioned) {
            var userHeader = request.Headers["X-Blob-Revision-User"].FirstOrDefault();
            var timestampHeader = request.Headers["X-Blob-Revision-Timestamp"].FirstOrDefault();
            if(userHeader != null && timestampHeader != null && DateTime.TryParse(timestampHeader, out var timestamp)) {
                revisioned.Revision = new UserTimestamp { User = userHeader, Timestamp = timestamp };
            }
        }
        var memoryStream = new MemoryStream();
        await request.Body.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();
        blob.Content = bytes;
        blob.Length = bytes.Length;
        return blob;
    }

    private static void AddProperties<T>(T blob, Action<string, string> keyValueAdder) where T : IBlob
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach(var property in properties) {
            var jsonIgnore = property.GetCustomAttribute<JsonIgnoreAttribute>();
            var ignore = jsonIgnore?.Condition ?? JsonIgnoreCondition.WhenWritingDefault;
            if(ignore == JsonIgnoreCondition.Always || property.Name == nameof(IBlob.Content)) {
                continue;
            }
            var headerName = property.GetCustomAttribute<HttpHeaderAttribute>()?.Name ?? $"X-Blob-{property.Name}";
            var headerValue = property.GetValue(blob)?.ToString() ?? "";
            if(headerName == "X-Blob-Revision") {
                // skip revision property, handled below
                continue;
            }
            if(!string.IsNullOrEmpty(headerValue) || ignore == JsonIgnoreCondition.Never) {
                keyValueAdder(headerName, headerValue);
            }
        }
        if(blob is IRevisioned revisioned) {
            keyValueAdder("X-Blob-Revision-User", revisioned.Revision.User);
            keyValueAdder("X-Blob-Revision-Timestamp", revisioned.Revision.Timestamp.ToString("o", CultureInfo.InvariantCulture));
        }
    }
}
