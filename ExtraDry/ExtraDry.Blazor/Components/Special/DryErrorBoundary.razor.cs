using ExtraDry.Blazor.Components.Internal;
using ExtraDry.Core.Models;

namespace ExtraDry.Blazor;

/// <summary>
/// Creates a error boundary very similar to ErrorBoundary, but with the ErrorContent managed
/// centrally so that it can stay consistent.  Provide the type for the RazorComponent either
/// here or in an enclosing Theme tag.  If not component is provided a default component
/// is displayed that can be styled.
/// </summary>
public partial class DryErrorBoundary : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The cascading theme information from a Theme tag.  Used to access global exception pages.
    /// </summary>
    [CascadingParameter]
    public ThemeInfo? ThemeInfo { get; set; }

    /// <summary>
    /// The normal content to display.  Exceptions in this child will be caught and exceptions
    /// shown through the ErrorComponent.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// When an exception occurs in ChildContent, this component will be instantiated and displayed
    /// instead.  The component will recieve the Exception and, if availabe, the ProblemDetails as 
    /// parameters.  If null, the component from the Theme is used.
    /// </summary>
    [Parameter]
    public Type? ErrorComponent { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    public void Recover()
    {
        ErrorBoundary.Recover();
    }

    // Injected from razor component page.
    private ExposedErrorBoundary ErrorBoundary { get; set; } = null!;

    private Type ErrorType => ErrorComponent ?? ThemeInfo?.ErrorComponent ?? typeof(DefaultErrorComponent);

    private Dictionary<string, object?> ErrorParameters {
        get {
            var parameters = new Dictionary<string, object?>() {
                { nameof(CssClass), CssClass },
                { nameof(Exception), ErrorBoundary.ExposedCurrentException },
                { nameof(ProblemDetails), (ErrorBoundary.ExposedCurrentException as DryException)?.ProblemDetails }
            };
            if(UnmatchedAttributes != null) {
                foreach(var entry in UnmatchedAttributes) {
                    parameters.Add(entry.Key, entry.Value);
                }
            }
            return parameters;
        }
    }

}
