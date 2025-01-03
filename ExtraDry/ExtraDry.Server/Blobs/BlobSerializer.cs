using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;

namespace ExtraDry.Server;

/// <summary>
/// A serializer for blobs to serialize and deserialize to and from HTTP requests and responses.
/// For the server-side, this is used to serialize the blob to a Response and deserialize the blob
/// from a Request. There is a similar class in ExtraDry.Blazor for the client-side.
/// </summary>
public static class BlobSerializer
{
    /// <summary>
    /// Serialize a Blob to a <see cref="HttpResponse" />.
    /// </summary>
    public static async Task SerializeBlobAsync<T>(this HttpResponse response, T blob) where T : IBlob
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
            if(!string.IsNullOrEmpty(headerValue) || ignore == JsonIgnoreCondition.Never) {
                response.Headers.Append(headerName, headerValue);
            }
        }
        await response.Body.WriteAsync(blob.Content);
    }

    /// <summary>
    /// Deserialize a Blob from a <see cref="HttpRequest" />.
    /// </summary>
    public static async Task<T> DeserializeBlobAsync<T>(HttpRequest request) where T : IBlob, new()
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
                else if(headerAttribute != null) {
                    throw new NotImplementedException("HttpHeaderAttribute only supports string, int, and Guid types.");
                }
                else {
                    // silently ignore other types unless the developer tries to force them with
                    // HttpHeaderAttribute.
                }
            }
        }
        var memoryStream = new MemoryStream();
        await request.Body.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray(); // TODO: can be more efficient if no MD5.
        blob.Content = bytes;
        blob.Length = bytes.Length;
        return blob;
    }
}
