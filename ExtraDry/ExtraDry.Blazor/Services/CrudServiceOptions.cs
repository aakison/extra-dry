namespace ExtraDry.Blazor;

/// <summary>
/// Options for a CRUD service.
/// </summary>
public class CrudServiceOptions : IHttpClientOptions, IValidatableObject
{
    /// <inheritdoc />
    public string HttpClientName { get; set; } = string.Empty;

    /// <inheritdoc />
    public Type? HttpClientType { get; set; }

    /// <summary>
    /// The endpoint template for the service. This is required. If a HttpClientName or
    /// HttpClientType is specified then this endpoint is relative the base address of that client.
    /// </summary>
    [Required, StringLength(StringLength.Sentence)]
    public string CrudEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Validates the inter-dependant options for the CrudService.
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
