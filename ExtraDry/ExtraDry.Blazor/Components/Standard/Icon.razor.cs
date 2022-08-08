#nullable enable

namespace ExtraDry.Blazor;

public partial class Icon : ComponentBase {

    [CascadingParameter]
    protected ThemeInfo? ThemeInfo { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public string Key { get; set; } = null!;

    [Parameter]
    public string? Alt { get; set; }

    private string Classes => $"{CssClass} {IconClass}".Trim();

    private string IconClass => ThemeInfo?.Icons.ContainsKey(Key) ?? false ? ThemeInfo.Icons[Key].CssClass : "";

    private string IconAlternateText => Alt ?? (ThemeInfo?.Icons.ContainsKey(Key) ?? false ? ThemeInfo.Icons[Key].AlternateText : "");

    private string ImagePath {
        get {
            if(ThemeInfo == null) {
                return $"/img/themeless/{Key}.svg";
            }
            else if(ThemeInfo.Icons.ContainsKey(Key)) {
                return ThemeInfo.Icons[Key].ImagePath;
            }
            else if(!string.IsNullOrWhiteSpace(ThemeInfo.IconTemplate)) {
                return string.Format(ThemeInfo.IconTemplate, Key);
            }
            else {
                return $"/img/no-icon-for-{Key}.svg";
            }
        }
    }

}

