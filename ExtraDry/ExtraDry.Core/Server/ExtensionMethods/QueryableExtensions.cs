using ExtraDry.Core;
using ExtraDry.Server.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace ExtraDry.Server {

    /// <summary>
    /// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
    /// </summary>
    public static class QueryableExtensions {

        public static IPartialQueryable<T> QueryWith<T>(this IQueryable<T> source, PageQuery partialQuery, Expression<Func<T, bool>>? defaultFilter = null)
        {
            return new PartialQueryable<T>(source, partialQuery, defaultFilter);
        }

        public static IPartialQueryable<T> QueryWith<T>(this IQueryable<T> source, FilterQuery filteredQuery, Expression<Func<T, bool>>? defaultFilter = null)
        {
            return new PartialQueryable<T>(source, filteredQuery, defaultFilter);
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
            return source.WhereFilterConditions(filterProperties.ToArray(), filter);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, FilterQuery query)
        {
            var keyPropertyName = "Id";
            var type = typeof(T);
            var properties = type.GetProperties();

            var keyProperties = properties.Where(e => e.GetCustomAttributes(true).Any(e => e is KeyAttribute));
            
            if(!string.IsNullOrWhiteSpace(query.Stabilizer)) {
                keyPropertyName = query.Stabilizer;
            }
            else if(keyProperties.Count() == 1) {
                keyPropertyName = keyProperties.First().Name;
            }
            else if(keyProperties.Count() > 1) {
                throw new DryException("Sort requires that a single EF key is well defined to stabalize the sort, composite keys are not supported.  Manually specify a Stabilizer in the FilterQuery, or use a single [Key] attribute.", "Unable to Sort (0x0F3F241D)");
            }
            else if(properties.Any(e => e.Name == "Id")) {
                keyPropertyName = "Id";
            }
            else if(properties.Any(e => e.Name == $"{type.Name}Id")) {
                keyPropertyName = $"{type.Name}Id";
            }
            else {
                throw new DryException("Sort requires that an EF key is uniquely defined to stabalize the sort, even if another sort property is present.  Create a unique key following EF conventions or specify a Stabilizer in the FilterQuery.", "Unable to Sort (0x0F3F241C)");
            }

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
        /// <param name="stabilizer">The name of a unique property to ensure paging works, use monotonically increasing value such as `int Identity` or created timestamp (required, case insensitive)</param>
        /// <param name="token">If this is not a new request, the token passed back from the previous request to maintain stability (optional)</param>
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string? sort, bool? ascending, string stabilizer, string? continuationToken)
        {
            var token = ContinuationToken.FromString(continuationToken);
            var actualSort = token?.Sort ?? sort;
            var actualStabilizer = token?.Stabilizer ?? stabilizer;
            var actualAscending = token?.Ascending ?? ascending ?? true;
            var query = source;
            if(string.IsNullOrWhiteSpace(stabilizer)) {
                throw new DryException($"Must supply a stabilizer to ensure paging is consistent", "Internal Server Error - 0x0F850FD9");
            }
            if(!string.IsNullOrWhiteSpace(actualSort)) {
                query = actualAscending ? 
                    query.OrderBy(actualSort).ThenBy(actualStabilizer) : 
                    query.OrderByDescending(actualSort).ThenByDescending(actualStabilizer);
            }
            else {
                query = query.OrderBy(stabilizer);
            }
            return query;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, PageQuery partialQuery)
        {
            // TODO: Implement stabilizer using model meta-data.
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
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int skip, int take, string? continuationToken)
        {
            var token = ContinuationToken.FromString(continuationToken);
            var actualSkip = ContinuationToken.ActualSkip(token, skip);
            var actualTake = ContinuationToken.ActualTake(token, take);
            return source.Skip(actualSkip).Take(actualTake);
        }

    }
}
