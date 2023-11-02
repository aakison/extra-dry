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
    public PagedHierarchyCollection<T> ToPagedHierarchyCollection() =>
        CreatePagedCollection(PagedQuery.ToList(), FilteredQuery.Count());

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedHierarchyCollection<T>> ToPagedHierarchyCollectionAsync(CancellationToken cancellationToken = default) =>
        CreatePagedCollection(await ToListAsync(PagedQuery, cancellationToken), await ToCountAsync(FilteredQuery, cancellationToken));

    private PagedHierarchyCollection<T> CreatePagedCollection(List<T> items, int total)
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
            Expand = query.Expand.Any() ? query.Expand : null,
            Collapse = query.Collapse.Any() ? query.Collapse : null,
        };
    }

}
