using System.Text.Json;

namespace ExtraDry.Blazor;

public class StatServiceOptions : IHttpClientOptions, IValidatableObject
{
    public string HttpClientName { get; set; } = string.Empty;

    public Type? HttpClientType { get; set; }

    [Required]
    public string StatEndpoint { get; set; } = string.Empty;

    // Make default json to ignore case, most non-.NET "RESTful" services use camelCase...
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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
