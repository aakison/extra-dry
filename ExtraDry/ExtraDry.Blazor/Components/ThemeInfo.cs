namespace ExtraDry.Blazor;

public class ThemeInfo {

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

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
}
