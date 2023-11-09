using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedHierarchyQueryable<T> : FilteredHierarchyQueryable<T> where T : class, IHierarchyEntity<T> 
    {
    public PagedHierarchyQueryable(IQueryable<T> queryable, PageHierarchyQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
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
        var query = FilteredQuery
            .GroupBy(_ => 1, (_, records) =>
                new Stats(records.Count(), records.Max(r => r.Lineage.GetLevel() + 1)));
        var stats = query.Single();
        return CreatePagedCollection(PagedQuery.ToList(), stats.Total, stats.MaxLevels);
    }

    private record Stats(int Total, int MaxLevels);

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedHierarchyCollection<T>> ToPagedHierarchyCollectionAsync(CancellationToken cancellationToken = default)
    {
        var query = FilteredQuery
            .GroupBy(_ => 1, (_, records) =>
                new Stats(records.Count(), records.Max(r => r.Lineage.GetLevel() + 1)));
        var stats = await ToSingleAsync(query, cancellationToken);
        return CreatePagedCollection(await ToListAsync(PagedQuery, cancellationToken), stats.Total, stats.MaxLevels);
    }

    private PagedHierarchyCollection<T> CreatePagedCollection(List<T> items, int total, int maxLevels)
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
        };
    }

}
