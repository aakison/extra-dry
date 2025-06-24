namespace ExtraDry.Core;

/// <summary>
/// Configuration options for the <see cref="BlobClient{TBlob}" />. 
/// </summary>
public class BlobServiceOptions : IValidatableObject, IHttpClientOptions
{
    /// <inheritdoc />
    public string HttpClientName { get; set; } = string.Empty;

    /// <inheritdoc />
    public Type? HttpClientType { get; set; }

    /// <summary>
    /// The endpoint for the API that will receive the blob. This is appended to the URI of the
    /// HttpClient.
    /// </summary>
    /// <example>/api/blobs</example>
    [Required]
    public string BlobEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Set the maximum size of a blob that can be uploaded. This is a security measure to prevent
    /// denial of service attacks. The default is 10MB.
    /// </summary>
    public int MaxBlobSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Indicates if the client should compute a content hash and send it to the server. The server
    /// will use this hash to validate the content. This is a reliability measure to ensure content
    /// is not corrupted in transit. However, it does require additional memory and processing time
    /// on the client and on the server. The default is true.
    /// </summary>
    public bool ValidateHashOnCreate { get; set; } = true;

    /// <summary>
    /// Files on local filesystems allow many characters and patterns that can cause security
    /// problems on the server or when transferred to a computer running a different operating
    /// system. This will re-write the filename so that it is safe for use on the web, including in
    /// a URI. The default is true.
    /// </summary>
    public bool RewriteWebSafeFilename { get; set; } = true;

    /// <summary>
    /// Validates the Blob service, enforcing constraints on the properties.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(HttpClientType != null && HttpClientName != string.Empty) {
            yield return new ValidationResult("Only one of HttpClientType or HttpClientName can be specified.");
        }

        if(HttpClientType != null && !HttpClientType.IsSubclassOf(typeof(HttpClient))) {
            yield return new ValidationResult("HttpClientType must define a subtype of HttpClient.");
        }
    }
}
