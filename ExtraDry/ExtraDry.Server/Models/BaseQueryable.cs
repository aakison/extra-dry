using ExtraDry.Server.Internal;
using System.Collections;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public class BaseQueryable<T> : IQueryable<T>
{
    protected BaseQueryable() { }

    public BaseQueryable(IQueryable<T> queryable)
    {
        PagedQuery = SortedQuery = FilteredQuery = queryable;
    }

    internal ContinuationToken? Token { get; set; }

    protected FilterQuery Query { get; set; } = null!;

    protected IQueryable<T> FilteredQuery { get; set; } = null!;

    protected IQueryable<T> SortedQuery { get; set; } = null!;

    protected IQueryable<T> PagedQuery { get; set; } = null!;

    public Type ElementType => PagedQuery.ElementType;

    public Expression Expression => PagedQuery.Expression;

    public IQueryProvider Provider => PagedQuery.Provider;

    public IEnumerator GetEnumerator() => PagedQuery.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => PagedQuery.GetEnumerator();

    protected async Task<List<TItem>> ToListAsync<TItem>(IQueryable<TItem> queryable, CancellationToken cancellationToken = default)
    {
        var items = new List<TItem>();
        if(queryable is IAsyncEnumerable<TItem> asyncQuery) {
            await foreach(var element in asyncQuery.WithCancellation(cancellationToken)) {
                if(element != null) {
                    items.Add(element);
                }
            }
        }
        else {
            items.AddRange(queryable);
        }
        return items;
    }

    internal BaseCollection<T> ToBaseCollectionInternal() =>
        BaseQueryable<T>.CreateBaseCollection(FilteredQuery.ToList());

    internal async Task<BaseCollection<T>> ToBaseCollectionInternalAsync(CancellationToken cancellationToken = default) =>
        BaseQueryable<T>.CreateBaseCollection(await ToListAsync(FilteredQuery, cancellationToken));

    private static BaseCollection<T> CreateBaseCollection(List<T> items) =>
        new() {
            Items = items,
        };

}
