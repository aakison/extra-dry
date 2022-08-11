#nullable enable

namespace ExtraDry.Blazor;

/// <summary>
/// Represents a consistent display for items in lists which may present as simple title,
/// or which might also have a thumbnail and a subtitle.
/// Used for consistency when rendering in dropdown lists, etc.
/// </summary>
public partial class MiniCard : ComponentBase {

    /// <summary>
    /// The CSS Class that is merged with semantic classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The URL for the thumbnail, null if no thumbnail available.
    /// </summary>
    [Parameter]
    public string? Thumbnail { get; set; }

    /// <summary>
    /// The text for the title, required.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Title";

    /// <summary>
    /// The subtitle for the card, null if no subtitle should be presented.
    /// </summary>
    [Parameter]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Additional attributes are chained to the root `div` on the control.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }

    private string SemanticThumbnail => Thumbnail == null ? string.Empty : "thumbnail";

    private string SemanticSubtitle => Subtitle == null ? string.Empty : "subtitle";

    private bool ShowThumbnail => Thumbnail != null;

    private bool ShowSubtitle => Subtitle != null;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "mini-card", CssClass, SemanticThumbnail, SemanticSubtitle);

}
