using ExtraDry.Server.Internal;
using System.Collections;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public abstract class FilteredQueryable<T> : IFilteredQueryable<T> {

    public abstract PagedCollection<T> ToPagedCollection();


    public abstract Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default);


    public abstract FilteredCollection<T> ToFilteredCollection();


    public abstract Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default);


    /// <inheritdoc cref="IFilteredQueryable{T}.ToStatistics" />
    public Statistics<T> ToStatistics()
    {
        var stats = new Statistics<T> {
            Distributions = new List<DataDistribution>(),
            Filter = Query.Filter
        };
        var description = new ModelDescription(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = FilteredQuery
                .GroupBy(statProp)
                .Select(e => new CountInfo(e.Key, e.Count()));

            // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
            var items = new List<CountInfo>();
            items.AddRange(statsQuery);
            stats.Distributions.Add(new DataDistribution(statProp.Property.Name, items.ToDictionary(e => e.Key, e => e.Count)));
        }
        return stats;
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToStatisticsAsync(CancellationToken)" />
    public async Task<Statistics<T>> ToStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var stats = new Statistics<T> {
            Distributions = new List<DataDistribution>(),
            Filter = Query.Filter
        };
        var description = new ModelDescription(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = FilteredQuery
                .GroupBy(statProp)
                .Select(e => new CountInfo(e.Key, e.Count()));

            // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
            var items = new List<CountInfo>();
            if(statsQuery is IAsyncEnumerable<CountInfo> statsQueryAsync) {
                await foreach(var element in statsQueryAsync.WithCancellation(cancellationToken)) {
                    if(element != null) {
                        items.Add(element);
                    }
                }
            }
            else {
                items.AddRange(statsQuery);
            }
            stats.Distributions.Add(new DataDistribution(statProp.Property.Name, items.ToDictionary(e => e.Key, e => e.Count)));
        }
        return stats;
    }

    #region IQueryable interface support

    public Type ElementType => PagedQuery.ElementType;

    public Expression Expression => PagedQuery.Expression;

    public IQueryProvider Provider => PagedQuery.Provider;

    public IEnumerator GetEnumerator() => PagedQuery.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => PagedQuery.GetEnumerator();

    #endregion

    protected string Sort => (Query as SortQuery)?.Sort ?? "";

    internal ContinuationToken? Token { get; set; }

    public StringComparison? ForceStringComparison { get; protected set; }

    // Derived classes populate in constructors so remove warnings.

    protected FilterQuery Query { get; set; } = null!;

    protected IQueryable<T> FilteredQuery { get; set; } = null!;

    protected IQueryable<T> SortedQuery { get; set; } = null!;

    protected IQueryable<T> PagedQuery { get; set; } = null!;

}
