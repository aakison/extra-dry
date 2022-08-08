#nullable enable

namespace ExtraDry.Blazor;

public class ThemeInfo {

    [Obsolete("Use IconTemplate for SVG filename replacement")]
    public string IconPrefix { get; set; } = string.Empty;

    public string IconTemplate { get; set; } = "/img/{0}.svg";

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

    public Dictionary<string, IconInfo> Icons { get; set; } = new();

}
