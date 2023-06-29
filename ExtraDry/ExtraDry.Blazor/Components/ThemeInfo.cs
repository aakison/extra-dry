namespace ExtraDry.Blazor;

public class ThemeInfo {

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

    /// <inheritdoc cref="Suspense{TModel}.ErrorIndicator" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseErrorIndicator { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.TimeoutIndicator" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseTimeoutIndicator { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.LoadingIndicator" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseLoadingIndicator { get; set; }
}
