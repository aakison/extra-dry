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
        FilteredQuery = InitializeMergedFilter(queryable, filterQuery, defaultFilter);
        SortedQuery = FilteredQuery;
        PagedQuery = SortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public FilteredListQueryable(IQueryable<T> queryable, SortQuery sortQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = sortQuery;
        FilteredQuery = InitializeMergedFilter(queryable, sortQuery, defaultFilter);
        SortedQuery = FilteredQuery.Sort(sortQuery);
        PagedQuery = SortedQuery.Page(0, PageQuery.DefaultTake, null);
    }

    public FilteredListQueryable(IQueryable<T> queryable, PageQuery pageQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as FilteredListQueryable<T>)?.ForceStringComparison;
        Query = pageQuery;
        Token = ContinuationToken.FromString(pageQuery.Token);
        FilteredQuery = InitializeMergedFilter(queryable, pageQuery, defaultFilter);
        SortedQuery = FilteredQuery.Sort(pageQuery);
        PagedQuery = SortedQuery.Page(pageQuery);
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

    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollection" />
    public override FilteredCollection<T> ToFilteredCollection()
    {
        var items = SortedQuery.ToList();
        return CreateFilteredCollection(items);
    }

    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollectionAsync(CancellationToken)" />
    public override async Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default)
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

    private FilteredCollection<T> CreateFilteredCollection(List<T> items)
    {
        return new FilteredCollection<T> {
            Items = items,
            Filter = Query.Filter,
            Sort = Sort,
        };
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

}
