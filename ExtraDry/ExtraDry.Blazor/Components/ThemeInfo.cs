namespace ExtraDry.Blazor;

public class ThemeInfo {

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

    /// <summary>
    /// Indicates that the Theme is in a loading state and not all icons are available.
    /// </summary>
    public bool Loading { get; set; }

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
