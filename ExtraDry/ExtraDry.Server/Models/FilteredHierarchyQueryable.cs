using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class FilteredHierarchyQueryable<T> : FilteredListQueryable<T> where T : class, IHierarchyEntity<T>
{

    protected FilteredHierarchyQueryable()
    {
        UnfilteredQuery = new List<T>().AsQueryable();
        Query = new();
    }

    public FilteredHierarchyQueryable(IQueryable<T> queryable, HierarchyQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        UnfilteredQuery = queryable;
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = query;
        // Level is the depth to query to, applied in addition to the filter..
        FilteredQuery = ApplyLevelFilter(queryable, query.Level);
        // Then filter with common filter mechanism
        FilteredQuery = ApplyKeywordFilter(FilteredQuery, query, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        FilteredQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, query);
        // Then sort it the only way that is allowed, depth-first by lineage.
        SortedQuery = ApplyLineageSort(FilteredQuery);
        // Finally, ignore paging.
        PagedQuery = SortedQuery;
    }

    public HierarchyCollection<T> ToHierarchyCollection()
    {
        var childrenQuery = CreateChildrenQuery();

        var items = SortedQuery.ToList();
        var children = childrenQuery.ToList();
        return CreateHierarchyCollection(items, children);
    }

    public async Task<HierarchyCollection<T>> ToHierarchyCollectionAsync()
    {
        var childrenQuery = CreateChildrenQuery();

        var items = await ToListAsync(SortedQuery);
        var children = await ToListAsync(childrenQuery);
        return CreateHierarchyCollection(items, children);
    }

    protected IQueryable<T> ApplyLineageSort(IQueryable<T> queryable)
    {
        return queryable.OrderBy(e => e.Lineage);
    }

    protected IQueryable<T> ApplyLevelFilter(IQueryable<T> queryable, int level)
    {
        return level == 0
            ? new BaseQueryable<T>(queryable, ForceStringComparison)
            : new BaseQueryable<T>(queryable.Where(e => e.Lineage.GetLevel() < level), ForceStringComparison);
    }

    protected static IQueryable<T> ExpandCollapseHierarchy(IQueryable<T> baseQueryable, IQueryable<T> filteredQueryable, HierarchyQuery query)
    {
        var results = filteredQueryable;
        if(query.Expand.Count != 0) {
            results = results.Union(ChildrenOf(query.Expand));
        }
        if(!string.IsNullOrWhiteSpace(query.Filter)) {
            /// Add Tag for <see cref="ImproveHierarchyQueryPerformance"/> to work
            var ancestors = AncestorsOf(filteredQueryable).TagWith(ImproveHierarchyQueryPerformance.Tag);
            results = results.Union(ancestors);
        }
        if(query.Collapse.Count != 0) {
            results = results.Except(DescendantOf(query.Collapse));
        }
        return results;

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

        IQueryable<T> AncestorsOf(IQueryable<T> filtered) =>
            filtered.SelectMany(descendant => baseQueryable
                .Where(ancestor => descendant.Lineage.IsDescendantOf(ancestor.Lineage)),
                (_, ancestor) => ancestor).Distinct();

    }

    protected IQueryable<string> CreateChildrenQuery()
    {
        return PagedQuery
            .Join(UnfilteredQuery, parent => parent, child => child.Parent, (parent, child) =>
                new { ParentSlug = parent.Slug, ChildSlug = child.Slug })
            .GroupBy(e => e.ParentSlug)
            .Select(e => e.Key);
    }

    protected IQueryable<PagedHierarchyQueryable<T>.Stats> CreateStatQuery()
    {
        return FilteredQuery
            .GroupBy(_ => 1, (_, records) =>
                new Stats(records.Count(), records.Max(r => r.Lineage.GetLevel() + 1), records.Min(r => r.Lineage.GetLevel() + 1)));
    }

    protected IQueryable<T> UnfilteredQuery { get; set; }

    protected record Stats(int Total, int MaxLevels, int MinLevels);

    private new HierarchyQuery Query { get; }

    private HierarchyCollection<T> CreateHierarchyCollection(IList<T> items, List<string> expandable) =>
        new() {
            Filter = Query.Filter,
            Items = items,
            Sort = nameof(IHierarchyEntity<T>.Lineage).ToLowerInvariant(),
            Level = Query.Level,
            Expand = Query.Expand.Count != 0 ? Query.Expand : null,
            Collapse = Query.Collapse.Count != 0 ? Query.Collapse : null,
            Expandable = expandable.Count != 0 ? expandable : null,
        };

}
