using ExtraDry.Server.Internal;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <inheritdoc cref="IFilteredQueryable{T}" />
public class FilteredListQueryable<T> : FilteredQueryable<T> {

    protected FilteredListQueryable() { }

    public FilteredListQueryable(IQueryable<T> queryable, FilterQuery filterQuery, Expression<Func<T, bool>>? defaultFilter)
    {
        ForceStringComparison = (queryable as BaseQueryable<T>)?.ForceStringComparison;
        Query = filterQuery;
        FilteredQuery = ApplyKeywordFilter(queryable, filterQuery, defaultFilter);
        SortedQuery = FilteredQuery;
        PagedQuery = SortedQuery;
    }

    protected static IQueryable<T> ApplyKeywordFilter(IQueryable<T> queryable, FilterQuery query, Expression<Func<T, bool>>? defaultFilter)
    {
        return (string.IsNullOrWhiteSpace(query.Filter), defaultFilter) switch {
            (true, null) => queryable,
            (true, _) => queryable.Where(defaultFilter).AsQueryable(),
            (false, null) => queryable.Filter(query),
            (false, _) => ApplyDefaultFilterIfNotInFilterQuery(),
        };

        IQueryable<T> ApplyDefaultFilterIfNotInFilterQuery()
        {
            var filter = FilterParser.Parse(query.Filter ?? "");
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

    /// <summary>
    /// Given a collection of items retrieved from a data store, create a filtered collection.
    /// Base classes will use their knowledge of the queries to fill in the correct details.
    /// </summary>
    protected FilteredCollection<T> CreateFilteredCollection(List<T> items) =>
        new() {
            Filter = Query.Filter,
            Items = items,
        };
    
    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollection" />
    public FilteredCollection<T> ToFilteredCollection() => 
        CreateFilteredCollection(FilteredQuery.ToList());

    /// <inheritdoc cref="IFilteredQueryable{T}.ToFilteredCollectionAsync(CancellationToken)" />
    public async Task<FilteredCollection<T>> ToFilteredCollectionAsync(CancellationToken cancellationToken = default) =>
        CreateFilteredCollection(await ToListAsync(FilteredQuery, cancellationToken));

}
