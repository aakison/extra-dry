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

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    private void DoFiltersExpandCollapse(MouseEventArgs _)
    {
        if(QueryBuilderAccessor == null || Item.Item == null) {
            return;
        }
        if(Item.Item is not IHierarchyEntity item) {
            return;
        }
        var slug = item.Slug;
        if(Item.IsGroup) {
            if(Item.IsExpanded) {
                QueryBuilderAccessor.QueryBuilder.Hierarchy.Collapse(slug);
                Item.IsExpanded = false;
            }
            else {
                QueryBuilderAccessor.QueryBuilder.Hierarchy.Expand(slug);
                Item.IsExpanded = true;
            }
            QueryBuilderAccessor.QueryBuilder.NotifyChanged();
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
