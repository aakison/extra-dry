using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Trivial extension to the ErrorBoundary to allow access to the CurrentException. This allows
/// exception information to be passed to ErrorComponent visualizations.
/// </summary>
/// <remarks>
/// Probably suppressed since exposing server side errors is considered bad form, but the damage is
/// done when the exception comes across the wire, not when it's shown in the UI. It's only bad UI
/// design to show to an end user. The hacker already saw the error information through dev tools.
/// Typically the exposed exception will be a DryException with ProblemDetails which are designed
/// for end-users and system integrators.
/// </remarks>
[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Not a full-blown DRY component, just a bit of a hack to get the Exception out.")]
public class ExposedErrorBoundary : ErrorBoundary
{
    /// <summary>
    /// The current exception that is being handled by the ErrorBoundary.
    /// </summary>
    public Exception? ExposedCurrentException => CurrentException;
}
