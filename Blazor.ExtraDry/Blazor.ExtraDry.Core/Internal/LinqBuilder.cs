using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.ExtraDry {

    /// <summary>
    /// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
    /// </summary>
    internal static class LinqBuilder {

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

        public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, PropertyInfo property, string value)
        {
            return WhereStringMethod(source, property, ContainsMethod, value);
        }

        public static IQueryable<T> WhereStartsWith<T>(this IQueryable<T> source, PropertyInfo property, string value)
        {
            return WhereStringMethod(source, property, StartsWithMethod, value);
        }

        public static IQueryable<T> WhereStringEquals<T>(this IQueryable<T> source, PropertyInfo property, string value)
        {
            return WhereStringMethod(source, property, StringEqualsMethod, value);
        }

        private static MethodInfo ContainsMethod => typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

        private static MethodInfo StringEqualsMethod => typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) });

        private static MethodInfo StartsWithMethod => typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });

        private static IQueryable<T> WhereStringMethod<T>(IQueryable<T> source, PropertyInfo propertyInfo, MethodInfo method, string value)
        {
            var e = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(e, propertyInfo);
            var valueConstant = Expression.Constant(value);
            var lambda = Expression.Lambda<Func<T, bool>>(
                Expression.Call(property, method, valueConstant),
                e
            );  
            return source.Where(lambda);
        }
    }
}
