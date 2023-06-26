namespace ExtraDry.Blazor;

/// <summary>
/// Provides a cascading theme mechanism to configure portions of ExtraDry.
/// Note: this does not cover styles like colors which should be done in CSS.
/// </summary>
public partial class Theme : ComponentBase {

    /// <summary>
    /// The content that this theme applies to.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// The collection of icons that are used for this theme.
    /// See IconInfo for details on creating the icons, then register them here.
    /// </summary>
    [Parameter]
    public IEnumerable<IconInfo>? Icons { get; set; }

    /// <summary>
    /// A ThemeInfo that might have been cascaded from a parent theme.
    /// </summary>
    [CascadingParameter]
    public ThemeInfo ThemeInfo { get; set; } = new();

    /// <summary>
    /// A custom error component that is applied and used on any DryErrorBoundary instead of the default.
    /// </summary>
    [Parameter]
    public Type? ErrorComponent { get; set; }

    /// <inheritdoc cref="ValueLoader{ValueModel}.ErrorIndicator" />
    /// <see cref="ValueLoader{ValueModel}"/>
    [Parameter]
    public RenderFragment<IndicatorContext>? ErrorIndicator { get; set; }
    /// <inheritdoc cref="ValueLoader{ValueModel}.TimeoutIndicator" />
    /// <see cref="ValueLoader{ValueModel}"/>
    [Parameter]
    public RenderFragment<IndicatorContext>? TimeoutIndicator { get; set; }
    /// <inheritdoc cref="ValueLoader{ValueModel}.LoadingIndicator" />
    /// <see cref="ValueLoader{ValueModel}"/>
    [Parameter]
    public RenderFragment<IndicatorContext>? LoadingIndicator { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        ThemeInfo.Icons = Icons?.ToDictionary(e => e.Key, e => e, StringComparer.InvariantCultureIgnoreCase) ?? new();
        if(ErrorComponent != null) {
            ThemeInfo.ErrorComponent = ErrorComponent;
        }

        if(ErrorIndicator != null) {
            ThemeInfo.ErrorIndicator = ErrorIndicator;
        }

        if(TimeoutIndicator != null) {
            ThemeInfo.TimeoutIndicator = TimeoutIndicator;
        }

        if(LoadingIndicator != null) {
            ThemeInfo.LoadingIndicator = LoadingIndicator;
        }
    }
}

