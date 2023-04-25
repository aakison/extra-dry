using ExtraDry.Server.Internal;
using System.Collections;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <inheritdoc cref="IPartialQueryable{T}" />
public class PartialQueryable<T> : IPartialQueryable<T> {

    public PartialQueryable(IQueryable<T> queryable, StringComparison forceStringComparison) { 
        ForceStringComparison = forceStringComparison;
        query = new();
        pagedQuery = sortedQuery = filteredQuery = queryable;
    }

    public PartialQueryable(IQueryable<T> queryable, FilterQuery filterQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as PartialQueryable<T>)?.ForceStringComparison;
        query = filterQuery;
        filteredQuery = InitializeMergedFilter(queryable, filterQuery, defaultFilter);
        sortedQuery = filteredQuery;
        pagedQuery = sortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public PartialQueryable(IQueryable<T> queryable, SortQuery sortQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as PartialQueryable<T>)?.ForceStringComparison;
        query = sortQuery;
        filteredQuery = InitializeMergedFilter(queryable, sortQuery, defaultFilter);
        sortedQuery = filteredQuery.Sort(sortQuery);
        pagedQuery = sortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public PartialQueryable(IQueryable<T> queryable, PageQuery pageQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as PartialQueryable<T>)?.ForceStringComparison;
        query = pageQuery;
        token = ContinuationToken.FromString(pageQuery.Token);
        filteredQuery = InitializeMergedFilter(queryable, pageQuery, defaultFilter);
        sortedQuery = filteredQuery.Sort(pageQuery);
        pagedQuery = sortedQuery.Page(pageQuery);
    }

    private static IQueryable<T> InitializeMergedFilter(IQueryable<T> queryable, FilterQuery query, Expression<Func<T, bool>>? defaultFilter)
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

    // https://stackoverflow.com/questions/31515898/traverse-an-expression-tree-and-extract-parameters
    public class MemberAccessVisitor : ExpressionVisitor {

        public MemberAccessVisitor(Type forType)
        {
            declaringType = forType;
        }

        public IList<string> PropertyNames { get; } = new List<string>();

        protected override Expression VisitMember(MemberExpression node)
        {
            if(node.Member.DeclaringType == declaringType) {
                PropertyNames.Add(node.Member.Name);
            }
            return base.VisitMember(node);
        }

        private readonly Type declaringType;
    }

    #region IQueryable interface support

    public Type ElementType => pagedQuery.ElementType;

    public Expression Expression => pagedQuery.Expression;

    public IQueryProvider Provider => pagedQuery.Provider;

    public IEnumerator GetEnumerator() => pagedQuery.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => pagedQuery.GetEnumerator();

    #endregion

    /// <inheritdoc cref="IPartialQueryable{T}.ToFilteredCollection" />
    public FilteredCollection<T> ToFilteredCollection()
    {
        var items = sortedQuery.ToList();
        return CreateFilteredCollection(items);
    }

    /// <inheritdoc cref="IPartialQueryable{T}.ToFilteredCollectionAsync(CancellationToken)" />
    public async Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default)
    {
        // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
        var items = new List<T>();
        if(sortedQuery is IAsyncEnumerable<T> sortedAsyncQuery) {
            await foreach(var element in sortedAsyncQuery.WithCancellation(cancellationToken)) {
                if(element != null) {
                    items.Add(element);
                }
            }
        } 
        else { 
            items.AddRange(sortedQuery);
        }
        return CreateFilteredCollection(items);
    }

    private FilteredCollection<T> CreateFilteredCollection(List<T> items)
    {
        return new FilteredCollection<T> {
            Items = items,
            Filter = query.Filter,
            Sort = Sort,
        };
    }

    /// <inheritdoc cref="IPartialQueryable{T}.ToPagedCollection"/>
    public PagedCollection<T> ToPagedCollection()
    {
        var items = pagedQuery.ToList();
        return CreatePartialCollection(items);
    }

    /// <inheritdoc cref="IPartialQueryable{T}.ToPagedCollectionAsync(CancellationToken)" />
    public async Task<PagedCollection<T>> ToPagedCollectionAsync(CancellationToken cancellationToken = default)
    {
        // Logic like EF Core `.ToListAsync` but without taking a dependency on that package into Blazor.
        var items = new List<T>();
        if(pagedQuery is IAsyncEnumerable<T> pagedAsyncQuery) {
            await foreach(var element in pagedAsyncQuery.WithCancellation(cancellationToken)) {
                items.Add(element);
            }
        }
        else {
            items.AddRange(pagedQuery);
        }
        return CreatePartialCollection(items);
    }

    /// <inheritdoc cref="IPartialQueryable{T}.ToStatistics" />
    public Statistics<T> ToStatistics()
    {
        var stats = new Statistics<T> {
            Distributions = new List<DataDistribution>(),
            Filter = query.Filter
        };
        var description = new ModelDescription(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = filteredQuery
                .GroupBy(statProp)
                .Select(e => new CountInfo(e.Key, e.Count()));

            // Logic like EF Core `.ToListAsync` but without taking a dependency on that entire package.
            var items = new List<CountInfo>();
            items.AddRange(statsQuery);
            stats.Distributions.Add(new DataDistribution(statProp.Property.Name, items.ToDictionary(e => e.Key, e => e.Count)));
        }
        return stats;
    }

    /// <inheritdoc cref="IPartialQueryable{T}.ToStatisticsAsync(CancellationToken)" />
    public async Task<Statistics<T>> ToStatisticsAsync(CancellationToken cancellationToken = default) 
    {
        var stats = new Statistics<T> {
            Distributions = new List<DataDistribution>(),
            Filter = query.Filter
        };
        var description = new ModelDescription(typeof(T));

        foreach(var statProp in description.StatisticsProperties) {
            var statsQuery = filteredQuery
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

    internal StringComparison? ForceStringComparison { get; private set; }

    private PagedCollection<T> CreatePartialCollection(List<T> items)
    {
        var skip = (query as PageQuery)?.Skip ?? 0;
        var take = (query as PageQuery)?.Take ?? PageQuery.DefaultTake;
        var ascending = (query as SortQuery)?.Ascending ?? true;

        var nextToken = (token ?? new ContinuationToken(query.Filter, Sort, ascending, take, take)).Next(skip, take);
        var previousTake = ContinuationToken.ActualTake(token, take);
        var previousSkip = ContinuationToken.ActualSkip(token, skip);
        var total = items.Count == previousTake ? filteredQuery.Count() : previousSkip + items.Count;
        return new PagedCollection<T> {
            Items = items,
            Filter = nextToken.Filter,
            Sort = nextToken.Sort,
            Start = previousSkip,
            Total = total,
            ContinuationToken = nextToken.ToString(),
        };
    }

    private string Sort => (query as SortQuery)?.Sort ?? "";

    private readonly FilterQuery query;

    private readonly ContinuationToken? token;

    private readonly IQueryable<T> filteredQuery;

    private readonly IQueryable<T> sortedQuery;

    private readonly IQueryable<T> pagedQuery;
}
