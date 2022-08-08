#nullable enable

namespace ExtraDry.Blazor;

public partial class MiniCard<TItem> : ComponentBase {

    /// <summary>
    /// The object to be used to render the card.
    /// </summary>
    [Parameter]
    public TItem? Value { get; set; }

    /// <summary>
    /// Indicates if the thumbnail should be rendered.  If not set, component 
    /// will check if the `Value` has a Thumbnail image.
    /// </summary>
    [Parameter]
    public bool ShowThumbnail {
        get => showThumbnail ?? Thumbnail != string.Empty;
        set => showThumbnail = value; 
    }
    private bool? showThumbnail;

    /// <summary>
    /// Indicates if the subtitle should be rendered.  If not set, component
    /// will check if the `Value` has a Subtitle. 
    /// </summary>
    [Parameter]
    public bool ShowSubtitle {
        get => showThumbnail ?? Subtitle != string.Empty;
        set => showSubtitle = value;
    }
    private bool? showSubtitle;

    private string SemanticThumbnail => ShowThumbnail ? "thumbnail" : string.Empty;

    private string SemanticSubtitle => ShowSubtitle ? "subtitle" : string.Empty;

    private string SemanticType => 
        typeof(TItem).Name.ToLowerInvariant();

    private string Thumbnail => 
        (Value as ISubjectViewModel)?.Thumbnail 
        ?? string.Empty;

    private string Title => 
        (Value as ISubjectViewModel)?.Title
        ?? Value?.ToString() 
        ?? "null";

    private string Subtitle => 
        (Value as ISubjectViewModel)?.Subtitle
        ?? string.Empty;

}
