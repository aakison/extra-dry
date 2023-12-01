namespace ExtraDry.Blazor;

public class BlobServiceOptions : IHttpClientOptions, IValidatableObject {

    public string HttpClientName { get; set; } = string.Empty;

    public Type? HttpClientType { get; set; }

    [Required]
    public string BlobEndpoint { get; set; } = string.Empty;

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
