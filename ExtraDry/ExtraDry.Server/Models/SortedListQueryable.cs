using System.Linq.Expressions;

namespace ExtraDry.Server;

public class SortedListQueryable<T> : FilteredListQueryable<T>
{
    protected SortedListQueryable()
    { }

    public SortedListQueryable(IQueryable<T> queryable, FilterQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        Query = query;
        FilteredQuery = ApplyKeywordFilter(queryable, query, defaultFilter);
        SortedQuery = ApplyPropertySort(FilteredQuery, query);
        PagedQuery = SortedQuery;
    }

    public SortedCollection<T> ToSortedCollection()
    {
        return CreateSortedCollection(SortedQuery.ToList());
    }

    public async Task<SortedCollection<T>> ToSortedCollectionAsync(CancellationToken cancellationToken = default)
    {
        return CreateSortedCollection(await ToListAsync(SortedQuery, cancellationToken));
    }

    private SortedCollection<T> CreateSortedCollection(IList<T> items)
    {
        return new() {
            Items = items,
            Filter = Query.Filter,
            Sort = (Query as FilterQuery)?.Sort,
        };
    }

    protected IQueryable<T> ApplyPropertySort(IQueryable<T> queryable, FilterQuery query)
    {
        return queryable.Sort(query);
    }
}
