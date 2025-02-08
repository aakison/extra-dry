using ExtraDry.Core.Parser.Internal;

namespace ExtraDry.Core;

/// <summary>
/// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and
/// paging API result sets.
/// </summary>
public static class QueryableExtensions
{

    /// <summary>
    /// Given a `FilterQuery`, dynamically constructs an expression query that applies the
    /// indicated filtering but not the indicated sorting.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="query">A filter query that contains filtering and sorting information.</param>
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, FilterQuery query)
    {
        return source.Filter(query.Filter, query.Comparison);
    }

    /// <summary>
    /// Given a `filter`, dynamically constructs an expression query that applies the indicated
    /// filtering.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="source">The extension source</param>
    /// <param name="filter">A filter query that contains filtering information.</param>
    /// <param name="stringComparison">String comparison to override defaults when accessing in-memory data structures.</param>
    public static IQueryable<T> Filter<T>(this IQueryable<T> source, string? filter, StringComparison? stringComparison = null)
    {
        if(string.IsNullOrWhiteSpace(filter)) {
            return source;
        }
        var description = new ModelInfo(typeof(T));
        if(description.FilterProperties.Count == 0) {
            return source;
        }
        return source.WhereFilterConditions([.. description.FilterProperties], filter.Trim(), stringComparison);
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
        return source.Sort(query.Sort, token, query.Stabilization);
    }

    /// <summary>
    /// Given a name of a property and an order, dynamically creates a sort query. Also merges in
    /// the continuation token to supply values for consisted paging. On conflict, the token will
    /// override the specified sort.
    /// </summary>
    /// <param name="source">
    /// The queryable source, typically from EF, this is from `DbSet.AsQueryable()`
    /// </param>
    /// <param name="sort">The name of the property to sort by (optional, case insensitive)</param>
    /// <param name="continuationToken">
    /// If this is not a new request, the token passed back from the previous request to maintain
    /// stability (optional)
    /// </param>
    /// <param name="stabilization">Determines if a stabalizer should be added to the sort.  Some data providers do not support this.</param>
    internal static IQueryable<T> Sort<T>(this IQueryable<T> source, string? sort, string? continuationToken, SortStabilization stabilization = SortStabilization.PrimaryKey)
    {
        var token = ContinuationToken.FromString(continuationToken);
        var actualSort = (string.IsNullOrWhiteSpace(token?.Sort) ? sort : null) ?? "";
        var sortProperty = actualSort.TrimStart('+', '-');
        var ascending = !actualSort.StartsWith('-');
        var query = source;
        var modelDescription = new ModelInfo(typeof(T));
        if(modelDescription.StabilizerProperty == default) {
            throw new DryException(MissingStabilizerErrorMessage, SortErrorUserMessage);
        }
        if(!string.IsNullOrWhiteSpace(actualSort)) {
            // If set to always add key, we add a secondary sort to stabilize the sort
            query = (stabilization, ascending) switch {
                (SortStabilization.PrimaryKey, true) =>
                    query.OrderBy(sortProperty).ThenBy(modelDescription.StabilizerProperty.ExternalName),
                (SortStabilization.PrimaryKey, false) =>
                    query.OrderByDescending(sortProperty).ThenByDescending(modelDescription.StabilizerProperty.ExternalName),
                (_, true) =>
                    query.OrderBy(sortProperty),
                (_, false) =>
                    query.OrderByDescending(sortProperty),
            };
        }
        else {
            // For anything other than ProviderDefaultsOnly, add a default sort for stabilization.
            if(stabilization == SortStabilization.PrimaryKey) {
                query = query.OrderBy(modelDescription.StabilizerProperty.ExternalName);
            }
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
    /// Given a starting position and page size, returns a subset of the a collection. Use will
    /// typically immediately follow a call to `Sort`. Also merges in continuation token to supply
    /// values for paging if skip and take are missing. On conflict, the skip and take values will
    /// override the token.
    /// </summary>
    /// <param name="source">The queryable source, typically from result of call to `Sort`</param>
    /// <param name="skip">
    /// The number of records to skip, if paging this is the page number times the take size.
    /// </param>
    /// <param name="take">
    /// the number of records to take, this is the page size of the fetch. Use to balance call API
    /// latency versus bandwidth
    /// </param>
    /// <param name="continuationToken">
    /// If this is not a new request, the token passed back from the previous request to maintain
    /// stability (optional)
    /// </param>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int skip, int take, string? continuationToken)
    {
        var token = ContinuationToken.FromString(continuationToken);
        var actualSkip = ContinuationToken.ActualSkip(token, skip);
        var actualTake = ContinuationToken.ActualTake(token, take);
        return source.Skip(actualSkip).Take(actualTake);
    }

    private const string MissingStabilizerErrorMessage = "Sort requires that a single EF key is uniquely defined to stabalize the sort, even if another sort property is present.  Use a single unique key following EF conventions";

    private const string SortErrorUserMessage = "Unable to Sort. 0x0F3F241C";
}
