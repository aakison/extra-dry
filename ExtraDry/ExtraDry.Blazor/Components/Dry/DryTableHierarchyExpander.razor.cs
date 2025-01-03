using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

public partial class DryTableHierarchyExpander<TItem> : ComponentBase, IExtraDryComponent
{
    /// <summary>
    /// The item that is to be expanded or collapsed in the hierarchy managed by the <see
    /// cref="QueryBuilder" />.
    /// </summary>
    [Parameter, EditorRequired]
    public ListItemInfo<TItem> Item { get; set; } = default!;

    /// <inheritdoc cref="IExtraDryComponent.CssClass " />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc cref="DryPageQueryView.PageQueryBuilder" />
    [CascadingParameter]
    public QueryBuilder? QueryBuilder { get; set; }

    private void DoFiltersExpandCollapse(MouseEventArgs _)
    {
        if(QueryBuilder == null || Item.Item == null) {
            return;
        }
        if(Item.Item is not IHierarchyEntity item) {
            return;
        }
        var slug = item.Slug;
        if(Item.IsGroup) {
            if(Item.IsExpanded) {
                QueryBuilder.Hierarchy.Collapse(slug);
                Item.IsExpanded = false;
            }
            else {
                QueryBuilder.Hierarchy.Expand(slug);
                Item.IsExpanded = true;
            }
            QueryBuilder.NotifyChanged();
        }
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-hierarchy-expander", LevelCss, CssClass);

    private string LevelCss => $"level-{Item.GroupDepth}";

    private string Icon =>
        (Item.IsGroup, Item.IsExpanded) switch {
            (false, _) => "blank",
            (_, true) => "collapse",
            (_, false) => "expand",
        };

    private string Caption { get; set; } = "Expand";
}
