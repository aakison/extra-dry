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
    //public Func<object, string> KeyFormatter { get; set; } = e => e.ToString() ?? "";

    internal List<EndpointFormatter> EndpointFormatters { get; set; } = [
        new EndpointFormatter {
                ParmeterName = "key",
                Formatter = e => e.ToString() ?? "",
                Mode = EndpointMode.Append,
                Operations = CrudOperation.Existing
            },
        ];


    /// <summary>
    /// Adds a endpoint function that takes a key and creates the endpoint from it for the given operations.
    /// If used, the CrudEndpoint is ignored.
    /// </summary>
    /// <param name="generator">Function that generates the entire endpoint from the key.</param>
    /// <param name="operations">Operations that this formatter is used on.</param>
    public void AddEndpointGenerator<TKey>(Func<TKey, string> generator, CrudOperation operations = CrudOperation.All)
    {
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = "",
            Formatter = TypeSafeWrapper(generator),
            Mode = EndpointMode.Generate,
            Operations = operations
        });
    }

    /// <summary>
    /// Adds a endpoint function that will replace a specific parameter (like in an interpolated string)
    /// with the result of the function.
    /// </summary>
    /// <param name="parameterName">The name of the parameter, e.g. "uuid" in "/commerce/carts/{uuid}</param>
    /// <param name="formatter">Function that returns the key-value from the key.</param>
    /// <param name="operations">Operations that this formatter is used on.</param>
    public void AddEndpointReplacer<TKey>(string parameterName, Func<TKey, string> formatter, CrudOperation operations = CrudOperation.All)
    {
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = parameterName,
            Formatter = TypeSafeWrapper(formatter),
            Mode = EndpointMode.Replace,
            Operations = operations
        });
    }

    /// <summary>
    /// Add and endpoint function that will append the key value to the end of the CrudEndpoint.
    /// As order is not guaranteed, only a single appender can be used, and it will remove any existing appender when added.
    /// </summary>
    /// <param name="appender">Function that returns the key-value from the key.</param>
    /// <param name="operations">Operations that this formatter is used on.</param>
    public void AddEndpointAppender<TKey>(Func<T, string> appender, CrudOperation operations = CrudOperation.Existing)
    {
        EndpointFormatters.RemoveAll(e => e.Mode == EndpointMode.Append);
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = "",
            Formatter = TypeSafeWrapper(appender),
            Mode = EndpointMode.Append,
            Operations = operations,
        });
    }

    private static Func<object, string> TypeSafeWrapper<TKey>(Func<TKey, string> func)
    {
        return key => {
            if(key is not TKey) {
                throw new ArgumentException($"Key must be of type {typeof(TKey).Name} for this endpoint.");
            }
            var result = func((TKey)key);
            Console.WriteLine($"Replacing value with {result}");
            return result;
        };
    }

    public void ClearEndpointFormatters() => EndpointFormatters.Clear();

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
