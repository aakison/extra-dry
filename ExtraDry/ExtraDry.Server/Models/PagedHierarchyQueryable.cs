using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedHierarchyQueryable<T> : FilteredHierarchyQueryable<T> where T : IHierarchyEntity<T> 
    {
    public PagedHierarchyQueryable(IQueryable<T> queryable, PageHierarchyQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = query;
        Token = ContinuationToken.FromString(query.Token);
        var levelQuery = ApplyLevelFilter(queryable, query.Level);
        FilteredQuery = ApplyKeywordFilter(levelQuery, query, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        var hierarchyQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, query);
        // Then sort it the only way that is allowed, breadth-first by lineage.
        SortedQuery = ApplyLineageSort(hierarchyQuery);
        // Finally, apply paging
        PagedQuery = SortedQuery.Page(query);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public PagedHierarchyCollection<T> ToPagedHierarchyCollection() =>
        CreatePagedCollection(PagedQuery.ToList());

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedHierarchyCollection<T>> ToPagedHierarchyCollectionAsync(CancellationToken cancellationToken = default) =>
        CreatePagedCollection(await ToListAsync(PagedQuery, cancellationToken));

    private PagedHierarchyCollection<T> CreatePagedCollection(List<T> items)
    {
        var query = (Query as PageHierarchyQuery)!;
        var skip = query.Skip;
        var take = query.Take;

        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);
        var total = items.Count == previousTake ? FilteredQuery.Count() : previousSkip + items.Count;
        return new PagedHierarchyCollection<T> {
            Items = items,
            Filter = Query.Filter,
            Sort = nameof(IHierarchyEntity<T>.Lineage).ToLowerInvariant(),
            Start = previousSkip,
            Total = total,
            Expand = query.Expand,
        };
    }

}
