namespace ExtraDry.Blazor;

public partial class DryHierarchyFilter<TItem> : ComponentBase, IExtraDryComponent
{
    public DryHierarchyFilter()
    {
        ViewModelDescription = new DecoratorInfo(typeof(TItem), this);
    }

    /// <inheritdoc cref="IExtraDryComponent.CssClass " />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The optional content that is displayed alongside the filter. This is useful to augment the
    /// filter with additional controls that are rendered inside the filter dialog.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    public QueryBuilder? QueryBuilder { get; set; }

    protected void DoFiltersExpand(MouseEventArgs _)
    {
        if(QueryBuilder?.Level?.Expand() ?? false) {
            QueryBuilder.NotifyChanged();
        }
    }

    protected void DoFiltersCollapse(MouseEventArgs _)
    {
        if(QueryBuilder?.Level?.Collapse() ?? false) {
            QueryBuilder.NotifyChanged();
        }
    }

    private DecoratorInfo ViewModelDescription { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-hierarchy-filter", CssClass);
}
