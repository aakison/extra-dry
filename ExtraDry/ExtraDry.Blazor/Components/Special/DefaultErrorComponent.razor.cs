using ExtraDry.Core.Models;

namespace ExtraDry.Blazor;

/// <summary>
/// Displays an error using a consistent display style.  Use with DryErrorBoundary to get 
/// consistent styling of exceptions across site.  To override, use the `Theme` component to 
/// register your own custom implementation.
/// </summary>
public partial class DefaultErrorComponent : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// If an exception was raised in the application, provides the details of the Exception.
    /// </summary>
    [Parameter]
    public Exception? Exception { get; set; }

    /// <summary>
    /// If ProblemDetails are available (per RFC7231) they are provided here.  This will be the 
    /// case with any RFC7231 APIs that return problem details on any 4xx response.
    /// </summary>
    [Parameter]
    public ProblemDetails? ProblemDetails { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "error", ErrorCss, CssClass);

    private string ErrorCss => ProblemDetails?.Status == null ? "errorXxx" : $"error{ProblemDetails.Status}";

}
