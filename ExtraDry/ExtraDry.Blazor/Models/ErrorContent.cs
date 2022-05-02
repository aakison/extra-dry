namespace ExtraDry.Blazor.Components;

/// <summary>
/// Standard document format for ASP.NET MVC Core validation errors
/// </summary>
// TODO: Consider moving to ProblemDetails class or ValidationProblemDetails
public class ErrorContent {

    public string Type { get; set; }

    public string Title { get; set; }

    public int Status { get; set; }

    public Dictionary<string, List<string>> Errors { get; set; }

}
