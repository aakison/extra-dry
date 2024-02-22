namespace ExtraDry.Blazor;
public partial class DryStaticFilter : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass "/>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Parameter]
    public int? InitialHierarchyLevel { get; set; }

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    public QueryBuilder? QueryBuilder { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if(InitialHierarchyLevel.HasValue) {
            QueryBuilder?.Level.SetInitialLevel(InitialHierarchyLevel.Value);
        }
    }
}
