using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
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
    public PagedCollection<T> ToPagedCollection()
    {
        var list = PagedQuery.ToList();
        var count = FilteredQuery.Count();
        return CreatePagedCollection(list, count);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default)
    {
        var list = await ToListAsync(PagedQuery, cancellationToken);
        var count = await FilteredQuery.CountAsync(cancellationToken);
        return CreatePagedCollection(list, count);
    }

    private PagedCollection<T> CreatePagedCollection(List<T> items, int count)
    {
        var query = (Query as PageQuery)!;
        var skip = query.Skip;
        var take = query.Take;
        var sort = query.Sort;

        var lastToken = Token ?? new ContinuationToken(Query.Filter, sort, skip, take);
        var previousTake = ContinuationToken.ActualTake(Token, take);
        var previousSkip = ContinuationToken.ActualSkip(Token, skip);

        /*
         *  When a full page of results is not returned then the 'skip' value in the 
         *  continuation token should be the number of records that have been returned.
         *  
         *  As an example; the query results in 13 items, with a skip value of 10 and a take value
         *  of 5, this would result in 3 items being returned.  Therefore the continuation tokens
         *  skip and take values should be 13 and 5.
         */
        var nextToken = items.Count == previousTake
            ? lastToken.Next(skip, take)
            : new ContinuationToken(Query.Filter, sort, count, previousTake);

        return new PagedCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = FilteredQuery.Count(),
            ContinuationToken = nextToken.ToString(),
        };
    }
}
