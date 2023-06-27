namespace ExtraDry.Blazor;

public class ThemeInfo {

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

    /// <inheritdoc cref="Suspense{ValueModel}.ErrorIndicator" />
    /// <see cref="Suspense{ValueModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseErrorIndicator { get; set; }

    /// <inheritdoc cref="Suspense{ValueModel}.TimeoutIndicator" />
    /// <see cref="Suspense{ValueModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseTimeoutIndicator { get; set; }

    /// <inheritdoc cref="Suspense{ValueModel}.LoadingIndicator" />
    /// <see cref="Suspense{ValueModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseLoadingIndicator { get; set; }
}
