using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedHierarchyQueryable<T> : FilteredHierarchyQueryable<T> where T : class, IHierarchyEntity<T> 
    {
    public PagedHierarchyQueryable(IQueryable<T> queryable, PageHierarchyQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        UnfilteredQuery = queryable;
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = query;
        // Filter by level first, big performance gain.
        FilteredQuery = ApplyLevelFilter(queryable, query.Level);
        // Filter by keyword next, if provided.
        FilteredQuery = ApplyKeywordFilter(FilteredQuery, query, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        FilteredQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, query);
        // Then sort it the only way that is allowed, breadth-first by lineage.
        SortedQuery = ApplyLineageSort(FilteredQuery);
        // Finally, apply paging
        PagedQuery = SortedQuery.Page(query);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public PagedHierarchyCollection<T> ToPagedHierarchyCollection()
    {
        var statsQuery = CreateStatQuery();
        var childrenQuery = CreateChildrenQuery();

        var items = PagedQuery.ToList();
        var stats = statsQuery.Single();
        var children = childrenQuery.ToList();
        return CreatePagedCollection(items, stats.Total, stats.MaxLevels, children);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedHierarchyCollection<T>> ToPagedHierarchyCollectionAsync(CancellationToken cancellationToken = default)
    {
        var statsQuery = CreateStatQuery();
        var childrenQuery = CreateChildrenQuery();

        var items = await ToListAsync(PagedQuery, cancellationToken);
        var stats = await ToSingleAsync(statsQuery, cancellationToken);
        var children = await ToListAsync(childrenQuery, cancellationToken);
        return CreatePagedCollection(items, stats.Total, stats.MaxLevels, children);
    }

    private new PageHierarchyQuery Query { get; }

    private PagedHierarchyCollection<T> CreatePagedCollection(List<T> items, int total, int maxLevels, List<string> expandable)
    {
        var query = (Query as PageHierarchyQuery)!;
        var skip = query.Skip;
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);
        return new PagedHierarchyCollection<T> {
            Items = items,
            Filter = Query.Filter,
            Sort = nameof(IHierarchyEntity<T>.Lineage).ToLowerInvariant(),
            Start = previousSkip,
            Total = total,
            MaxLevels = maxLevels,
            Level = query.Level,
            Expand = query.Expand.Any() ? query.Expand : null,
            Collapse = query.Collapse.Any() ? query.Collapse : null,
            Expandable = expandable.Any() ? expandable : null,
        };
    }

}
