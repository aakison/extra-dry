using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class FilteredHierarchyQueryable<T> : FilteredListQueryable<T> where T : IHierarchyEntity<T> {

    protected FilteredHierarchyQueryable() {
        Query = new();
    }

    public FilteredHierarchyQueryable(IQueryable<T> queryable, HierarchyQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = query;
        // Level is the depth to query to, applied in addition tot he filter..
        var levelQuery = ApplyLevelFilter(queryable, query.Level);
        // Then filter with common filter mechanism
        FilteredQuery = ApplyKeywordFilter(levelQuery, query, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        var hierarchyQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, query);
        // Then sort it the only way that is allowed, breadth-first by lineage.
        SortedQuery = ApplyLineageSort(hierarchyQuery);
        // Finally, ignore paging.
        PagedQuery = SortedQuery;
    }

    protected IQueryable<T> ApplyLineageSort(IQueryable<T> queryable)
    {
        return queryable.OrderBy(e => e.Lineage);
    }

    protected IQueryable<T> ApplyLevelFilter(IQueryable<T> queryable, int level)
    {
        return new BaseQueryable<T>(queryable.Where(e => e.Lineage.GetLevel() <= level), ForceStringComparison);
    }

    protected static IQueryable<T> ExpandCollapseHierarchy(IQueryable<T> baseQueryable, IQueryable<T> filteredQueryable, HierarchyQuery query)
    {
        var ancestors = AncestorOf(filteredQueryable);
        var expansions = ChildrenOf(query.Expand);
        var collapses = DescendantOf(query.Collapse);

        //var all = filtered.Union(ancestors);
        var all = filteredQueryable.Union(expansions).Union(ancestors).Except(collapses).OrderBy(e => e.Lineage);
        return all;

        IQueryable<T> ChildrenOf(IEnumerable<string> parentSlugs) =>
            baseQueryable.SelectMany(parent => baseQueryable
                .Where(child => child.Lineage.IsDescendantOf(parent.Lineage)
                            && child.Lineage.GetLevel() == parent.Lineage.GetLevel() + 1
                            && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<T> DescendantOf(IEnumerable<string> parentSlugs) =>
            baseQueryable.SelectMany(parent => baseQueryable
                .Where(child => child.Lineage.IsDescendantOf(parent.Lineage)
                    && child.Lineage.GetLevel() > parent.Lineage.GetLevel()
                    && parentSlugs.Contains(parent.Slug)),
                (parent, child) => child);

        IQueryable<T> AncestorOf(IQueryable<T> filteredSubset) =>
            baseQueryable.SelectMany(ancestor => filteredSubset
                .Where(descendant => descendant.Lineage.IsDescendantOf(ancestor.Lineage)
                    && descendant.Lineage.GetLevel() != ancestor.Lineage.GetLevel()),
                (ancestor, descendant) => ancestor);
    }

    public HierarchyCollection<T> ToHierarchyCollection() =>
        CreateHierarchyCollection(SortedQuery.ToList());

    public async Task<HierarchyCollection<T>> ToHierarchyCollectionAsync() =>
        CreateHierarchyCollection(await ToListAsync(SortedQuery));

    private HierarchyCollection<T> CreateHierarchyCollection(IList<T> items) => 
        new() {
            Filter = Query.Filter,
            Items = items,
            Sort = nameof(IHierarchyEntity<T>.Lineage).ToLowerInvariant(),
            Level = Query.Level,
            Expand = Query.Expand.Any() ? Query.Expand : null,
            Collapse = Query.Collapse.Any() ? Query.Collapse : null,
        };  

    private new HierarchyQuery Query { get; }

}
