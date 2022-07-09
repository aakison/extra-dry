#nullable enable

namespace ExtraDry.Blazor;

public class ThemeInfo {

    public string IconPrefix { get; set; } = "fas fa-";

    public Type ErrorComponent { get; set; } = typeof(DefaultErrorComponent);

}
