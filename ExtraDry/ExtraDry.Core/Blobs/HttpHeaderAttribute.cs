namespace ExtraDry.Core;

/// <summary>
/// For the serialization of Blobs, the HTTP headers are used for properties instead of a JSON 
/// body.  When creating a Blob (which implements <see cref="IBlob"/>) the properties are read
/// and sent to the server as HTTP headers.  To override the default header names, use this
/// attribute.  If the attribute is not used, the header name will be X-Blob-{PropertyName}.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HttpHeaderAttribute : Attribute
{

    /// <inheritdoc cref="HttpHeaderAttribute" />
    public HttpHeaderAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name of the HTTP header to use for this property.
    /// </summary>
    public string Name { get; }
}
