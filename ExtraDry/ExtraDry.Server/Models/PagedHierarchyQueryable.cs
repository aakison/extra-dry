using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedHierarchyQueryable<T> : FilteredHierarchyQueryable<T> where T : IHierarchyEntity<T> 
    {
    public PagedHierarchyQueryable(IQueryable<T> queryable, PageHierarchyQuery pageHierarchyQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = pageHierarchyQuery;
        Token = ContinuationToken.FromString(pageHierarchyQuery.Token);
        FilteredQuery = ApplyKeywordFilter(queryable, pageHierarchyQuery, defaultFilter);
        // Ensure expanded slugs and ancestors are included, while excluding collapsed.
        var tempQuery = ExpandCollapseHierarchy(queryable, FilteredQuery, pageHierarchyQuery);
        // Then sort it the only way that is allowed, breadth-first by lineage.
        SortedQuery = tempQuery.OrderBy(e => e.Lineage);
        // Finally, apply paging
        PagedQuery = SortedQuery.Page(pageHierarchyQuery);
    }


    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public PagedHierarchyCollection<T> ToPagedCollection() =>
        CreatePagedCollection(PagedQuery.ToList());

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedHierarchyCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default) =>
        CreatePagedCollection(await ToListAsync(PagedQuery, cancellationToken));

    private PagedHierarchyCollection<T> CreatePagedCollection(List<T> items)
    {
        var skip = (Query as PageQuery)?.Skip ?? 0;
        var take = (Query as PageQuery)?.Take ?? PageQuery.DefaultTake;
        var sort = (Query as SortQuery)?.Sort;

        var nextToken = (Token ?? new ContinuationToken(Query.Filter, sort, take, take)).Next(skip, take);
        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);
        var total = items.Count == previousTake ? FilteredQuery.Count() : previousSkip + items.Count;
        return new PagedHierarchyCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = total,
            Expand = (Query as PageHierarchyQuery)?.Expand,
        };
    }

}
