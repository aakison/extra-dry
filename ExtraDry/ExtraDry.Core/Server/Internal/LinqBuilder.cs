﻿using ExtraDry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Server.Internal {

    /// <summary>
    /// A very lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
    /// </summary>
    internal static class LinqBuilder {

        /// <summary>
        /// Sorts the elements of the sequence according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, OrderType.OrderBy);
        }

        /// <summary>
        /// Sorts the elements of the sequence, in descending order, according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, OrderType.OrderByDescending);
        }

        /// <summary>
        /// Performs a subsequent ordering of a sequence according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, OrderType.ThenBy);
        }

        /// <summary>
        /// Performs a subsequent ordering of a sequence, in descending order, according to a key which is provided by name instead of a lambda.
        /// </summary>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, OrderType.ThenByDescending);
        }

        /// <summary>
        /// Applies LINQ method by property name and method name instead of using Method and Lambda.
        /// </summary>
        /// <remarks>see https://stackoverflow.com/questions/41244/dynamic-linq-orderby-on-ienumerablet-iqueryablet</remarks>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, OrderType methodType)
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
                    method => method.Name == methodType.ToString()
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2);
            // Feels weird to explicitly state these methods exist and return values, but enforced by rigorous lookup above.
            var result = methodInfo.MakeGenericMethod(typeof(T), type)!.Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// Given a list of filter properties and a list of match strings, constructs a queryable for an existing queryable.
        /// </summary>
        /// <remarks>
        /// This builds a Conjunctive Normal Form (CNF) linq expression where each string in `matchValues` must exist in 
        /// at least one of the properties.  The exact comparison function is also determined by the properties' filter attribute.
        /// </remarks>
        public static IQueryable<T> WhereFilterConditions<T>(this IQueryable<T> source, FilterProperty[] filterProperties, string filterQuery)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var terms = new List<Expression>();
            var filter = FilterParser.Parse(filterQuery);
            foreach(var rule in filter.Rules) {
                var property = filterProperties.FirstOrDefault(e => string.Equals(e.Property.Name, rule.PropertyName, StringComparison.OrdinalIgnoreCase));
                if(property == null) {
                    throw new DryException($"Could not find property '{rule.PropertyName}' requested in filter query.  No property had with that name has a [Filter] attribute applied to it.", "Unable to apply filter. 0x0F4F4931");
                }
                if(rule.LowerBound != BoundRule.None && rule.UpperBound != BoundRule.None && rule.Values.Count == 2) {
                    // range of values
                    var fields = RangeExpression(param, property.Property, rule);
                    terms.Add(AllOf(fields));
                }
                else if(property.Property.PropertyType == typeof(string)) {
                    var fields = rule.Values.Select(e => StringExpression(param, property.Property, property.Filter.Type, e)).ToArray();
                    terms.Add(AnyOf(fields));
                }
                else {
                    var fields = rule.Values.Select(e => ComparisonExpression(param, property.Property, e)).ToArray();
                    terms.Add(AnyOf(fields));
                }
            }

            //foreach(var match in matchValues) {
            //    var fields = filterProperties.Select(e => StringExpression(param, e.Property, e.Filter.Type, match)).ToArray();
            //    terms.Add(AnyOf(fields));
            //}
            var cnf = AllOf(terms.ToArray());
            var lambda = Expression.Lambda<Func<T, bool>>(cnf, param);
            return source.Where(lambda);
        }

        private static Expression AnyOf(Expression[] expressions)
        {
            var left = expressions.FirstOrDefault();
            foreach(var right in expressions.Skip(1)) {
                left = Expression.OrElse(left, right);
            }
            return left;
        }

        private static Expression AllOf(Expression[] expressions)
        {
            var left = expressions.FirstOrDefault();
            foreach(var right in expressions.Skip(1)) {
                left = Expression.AndAlso(left, right);
            }
            return left;
        }

        private static Expression ComparisonExpression(ParameterExpression parameter, PropertyInfo propertyInfo, string value)
        {
            var property = Expression.Property(parameter, propertyInfo);
            var valueConstant = Expression.Constant(ParseToType(propertyInfo.PropertyType, value));
            var equality = Expression.Equal(property, valueConstant);
            return equality;
        }

        private static Expression[] RangeExpression(ParameterExpression parameter, PropertyInfo propertyInfo, FilterRule rule)
        {
            if(!propertyInfo.PropertyType.IsValueType) {
                throw new DryException("Filters that specify a range expression may only be used with value types that have comparisons defined.", "Unable to apply filter. 0x0F4DC1B1");
            }
            var lowerValue = Expression.Constant(ParseToType(propertyInfo.PropertyType, rule.Values[0]));
            var upperValue = Expression.Constant(ParseToType(propertyInfo.PropertyType, rule.Values[1]));
            var property = Expression.Property(parameter, propertyInfo);
            var lowerBound = rule.LowerBound switch {
                BoundRule.Exclusive => Expression.GreaterThan(property, lowerValue),
                _ => Expression.GreaterThanOrEqual(property, lowerValue),
            };
            var upperBound = rule.UpperBound switch {
                BoundRule.Inclusive => Expression.LessThanOrEqual(property, upperValue),
                _ => Expression.LessThan(property, upperValue),
            };
            return new Expression[] { lowerBound, upperBound };
        }

        private static object ParseToType(Type type, string value)
        {
            var methodInfo = type.GetMethod("Parse", new Type[] { typeof(string) });
            var result = methodInfo.Invoke(null, new object[] { value });
            return result;
        }

        private static Expression StringExpression(ParameterExpression parameter, PropertyInfo propertyInfo, FilterType filterType, string value)
        {
            var property = Expression.Property(parameter, propertyInfo);
            var valueConstant = Expression.Constant(value);
            var caseConstant = Expression.Constant(StringComparison.InvariantCultureIgnoreCase);
            var method = filterType switch {
                FilterType.Contains => StringContainsMethod,
                FilterType.StartsWith => StringStartsWithMethod,
                _ => StringEqualsMethod,
            };
            return Expression.Call(property, method, valueConstant, caseConstant);
        }

        private static MethodInfo StringContainsMethod => typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string), typeof(StringComparison) });

        private static MethodInfo StringEqualsMethod => typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(StringComparison) });

        private static MethodInfo StringStartsWithMethod => typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string), typeof(StringComparison) });

        private enum OrderType {
            OrderBy, 
            ThenBy, 
            OrderByDescending, 
            ThenByDescending,
        }

    }
}