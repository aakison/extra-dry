using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <inheritdoc cref="IFilteredQueryable{T}" />
public class FilteredListQueryable<T> : FilteredQueryable<T> {

    public FilteredListQueryable(IQueryable<T> queryable, StringComparison forceStringComparison)
    {
        ForceStringComparison = forceStringComparison;
        Query = new();
        PagedQuery = SortedQuery = FilteredQuery = queryable;
    }

    public FilteredListQueryable(IQueryable<T> queryable, FilterQuery filterQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = filterQuery;
        FilteredQuery = ApplyKeywordFilter(queryable, filterQuery, defaultFilter);
        SortedQuery = FilteredQuery;
        PagedQuery = SortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public FilteredListQueryable(IQueryable<T> queryable, SortQuery sortQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = sortQuery;
        FilteredQuery = ApplyKeywordFilter(queryable, sortQuery, defaultFilter);
        SortedQuery = FilteredQuery.Sort(sortQuery);
        PagedQuery = SortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public FilteredListQueryable(IQueryable<T> queryable, PageQuery pageQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = pageQuery;
        Token = ContinuationToken.FromString(pageQuery.Token);
        FilteredQuery = ApplyKeywordFilter(queryable, pageQuery, defaultFilter);
        SortedQuery = FilteredQuery.Sort(pageQuery);
        PagedQuery = SortedQuery.Page(pageQuery);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollection"/>
    public override PagedCollection<T> ToPagedCollection()
    {
        var items = PagedQuery.ToList();
        return CreatePartialCollection(items);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public override async Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default)
    {
        // Logic like EF Core `.ToListAsync` but without taking a dependency on that package into Blazor.
        var items = new List<T>();
        if(PagedQuery is IAsyncEnumerable<T> pagedAsyncQuery) {
            await foreach(var element in pagedAsyncQuery.WithCancellation(cancellationToken)) {
                items.Add(element);
            }
        }
        else {
            items.AddRange(PagedQuery);
        }
        return CreatePartialCollection(items);
    }

    private PagedCollection<T> CreatePartialCollection(List<T> items)
    {
        var skip = (Query as PageQuery)?.Skip ?? 0;
        var take = (Query as PageQuery)?.Take ?? PageQuery.DefaultTake;

        var nextToken = (Token ?? new ContinuationToken(Query.Filter, Sort, take, take)).Next(skip, take);
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

    protected override string? Sort {
        get {
            var sort = (Query as SortQuery)?.Sort;
            return string.IsNullOrWhiteSpace(sort) ? null : sort;
        }
    }

}
