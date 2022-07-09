#nullable enable

namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Trivial extension to the ErrorBoundary to allow access to the CurrentException.
/// This allows exception information to be passed to ErrorComponent visualizations.
/// </summary>
/// <remarks>
/// Probably suppressed since exposing server side errors is considered bad form,
/// but the damage is done when the exception comes across the wire, not when it's
/// shown in the UI.  It's only bad UI design to show to an end user.  The hacker
/// already saw the error information through dev tools.
/// Typically the exposed exception will be a DryException with ProblemDetails
/// which are designed form end-users and system integrators.
/// </remarks>
public class ExposedErrorBoundary : ErrorBoundary {
    public Exception? ExposedCurrentException => CurrentException;
}
