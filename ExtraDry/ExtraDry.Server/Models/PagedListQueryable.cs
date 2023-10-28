using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedListQueryable<T> : SortedListQueryable<T>
{
    public PagedListQueryable(IQueryable<T> queryable, PageQuery pageQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = pageQuery;
        Token = ContinuationToken.FromString(pageQuery.Token);
        FilteredQuery = ApplyKeywordFilter(queryable, pageQuery, defaultFilter);
        SortedQuery = ApplyPropertySort(FilteredQuery, pageQuery);
        PagedQuery = SortedQuery.Page(pageQuery);
    }


    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public PagedCollection<T> ToPagedCollection() =>
        CreatePagedCollection(PagedQuery.ToList());

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default) =>
        CreatePagedCollection(await ToListAsync(PagedQuery, cancellationToken));

    private PagedCollection<T> CreatePagedCollection(List<T> items)
    {
        var skip = (Query as PageQuery)?.Skip ?? 0;
        var take = (Query as PageQuery)?.Take ?? PageQuery.DefaultTake;
        var sort = (Query as SortQuery)?.Sort;

        var nextToken = (Token ?? new ContinuationToken(Query.Filter, sort, take, take)).Next(skip, take);
        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);
        var total = items.Count == previousTake ? FilteredQuery.Count() : previousSkip + items.Count;
        return new PagedCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = total,
            ContinuationToken = nextToken.ToString(),
        };
    }

}
