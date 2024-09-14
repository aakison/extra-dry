namespace ExtraDry.Blazor;

/// <summary>
/// A serializer for blobs to serialize and deserialize to and from HTTP requests and responses.
/// For the client-side, this is used to serialize the blob to a Request and deserialize the blob
/// from a Response.  There is a similar class in ExtraDry.Server for the server-side.
/// </summary>
public static class BlobSerializer
{

    /// <summary>
    /// Serialize a Blob to a <see cref="ByteArrayContent"/> object suitable for sending as the 
    /// payload of an HTTP request.
    /// </summary>
    public static ByteArrayContent SerializeBlob<T>(T blob) where T : IBlob
    {
        var bytes = new ByteArrayContent(blob.Content ?? throw new ArgumentException("Blob must have content."));
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
                bytes.Headers.Add(headerName, headerValue);
            }
        }
        return bytes;
    }

    /// <summary>
    /// Deserialized a Blob from a <see cref="HttpResponseMessage"/>.
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
                    // silently ignore other types unless the developer tries to force them with HttpHeaderAttribute.
                }
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

}
