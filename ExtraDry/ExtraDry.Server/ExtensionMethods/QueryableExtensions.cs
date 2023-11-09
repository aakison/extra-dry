using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <summary>
/// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting 
/// and paging API result sets.
/// </summary>
public static class QueryableExtensions {
    
    public static FilteredListQueryable<T> QueryWith<T>(this IQueryable<T> source, FilterQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        return new FilteredListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a `FilterQuery`, dynamically constructs an expression query that applies the 
    /// indicated filtering and sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A sort query that contains filtering and sorting information.</param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden 
    /// by `filterQuery`.
    /// </param>
    public static SortedListQueryable<T> QueryWith<T>(this IQueryable<T> source, SortQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        return new SortedListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a `PageQuery`, dynamically constructs an expression query that applies the indicated 
    /// filtering, sorting, and paging.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A page query that contains filtering, sorting, and paging information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden 
    /// by `partialQuery`.
    /// </param>
    public static PagedListQueryable<T> QueryWith<T>(this IQueryable<T> source, PageQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class
    {
        return new PagedListQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a <see cref="HierarchyQuery"/>, dynamically constructs an expression query that 
    /// applies the indicated level and keyword filtering, expansions, and collapses.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A hierarchy query that contains filtering, expansion, and collapse information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by `partialQuery`.
    /// </param>
    public static FilteredHierarchyQueryable<T> QueryWith<T>(this IQueryable<T> source, HierarchyQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class, IHierarchyEntity<T>
    {
        return new FilteredHierarchyQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Given a <see cref="PageHierarchyQuery"/>, dynamically constructs an expression query that 
    /// applies the indicated level and keyword filtering, expansions, collapses, and paging.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">
    /// A page hierarchy query that contains filtering, expansion, collapse and paging information.
    /// </param>
    /// <param name="defaultFilter">
    /// An expression that provides default filtering support, which can be overridden by `partialQuery`.
    /// </param>
    public static PagedHierarchyQueryable<T> QueryWith<T>(this IQueryable<T> source, PageHierarchyQuery query, Expression<Func<T, bool>>? defaultFilter = null) where T : class, IHierarchyEntity<T>
    {
        return new PagedHierarchyQueryable<T>(source.AsNoTracking(), query, defaultFilter);
    }

    /// <summary>
    /// Forces the underlying query mechanism to build string comparisons with the given comparison 
    /// type.  This only works on in-memory databases and anything that executes on a remote 
    /// database will throw an exception.
    /// </summary>
    public static BaseQueryable<T> ForceStringComparison<T>(this IQueryable<T> source, StringComparison forceStringComparison)
    {
        return new BaseQueryable<T>(source, forceStringComparison);
    }

    /// <summary>
    /// Given a `FilterQuery`, dynamically constructs an expression query that applies the 
    /// indicated filtering but not the indicated sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A filter query that contains filtering and sorting information.</param>
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, FilterQuery query)
    {
        return source.Filter(query.Filter);
    }

    /// <summary>
    /// Given a `filter`, dynamically constructs an expression query that applies the 
    /// indicated filtering.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="filter">A filter query that contains filtering information.</param>
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, string? filter)
    {
        if(string.IsNullOrWhiteSpace(filter)) {
            return source;
        }
        var description = new ModelDescription(typeof(T));
        if(!description.FilterProperties.Any()) {
            return source;
        }
        var comparison = (source as BaseQueryable<T>)?.ForceStringComparison;
        return source.WhereFilterConditions(description.FilterProperties.ToArray(), filter.Trim(), comparison);
    }

    /// <summary>
    /// Given a `SortQuery`, dynamically constructs an expression query that applies the indicated 
    /// sorting but not the indicated filtering.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A filter query that contains sorting information.</param>
    public static IQueryable<T> Sort<T>(this IQueryable<T> source, SortQuery query)
    {
        var token = (query as PageQuery)?.Token; // Only need the token if it's a PageQuery, null if FilterQuery.
        return source.Sort(query.Sort, token);
    }

    /// <summary>
    /// Given a name of a property and an order, dynamically creates a sort query.
    /// Also merges in the continuation token to supply values for consisted paging.
    /// On conflict, the token will override the specified sort.
    /// </summary>
    /// <param name="source">The queryable source, typically from EF, this is from `DbSet.AsQueryable()`</param>
    /// <param name="sort">The name of the property to sort by (optional, case insensitive)</param>
    /// <param name="ascending">Indicates if the order is ascending or not (optional, default true)</param>
    /// <param name="continuationToken">If this is not a new request, the token passed back from the previous request to maintain stability (optional)</param>
    internal static IQueryable<T> Sort<T>(this IQueryable<T> source, string? sort, string? continuationToken)
    {
        var token = ContinuationToken.FromString(continuationToken);
        var actualSort = (string.IsNullOrWhiteSpace(token?.Sort) ? sort : null) ?? "";
        var sortProperty = actualSort.TrimStart('+', '-');
        var ascending = !actualSort.StartsWith("-");
        var query = source;
        var modelDescription = new ModelDescription(typeof(T));
        if(modelDescription.StabilizerProperty == default) {
            throw new DryException(MissingStabilizerErrorMessage, SortErrorUserMessage);
        }
        if(!string.IsNullOrWhiteSpace(actualSort)) {
            query = ascending ? 
                query.OrderBy(sortProperty).ThenBy(modelDescription.StabilizerProperty.ExternalName) : 
                query.OrderByDescending(sortProperty).ThenByDescending(modelDescription.StabilizerProperty.ExternalName);
        }
        else {
            query = query.OrderBy(modelDescription.StabilizerProperty.ExternalName);
        }
        return query;
    }

    /// <summary>
    /// Given a `PageQuery`, dynamically constructs an expression query that applies the indicated 
    /// paging but not the indicated filtering or sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A page query that contains paging information.</param>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, IPageQuery query)
    {
        // TODO: Implement stabilizer using model meta-data.
        return source.Page(query.Skip, query.Take, query.Token);
    }

    /// <summary>
    /// Given a starting position and page size, returns a subset of the a collection.  Use will 
    /// typically immediately follow a call to `Sort`.  Also merges in continuation token to 
    /// supply values for paging if skip and take are missing.  On conflict, the skip and take 
    /// values will override the token.
    /// </summary>
    /// <param name="source">The queryable source, typically from result of call to `Sort`</param>
    /// <param name="skip">The number of records to skip, if paging this is the page number times the take size.</param>
    /// <param name="take">the number of records to take, this is the page size of the fetch.  Use to balance call API latency versus bandwidth</param>
    /// <param name="continuationToken">If this is not a new request, the token passed back from the previous request to maintain stability (optional)</param>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int skip, int take, string? continuationToken)
    {
        var token = ContinuationToken.FromString(continuationToken);
        var actualSkip = ContinuationToken.ActualSkip(token, skip);
        var actualTake = ContinuationToken.ActualTake(token, take);
        return source.Skip(actualSkip).Take(actualTake);
    }

    public static BaseCollection<T> ToBaseCollection<T>(this IQueryable<T> source)
    {
        return new BaseCollection<T> {
            Items = source.ToList(),
        };
    }

    public static async Task<BaseCollection<T>> ToBaseCollectionAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
    {
        var baseQuery = new BaseQueryable<T>(source);
        return await baseQuery.ToBaseCollectionInternalAsync(cancellationToken); 
    }

    private const string MissingStabilizerErrorMessage = "Sort requires that a single EF key is uniquely defined to stabalize the sort, even if another sort property is present.  Use a single unique key following EF conventions";

    private const string SortErrorUserMessage = "Unable to Sort. 0x0F3F241C";

}
