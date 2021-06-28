#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blazor.ExtraDry {

    /// <summary>
    /// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
    /// </summary>
    public static class QueryableExtensions {

        public static IPartialQueryable<T> QueryWith<T>(this IQueryable<T> source, PageQuery partialQuery)
        {
            return new PartialQueryable<T>(source, partialQuery);
        }

        public static IPartialQueryable<T> QueryWith<T>(this IQueryable<T> source, FilterQuery filteredQuery)
        {
            return new PartialQueryable<T>(source, filteredQuery);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, FilterQuery filterQuery)
        {
            return source.Filter(filterQuery.Filter);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, string? filter)
        {
            if(string.IsNullOrWhiteSpace(filter)) {
                return source;
            }
            var description = new ModelDescription(typeof(T));
            var filterProperties = description.FilterProperties;
            if(!filterProperties.Any()) {
                return source;
            }
            return source.WhereFilterConditions(filterProperties.ToArray(), filter.Split(' '));
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, FilterQuery query)
        {
            var keyPropertyName = "Id";
            var propInfo = typeof(T).GetProperties().FirstOrDefault(e => e.GetCustomAttributes(true).Any(e => e is KeyAttribute));
            if(propInfo != null) {
                keyPropertyName = propInfo.Name;
            }

            // TODO: Implement stabalizer using model meta-data for Composite keys.
            var token = (query as PageQuery)?.Token; // Only need the token if it's a PageQuery, null if FilterQuery.
            return source.Sort(query.Sort, query.Ascending, keyPropertyName, token);
        }

        /// <summary>
        /// Given a name of a property and an order, dynamically creates a sort query.
        /// Also merges in the continuation token to supply values for consisted paging.
        /// On conflict, the token will override the specified sort.
        /// </summary>
        /// <param name="source">The queryable source, typically from EF, this is from `DbSet.AsQueryable()`</param>
        /// <param name="sort">The name of the property to sort by (optional, case insensitive)</param>
        /// <param name="ascending">Indicates if the order is ascending or not (optional, default true)</param>
        /// <param name="stabalizer">The name of a unique property to ensure paging works, use monotonically increasing value such as `int Identity` or created timestamp (required, case insensitive)</param>
        /// <param name="token">If this is not a new request, the token passed back from the previous request to maintain stability (optional)</param>
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string? sort, bool? ascending, string stabalizer, string? continuationToken)
        {
            var token = ContinuationToken.FromString(continuationToken);
            var actualSort = token?.Sort ?? sort;
            var actualStabalizer = token?.Stabalizer ?? stabalizer;
            var actualAscending = token?.Ascending ?? ascending ?? true;
            var query = source;
            if(string.IsNullOrWhiteSpace(stabalizer)) {
                throw new DryException($"Must supply a stabalizer to ensure paging is consistent", "Internal Server Error - 0x0F850FD9");
            }
            if(!string.IsNullOrWhiteSpace(actualSort)) {
                query = actualAscending ? 
                    query.OrderBy(actualSort).ThenBy(actualStabalizer) : 
                    query.OrderByDescending(actualSort).ThenByDescending(actualStabalizer);
            }
            else {
                query = query.OrderBy(stabalizer);
            }
            return query;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, PageQuery partialQuery)
        {
            // TODO: Implement stabalizer using model meta-data.
            return source.Page(partialQuery.Skip, partialQuery.Take, partialQuery.Token);
        }

        /// <summary>
        /// Given a starting position and page size, returns a subset of the a collection.
        /// Use will typically immediately follow a call to `Sort`.
        /// Also merges in continuation token to supply values for paging if skip and take are missing.
        /// On conflict, the skip and take values will override the token.
        /// </summary>
        /// <param name="source">The queryable source, typically from result of call to `Sort`</param>
        /// <param name="skip">The number of records to skip, if paging this is the page number times the take size.</param>
        /// <param name="take">the number of records to take, this is the page size of the fetch.  Use to balance call API latency versus bandwidth</param>
        /// <param name="token">If this is not a new request, the token passed back from the previous request to maintain stability (optional)</param>
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int skip, int take, string continuationToken)
        {
            var token = ContinuationToken.FromString(continuationToken);
            var actualSkip = ContinuationToken.ActualSkip(token, skip);
            var actualTake = ContinuationToken.ActualTake(token, take);
            return source.Skip(actualSkip).Take(actualTake);
        }

    }
}
