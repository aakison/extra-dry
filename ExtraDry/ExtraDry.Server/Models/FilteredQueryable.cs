using ExtraDry.Server.Internal;
using System.Collections;
using System.Linq.Expressions;

namespace ExtraDry.Server;

public abstract partial class FilteredQueryable<T> : IFilteredQueryable<T> {

    public abstract PagedCollection<T> ToPagedCollection();


    public abstract Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default);

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

    protected static IQueryable<T> ApplyKeywordFilter(IQueryable<T> queryable, FilterQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        if(string.IsNullOrWhiteSpace(query.Filter)) {
            if(defaultFilter == null) {
                return queryable;
            }
            else {
                return queryable.Where(defaultFilter).AsQueryable();
            }
        }
        else {
            if(defaultFilter == null) {
                return queryable.Filter(query);
            }
            else {
                var filter = FilterParser.Parse(query.Filter);
                var visitor = new MemberAccessVisitor(typeof(T));
                visitor.Visit(defaultFilter);
                var hasAnyPropertyInCommon = filter.Rules
                    .Any(r => visitor.PropertyNames
                        .Any(p => p.Equals(r.PropertyName, StringComparison.InvariantCultureIgnoreCase)));
                if(hasAnyPropertyInCommon) {
                    return queryable.Filter(query);
                }
                else {
                    return queryable.Where(defaultFilter).Filter(query);
                }
            }
        }
    }

    private FilteredCollection<T> CreateFilteredCollection(List<T> items)
    {
        return new FilteredCollection<T> {
            Items = items,
            Filter = Query.Filter,
            Sort = Sort,
        };
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollection" />
    public FilteredCollection<T> ToFilteredCollection()
    {
        var items = SortedQuery.ToList();
        return CreateFilteredCollection(items);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollectionAsync(CancellationToken)" />
    public async Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default)
    {
        // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
        var items = new List<T>();
        if(SortedQuery is IAsyncEnumerable<T> sortedAsyncQuery) {
            await foreach(var element in sortedAsyncQuery.WithCancellation(cancellationToken)) {
                if(element != null) {
                    items.Add(element);
                }
            }
        }
        else {
            items.AddRange(SortedQuery);
        }
        return CreateFilteredCollection(items);
    }


    #endregion

    protected virtual string? Sort { get; }

    internal ContinuationToken? Token { get; set; }

    public StringComparison? ForceStringComparison { get; protected set; }

    // Derived classes populate in constructors so remove warnings.

    protected FilterQuery Query { get; set; } = null!;

    protected IQueryable<T> FilteredQuery { get; set; } = null!;

    protected IQueryable<T> SortedQuery { get; set; } = null!;

    protected IQueryable<T> PagedQuery { get; set; } = null!;

}
