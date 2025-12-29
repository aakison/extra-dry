namespace ExtraDry.Core;

/// <summary>
/// Options for a CRUD service.
/// </summary>
public class CrudClientOptions<T> : IHttpClientOptions, IValidatableObject
{
    /// <inheritdoc />
    public string HttpClientName { get; set; } = string.Empty;

    /// <inheritdoc />
    public Type? HttpClientType { get; set; }

    /// <summary>
    /// The endpoint template for the service. This is required. If a HttpClientName or
    /// HttpClientType is specified then this endpoint is relative the base address of that client.
    /// The key for the CRUD operations will be appended to this endpoint using the KeyFormatter.
    /// To control the position, insert '{key}' into the endpoint string.
    /// </summary>
    [Required, StringLength(StringLength.Sentence)]
    public string CrudEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// The amount of time to cache the results of read operations.  This is useful for
    /// frequently accessed data that does not change often, such as username lookups.
    /// </summary>
    public TimeSpan ReadCache { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// A formatting function that translates the `key` object taken by the CRUD endpoints into a
    /// string which is appended to the `CrudEndpoint` to create the full endpoint URL. The default
    /// implementation is the object's built in string transformation.
    /// </summary>
    public Func<object, string> KeyFormatter { get; set; } = e => e.ToString() ?? "";

    /// <summary>
    /// Validates the inter-dependant options for the CrudClient.
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

    /// <summary>
    /// Provides an action that is executed when an item is created.  Useful for pre-serialization processing.
    /// </summary>
    public Action<T>? OnCreate { get; set; }

    /// <summary>
    /// Provides an async action that is executed when an item is created.  Useful for pre-serialization processing.
    /// </summary>
    public Func<T, Task>? OnCreateAsync { get; set; }

    /// <summary>
    /// Provides an action that is called when an item is read.  Useful for post-deserialization processing.
    /// </summary>
    public Action<T>? OnRead { get; set; }

    /// <summary>
    /// Provides an async action that is called when an item is read.  Useful for post-deserialization processing.
    /// </summary>
    public Func<T, Task>? OnReadAsync { get; set; }

    /// <summary>
    /// Provides an action that is called when an item is updated.  Useful for pre-serialization processing.
    /// </summary>
    public Action<T>? OnUpdate { get; set; }

    /// <summary>
    /// Provides an async action that is called when an item is updated.  Useful for pre-serialization processing.
    /// </summary>
    public Func<T, Task>? OnUpdateAsync { get; set; }

}
