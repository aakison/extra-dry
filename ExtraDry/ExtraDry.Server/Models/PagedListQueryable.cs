using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class PagedListQueryable<T> : SortedListQueryable<T>
{
    public PagedListQueryable(IQueryable<T> queryable, PageQuery pageQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
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
        var query = (Query as PageQuery)!;
        var skip = query.Skip;
        var take = query.Take;
        var sort = query.Sort;

        var lastToken = Token ?? new ContinuationToken(Query.Filter, sort, skip, take);
        var nextToken = lastToken.Next(skip, take);
        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);

        return new PagedCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = FilteredQuery.Count(),
            ContinuationToken = (previousSkip + items.Count) >= FilteredQuery.Count() ? lastToken.ToString() : nextToken.ToString(),
        };
    }
}
