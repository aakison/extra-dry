using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

public partial class DryFilter<TItem> : ComponentBase, IExtraDryComponent
{
    public DryFilter()
    {
        ViewModelDescription = new DecoratorInfo(typeof(TItem), this);
        AllFilters =
        [
            KeywordsFitlerIdentifier,
            .. ViewModelDescription.FilterProperties
                .Where(e => e.HasDiscreteValues)
                .Select(e => e.Property.Name),
            .. DisplayDateFilters.Select(e => e.Property.Name)
        ];
    }

    /// <inheritdoc cref="IExtraDryComponent.CssClass " />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="FlexiSelect{TItem}.ShowPreview" />
    [Parameter]
    public bool ShowPreview { get; set; } = true;

    /// <inheritdoc cref="FlexiSelect{TItem}.Icon" />
    [Parameter]
    public string Icon { get; set; } = string.Empty;

    /// <inheritdoc cref="FlexiSelect{TItem}.Affordance" />
    [Parameter]
    public string Affordance { get; set; } = "select";

    /// <inheritdoc cref="IComments.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "filter by keyword...";

    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    /// <summary>
    /// The optional content that is displayed alongside the filter. This is useful to augment the
    /// filter with additional controls that are rendered inside the filter dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The pattern for the placeholder that is applied when a select list is shown. The pattern
    /// supports the name of the select field through the use of the string format positional
    /// operator '{0}'.
    /// </summary>
    [Parameter]
    public string SelectPlaceholderPattern { get; set; } = "Select {0}...";

    [Parameter]
    public List<string> VisibleFilters { get; set; } = [];

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    protected override void OnParametersSet()
    {
        QueryBuilderAccessor ??= new QueryBuilderAccessor(Decorator);
    }

    protected void DoFiltersSubmit(DialogEventArgs _)
    {
        DisplayedFilters = [.. SelectedFilters];
    }

    protected void DoFiltersCancel(DialogEventArgs _)
    {
        SelectedFilters = [.. DisplayedFilters];
    }

    private void DoFiltersReset(MouseEventArgs _)
    {
        QueryBuilderAccessor?.QueryBuilder.Reset();
    }

    private List<string> AllFilters { get; }

    private List<string> SelectedFilters { get; set; } = [];

    private List<string> DisplayedFilters { get; set; } = [];

    private IEnumerable<PropertyDescription> DisplayedEnumFilters => ViewModelDescription.FilterProperties
        .Where(e => e.HasDiscreteValues && IsFilterSelected(e.Property.Name));

    private IEnumerable<PropertyDescription> DisplayDateFilters => ViewModelDescription.FilterProperties.Where(e => e.PropertyType == typeof(DateTime));

    private bool DisplayKeywordFilter => IsFilterSelected(KeywordsFitlerIdentifier);

    private bool IsFilterSelected(string name)
    {
        return DisplayedFilters.Count == 0 || DisplayedFilters.Contains(name);
    }

    private DecoratorInfo ViewModelDescription { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-filter", CssClass);

    private const string KeywordsFitlerIdentifier = "Keywords";
}
