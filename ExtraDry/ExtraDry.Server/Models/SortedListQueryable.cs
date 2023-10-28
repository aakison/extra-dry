using System.Linq.Expressions;

namespace ExtraDry.Server;

public class SortedListQueryable<T> : FilteredListQueryable<T>
{
    protected SortedListQueryable() { }

    public SortedListQueryable(IQueryable<T> queryable, SortQuery sortQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = sortQuery;
        FilteredQuery = ApplyKeywordFilter(queryable, sortQuery, defaultFilter);
        SortedQuery = ApplyPropertySort(FilteredQuery, sortQuery);
        PagedQuery = SortedQuery;
    }

    public SortedCollection<T> ToSortedCollection() =>
        CreateSortedCollection(SortedQuery.ToList());

    public async Task<SortedCollection<T>> ToSortedCollectionAsync(CancellationToken cancellationToken = default) =>
        CreateSortedCollection(await ToListAsync(SortedQuery, cancellationToken));

    private SortedCollection<T> CreateSortedCollection(IList<T> items) => 
        new() {
            Items = items,
            Filter = Query.Filter,
            Sort = Query.Sort,
        };

    protected IQueryable<T> ApplyPropertySort(IQueryable<T> queryable, SortQuery query) {
        return queryable.Sort(query);    
    }

    private new SortQuery Query { get; } = new();
}
