namespace ExtraDry.Blazor;

public partial class DryMiniCard<TItem> : ComponentBase, IExtraDryComponent {

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

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <summary>
    /// Indicates if the thumbnail should be rendered.  
    /// If null, component will check if the `Value` has a Thumbnail image.
    /// </summary>
    [Parameter]
    public bool? ShowThumbnail { get; set; }

    /// <summary>
    /// Indicates if the subtitle should be rendered.  If not set, component
    /// will check if the `Value` has a Subtitle. 
    /// </summary>
    [Parameter]
    public bool? ShowSubtitle { get; set; }

    [Inject]
    private IServiceProvider Services { get; set; } = null!;

    private ISubjectViewModel<TItem>? ResolvedViewModel
        => ViewModel
        ?? Services.GetService(typeof(ISubjectViewModel<TItem>)) as ISubjectViewModel<TItem>;

    private string SemanticThumbnail => DisplayThumbnail == null ? string.Empty : "thumbnail";

    private string SemanticSubtitle => DisplaySubtitle == null ? string.Empty : "subtitle";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, TypeName);

    private string TypeName {
        get {
            if(Model is ValueDescription description) {
                return $"enum {description.Key.ToString()?.ToLowerInvariant()}";
            }
            else {
                return typeof(TItem).Name.ToLowerInvariant();
            }
        }
    }

    private string ViewModelTitle => ViewModelHelper.Title(Model, ResolvedViewModel, null) 
        ?? Model?.ToString() 
        ?? "null";

    private string? ViewModelSubtitle => ViewModelHelper.Subtitle(Model, ResolvedViewModel, null);

    private string? ViewModelThumbnail => ViewModelHelper.Thumbnail(Model, ResolvedViewModel, null);

    private string DisplayTitle => ViewModelTitle;

    private string? DisplaySubtitle => ShowSubtitle switch {
        true => ViewModelSubtitle ?? string.Empty,
        false => null,
        _ => ViewModelSubtitle,
    };

    private string? DisplayThumbnail => ShowThumbnail switch {
        true => ViewModelThumbnail ?? string.Empty,
        false => null,
        _ => ViewModelThumbnail,
    };

}
