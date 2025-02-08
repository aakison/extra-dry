using System.Text.Json;

namespace ExtraDry.Blazor;

public class ListServiceOptions : IHttpClientOptions, IValidatableObject
{
    public string ListEndpoint { get; set; } = string.Empty;

    public string HierarchyEndpoint { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 100;

    public ListServiceMode ListMode { get; set; } = ListServiceMode.FilterSortAndPage;

    public HierarchyServiceMode HierarchyMode { get; set; } = HierarchyServiceMode.FilterAndPage;

    public HttpMethod HierarchyMethod { get; set; } = HttpMethod.Post;

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

    [Required]
    public string LevelParameterName { get; set; } = "level";

    [Required]
    public string ExpandParameterName { get; set; } = "expand";

    [Required]
    public string CollapseParameterName { get; set; } = "collapse";

    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(HttpClientType != null && HttpClientName != string.Empty) {
            yield return new ValidationResult("Only one of HttpClientType or HttpClientName can be specified.");
        }
        if(ListEndpoint == string.Empty && HierarchyEndpoint == string.Empty) {
            yield return new ValidationResult("One of ListEndpointPath or HierarchyEndpointPath must be specified.");
        }
        if(HierarchyMethod != HttpMethod.Post && HierarchyMethod != HttpMethod.Get) {
            yield return new ValidationResult("HierarchyMethod must be either Post or Get.");
        }
        if(HttpClientType != null && !HttpClientType.IsSubclassOf(typeof(HttpClient))) {
            yield return new ValidationResult("HttpClientType must define a subtype of HttpClient.");
        }
    }
}
