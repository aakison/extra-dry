using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.ExtraDry {

    /// <summary>
    /// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
    /// </summary>
    public static class LinqBuilder {

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
            // TODO: Implement filtering based on model meta-data.
            return source;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, string filter)
        {
            // TODO: Implement filtering based on model meta-data.
            return source;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, FilterQuery query)
        {
            // TODO: Implement stabalizer using model meta-data.
            var token = (query as PageQuery)?.Token; // Only need the token if it's a PageQuery, null if FilterQuery.
            return source.Sort(query.Sort, query.Ascending, "Id", token);
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
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sort, bool? ascending, string stabalizer, string continuationToken)
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

        /// <summary>
        /// Sorts the elements of the sequence according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        /// <summary>
        /// Sorts the elements of the sequence, in descending order, according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        /// <summary>
        /// Performs a subsequent ordering of a sequence according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        /// <summary>
        /// Performs a subsequent ordering of a sequence, in descending order, according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        /// <summary>
        /// Applies LINQ method by property name and method name instead of using Method and Lambda.
        /// </summary>
        /// <remarks>see https://stackoverflow.com/questions/41244/dynamic-linq-orderby-on-ienumerablet-iqueryablet</remarks>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach(string prop in props) {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                if(pi == null) {
                    throw new DryException($"Could not find sort property `{prop}`", "Could not apply requested sort");
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var methodInfo = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);
            if(methodInfo == null) {
                throw new DryException($"Could not find method `{methodName}`, must be one of `OrderBy`, `Thenby`, `OrderByDescending`, or `ThenByDescending`", "Internal Server Error - 0x0F72F021");
            }
            var result = methodInfo?.MakeGenericMethod(typeof(T), type)?.Invoke(null, new object[] { source, lambda });
            if(result == null) {
                throw new DryException($"Failed to execute order method `{methodName}`", "Internal Server Error - 0x0F0427A0");
            }
            return (IOrderedQueryable<T>)result;
        }

    }
}
