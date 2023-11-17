using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

public class ListItemsProviderResult<T> 
{
    public ListItemsProviderResult(BaseCollection<T> collection)
    {
        Collection = collection;
        ItemInfos = collection.Items.Select(e => new ListItemInfo<T> { 
            Item = e,
            IsLoaded = true,
            IsGroup = IsGroup(e),
            GroupDepth = GroupDepth(e),
            IsExpanded = IsExpanded(e),
        }).ToList();
        Count = collection.Count;
        Total = Collection is PagedCollection<T> paged ? paged.Total :
                Collection is PagedHierarchyCollection<T> pagedHierarchy ? pagedHierarchy.Total :
                Collection.Count; 
        MaxLevels = collection is HierarchyCollection<T> hierarchy ? hierarchy.MaxLevels : 0;
    }

    public List<ListItemInfo<T>> ItemInfos { get; }

    public int Count { get; set; }

    public int Total { get; set; }

    public int MaxLevels { get; set; }

    private BaseCollection<T> Collection { get; set; }

    private bool IsGroup(T item) => 
        Collection is HierarchyCollection<T> hierarchy && 
        item is IHierarchyEntity entity && 
        (hierarchy.Expandable?.Contains(entity.Slug) ?? false);

    private int GroupDepth(T item) =>
        item is IHierarchyEntity entity ? entity.Lineage.GetLevel() : 0;

    private bool IsExpanded(T item)
    {
        return Collection.Items.Any(child => IsDescendant(item as IHierarchyEntity, child as IHierarchyEntity));

        bool IsDescendant(IHierarchyEntity? parent, IHierarchyEntity? child)
        {
            if(parent == null || child == null) {
                return false;
            }
            if(parent.Lineage.GetLevel() == child.Lineage.GetLevel()) {
                return false;
            }
            return child.Lineage.IsDescendantOf(parent.Lineage);
        }
    }

}
