#nullable enable

namespace ExtraDry.Blazor;

public partial class MiniCard<TItem> : ComponentBase {

    [Parameter]
    public TItem? Value { get; set; }

    [Parameter]
    public bool ShowThumbnail { get; set; } = true;

    [Parameter]
    public bool ShowSubtitle { get; set; } = true;

    private string SemanticThumbnail => ShowThumbnail ? "thumbnail" : string.Empty;

    private string SemanticSubtitle => ShowSubtitle ? "subtitle" : string.Empty;

    private string SemanticType => typeof(TItem).Name.ToLowerInvariant();

    private string Thumbnail => (Value as IMiniCardItem)?.Thumbnail ?? string.Empty;

    private string Title => (Value as IMiniCardItem)?.Title ?? Value?.ToString() ?? "null";

    private string Subtitle => (Value as IMiniCardItem)?.Subtitle ?? string.Empty;

    private string CssClass => (Value as IMiniCardItem)?.CssClass ?? string.Empty;

}
