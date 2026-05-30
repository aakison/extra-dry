using System.Text.Json;

namespace ExtraDry.Core;

public class ListClientOptions : IHttpClientOptions, IValidatableObject
{
    public string ListEndpoint { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 100;

    public ListClientMode ListMode { get; set; } = ListClientMode.Paged;

    public string HttpClientName { get; set; } = string.Empty;

    public Type? HttpClientType { get; set; }

    [Required]
    public string FilterParameterName { get; set; } = "filter";

    [Required]
    public string SortParameterName { get; set; } = "sort";

    [Required]
    public string SkipParameterName { get; set; } = "skip";

    [Required]
    public string TakeParameterName { get; set; } = "take";

    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    /// <summary>
    /// Internal endpoint formatters.  None to begin with (unlike CrudClientOptions).
    /// </summary>
    internal List<EndpointFormatter> EndpointFormatters { get; set; } = [];

    /// <summary>
    /// Adds a endpoint function that takes a key and creates the endpoint from it for the given operations.
    /// If used, the ListEndpoint is ignored.  Only works with keyed versions of `GetItemsAsync`.
    /// </summary>
    /// <param name="generator">Function that generates the entire endpoint from the key.</param>
    public void AddEndpointGenerator<T>(Func<T, string> generator)
    {
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = "",
            Formatter = TypeSafeWrapper(generator),
            Mode = EndpointMode.Generate,
        });
    }

    /// <summary>
    /// Adds a endpoint function that will replace a specific parameter (like in an interpolated string)
    /// with the result of the function.  Only works with keyed versions of `GetItemsAsync`.
    /// </summary>
    /// <param name="parameterName">The name of the parameter, e.g. "uuid" in "/commerce/carts/{uuid}/items</param>
    /// <param name="formatter">Function that returns the key-value from the key.</param>
    public void AddEndpointReplacer<T>(string parameterName, Func<T, string> formatter)
    {
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = parameterName,
            Formatter = TypeSafeWrapper(formatter),
            Mode = EndpointMode.Replace,
        });
    }

    /// <summary>
    /// Add and endpoint function that will append the key value to the end of the ListEndpoint.
    /// As order is not guaranteed, only a single appender can be used, and it will remove any existing
    /// appender when added.  Only works with keyed versions of `GetItemsAsync`.
    /// </summary>
    /// <param name="appender">Function that returns the key-value from the key.</param>
    public void AddEndpointAppender<T>(Func<T, string> appender)
    {
        EndpointFormatters.RemoveAll(e => e.Mode == EndpointMode.Append);
        EndpointFormatters.Add(new EndpointFormatter {
            ParmeterName = "",
            Formatter = TypeSafeWrapper(appender),
            Mode = EndpointMode.Append,
        });
    }

    private Func<object, string> TypeSafeWrapper<T>(Func<T, string> func)
    {
        return key => {
            if(key is not T) {
                throw new ArgumentException($"Key must be of type {typeof(T).Name} for this List endpoint. {ListEndpoint}");
            }
            var result = func((T)key);
            Console.WriteLine($"Replacing value with {result}");
            return result;
        };
    }

    public void ClearEndpointFormatters() => EndpointFormatters.Clear();

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
