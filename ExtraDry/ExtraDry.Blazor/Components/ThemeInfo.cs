using ExtraDry.Core.Models;

namespace ExtraDry.Blazor;

/// <summary>
/// The cascading theme information for the application. This is used to provide a consistent set
/// of themes for the application, such as icons, error handling, and loading indicators. Inject it
/// into the application using the <see cref="Theme" /> component, typically in MainLayout. Then,
/// use CascadingParameter to inject it into components that need it.
/// </summary>
public class ThemeInfo
{
    /// <summary>
    /// The type of the error component that is rendered if an Error is thrown. Use the <see
    /// cref="DryErrorBoundary" /> component to catch errors and display them using this component
    /// reference.
    /// </summary>
    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    /// <summary>
    /// The type of the validation message component that is rendered if a Validation Error is
    /// thrown. Use the <see cref="ValidationBoundary" /> component to catch validation errors and
    /// display them using this component reference.
    /// </summary>
    public Type ValidationMessageComponent { get; set; } = typeof(DryValidationSummary);

    /// <summary>
    /// The set of Icons that are currently available in the theme. This information is used by
    /// <see cref="Icon" /> components and the many other components that compose Icons (e.g. <see
    /// cref="Button" />).
    /// </summary>
    public Dictionary<string, IconInfo> Icons { get; set; } = new(StringComparer.InvariantCultureIgnoreCase);

    /// <summary>
    /// Indicates that the Theme is in a loading state and not all icons are available.
    /// </summary>
    public bool Loading { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Error" />
    /// <see cref="Suspense{TModel}" />
    public RenderFragment<IndicatorContext>? SuspenseError { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Timeout" />
    /// <see cref="Suspense{TModel}" />
    public RenderFragment<IndicatorContext>? SuspenseTimeout { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Fallback" />
    /// <see cref="Suspense{TModel}" />
    public RenderFragment<IndicatorContext>? SuspenseFallback { get; set; }
}
