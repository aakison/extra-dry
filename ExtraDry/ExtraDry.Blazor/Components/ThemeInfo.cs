namespace ExtraDry.Blazor;

public class ThemeInfo {

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

    /// <inheritdoc cref="Suspense{TModel}.Error" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseError { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Timeout" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseTimeout { get; set; }

    /// <inheritdoc cref="Suspense{TModel}.Fallback" />
    /// <see cref="Suspense{TModel}"/>
    public RenderFragment<IndicatorContext>? SuspenseFallback { get; set; }
}
