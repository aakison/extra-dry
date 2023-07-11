namespace ExtraDry.Blazor;

public partial class DryFilter<TItem> : ComponentBase, IExtraDryComponent {

    public DryFilter()
    {
        ViewModelDescription = new ViewModelDescription(typeof(TItem), this);
        AllFilters = new List<string> { KeywordsFitlerIdentifier };
        AllFilters.AddRange(ViewModelDescription.FilterProperties
            .Where(e => e.HasDiscreteValues)
            .Select(e => e.Property.Name));
    }

    /// <inheritdoc cref="IExtraDryComponent.CssClass "/>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "filter by keyword...";

    [Parameter]
    public List<string> VisibleFilters { get; set; } = new();

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    public PageQueryBuilder? PageQueryBuilder { get; set; }

    protected void DoFiltersSubmit(DialogEventArgs _)
    {
        DisplayedFilters = SelectedFilters.ToList();
    }

    protected void DoFiltersCancel(DialogEventArgs _)
    {
        SelectedFilters = DisplayedFilters.ToList();
    }

    private void DoFiltersReset(MouseEventArgs _)
    {
        PageQueryBuilder?.Reset();
    }

    private List<string> AllFilters { get; }

    private List<string> SelectedFilters { get; set; } = new();

    private List<string> DisplayedFilters { get; set; } = new();

    private IEnumerable<PropertyDescription> DisplayedEnumFilters => ViewModelDescription.FilterProperties
        .Where(e => e.HasDiscreteValues && IsFilterSelected(e.Property.Name));

    private bool DisplayKeywordFilter => IsFilterSelected(KeywordsFitlerIdentifier);

    private bool IsFilterSelected(string name) =>
        !DisplayedFilters.Any() || DisplayedFilters.Contains(name);

    private ViewModelDescription ViewModelDescription { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-filter", CssClass);

    private const string KeywordsFitlerIdentifier = "Keywords";
}
