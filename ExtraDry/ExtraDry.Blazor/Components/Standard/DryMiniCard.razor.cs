#nullable enable

namespace ExtraDry.Blazor;

public partial class DryMiniCard<TItem> : ComponentBase {

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The view-model to be used to render the card.
    /// Have view-model implement ISubjectViewModel to get access to ...
    /// </summary>
    [Parameter, EditorRequired]
    public TItem Model { get; set; } = default!;

    [Parameter]
    public ISubjectViewModel<TItem>? ViewModel { get; set; }

    /// <summary>
    /// Additional attributes are chained to the root `div` on the control.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }

    /// <summary>
    /// Indicates if the thumbnail should be rendered.  If not set, component 
    /// will check if the `Value` has a Thumbnail image.
    /// </summary>
    [Parameter]
    public bool ShowThumbnail {
        get => showThumbnail ?? ViewModelThumbnail != string.Empty;
        set => showThumbnail = value; 
    }
    private bool? showThumbnail;

    /// <summary>
    /// Indicates if the subtitle should be rendered.  If not set, component
    /// will check if the `Value` has a Subtitle. 
    /// </summary>
    [Parameter]
    public bool ShowSubtitle {
        get => showThumbnail ?? ViewModelSubtitle != string.Empty;
        set => showSubtitle = value;
    }
    private bool? showSubtitle;

    private string SemanticThumbnail => ShowThumbnail ? "thumbnail" : string.Empty;

    private string SemanticSubtitle => ShowSubtitle ? "subtitle" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, typeof(TItem).Name.ToLowerInvariant());

    private string ViewModelTitle => ViewModelHelper.Title(Model, ViewModel, null) 
        ?? Model?.ToString() 
        ?? "null";

    private string ViewModelSubtitle => ViewModelHelper.Subtitle(Model, ViewModel, string.Empty)!;

    private string ViewModelThumbnail => ViewModelHelper.Thumbnail(Model, ViewModel, string.Empty)!;

    private string DisplayTitle => ViewModelTitle;

    private string? DisplaySubtitle => ShowSubtitle ? ViewModelSubtitle : null;

    private string? DisplayThumbnail => ShowThumbnail ? ViewModelThumbnail : null;

}
