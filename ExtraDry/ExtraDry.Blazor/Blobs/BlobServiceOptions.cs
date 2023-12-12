namespace ExtraDry.Blazor;

/// <summary>
/// Configuration options for the <see cref="BlobService{TBlob}"/>.  This class is not intended to 
/// be used directly, instead use the <see cref="ServiceCollectionExtensions.AddBlobService(Microsoft.Extensions.DependencyInjection.IServiceCollection, Action{BlobServiceOptions})" />
/// method's Config action to set these values.
/// </summary>
public class BlobServiceOptions : IBlobServiceOptions, IValidatableObject {

    /// <inheritdoc/>
    public string HttpClientName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Type? HttpClientType { get; set; }

    /// <summary>
    /// The endpoint for the API that will receive the blob.  This is appended to the URI of the 
    /// HttpClient.
    /// </summary>
    /// <example>/api/blobs</example>
    [Required]
    public string BlobEndpoint { get; set; } = string.Empty;

    /// <inheritdoc/>
    public int MaxBlobSize { get; set; } = 10 * 1024 * 1024;

    /// <inheritdoc/>
    public bool ValidateHashOnCreate { get; set; } = true;

    /// <inheritdoc/>
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
