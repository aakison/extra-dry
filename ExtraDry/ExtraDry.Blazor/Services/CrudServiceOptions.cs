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
    /// A formatting function that translates the `key` object taken by the CRUD endpoints into a
    /// string which is appended to the `CrudEndpoint` to create the full endpoint URL. The default
    /// implementation is the object's built in string transformation.
    /// </summary>
    public Func<object, string> KeyFormatter { get; set; } = e => e.ToString() ?? "";

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
