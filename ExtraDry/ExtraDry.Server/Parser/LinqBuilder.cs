using Microsoft.Azure.Amqp.Framing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;

namespace ExtraDry.Server.Internal;

/// <summary>
/// A lightweight dynamic linq builder, just enough to satisfy needs of filtering, sorting and paging API result sets.
/// </summary>
internal static class LinqBuilder
{

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
        var modelDescription = new ModelDescription(typeof(T));
        string[] props = property.Split('.');
        var type = typeof(T);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach(string prop in props) {
            var pi = modelDescription.SortProperties.FirstOrDefault(sortProp => string.Equals(sortProp.ExternalName, prop, StringComparison.OrdinalIgnoreCase))?.Property
                ?? (string.Equals(modelDescription.StabilizerProperty?.ExternalName, prop, StringComparison.OrdinalIgnoreCase)
                        ? modelDescription.StabilizerProperty!.Property
                        : throw new DryException($"Could not find sort property `{prop}`", "Could not apply requested sort"));
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
        var result = methodInfo.MakeGenericMethod(typeof(T), type)!.Invoke(null, [source, lambda])!;
        return (IOrderedQueryable<T>)result;
    }

    public static IQueryable<T> WhereVersionModified<T>(this IQueryable<T> source, EqualityType equality, DateTime timestamp)
    {
        // Build the tree for the following manually... (where 'Version' is the first property of type VersionInfo)
        // .Where(e => e.Version.DateModified >= timestamp)
        var param = Expression.Parameter(typeof(T), "e");

        Expression propertyExpression = param;
        var versionProperty = typeof(T).GetProperties().FirstOrDefault(e => e.PropertyType == typeof(VersionInfo))
            ?? throw new DryException("Can't do version modified after without a VersionInfo property.");
        propertyExpression = Expression.Property(propertyExpression, versionProperty);
        var modifiedProperty = typeof(VersionInfo).GetProperty(nameof(VersionInfo.DateModified))!;
        propertyExpression = Expression.Property(propertyExpression, modifiedProperty);

        var dateConstant = Expression.Constant(timestamp);

        var rangeExpression =
            equality == EqualityType.GreaterThan
                ? Expression.GreaterThan(propertyExpression, dateConstant)
                : Expression.Equal(propertyExpression, dateConstant);

        var lambda = Expression.Lambda<Func<T, bool>>(rangeExpression, param);

        return source.Where(lambda);
    }

    public static IQueryable<IGrouping<object, T>> GroupBy<T>(this IQueryable<T> source, StatisticsProperty property)
    {
        // Build up the group-by property, which is essentially the lambda `e => (object)e.propertyName`
        // The cast to object is required for enum support.
        var paramExpr = Expression.Parameter(typeof(T), "e");
        var statsProperty = typeof(T).GetProperty(property.Property.Name)
            ?? throw new DryException("Can't find property on class that defines the property?!?");
        var propertyExpr = Expression.Property(paramExpr, statsProperty);
        var objectPropertyExpr = Expression.Convert(propertyExpr, typeof(object));
        var keySelector = Expression.Lambda<Func<T, object>>(objectPropertyExpr, paramExpr);

        return source.GroupBy(keySelector);
    }

    /// <summary>
    /// Given a list of filter properties and a list of match strings, constructs a queryable for an existing queryable.
    /// </summary>
    /// <remarks>
    /// This builds a Conjunctive Normal Form (CNF) linq expression where each string in `matchValues` must exist in 
    /// at least one of the properties.  The exact comparison function is also determined by the properties' filter attribute.
    /// </remarks>
    public static IQueryable<T> WhereFilterConditions<T>(this IQueryable<T> source, FilterProperty[] filterProperties, string filterQuery, StringComparison? forceStringComparison = null)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var terms = new List<Expression>();
        var filter = FilterParser.Parse(filterQuery);
        foreach(var rule in filter.Rules) {
            var property = filterProperties.FirstOrDefault(e => string.Equals(e.ExternalName, rule.PropertyName, StringComparison.OrdinalIgnoreCase));
            if(rule.PropertyName == "*") {
                var keywords = new List<Expression>();
                foreach(var filterProperty in filterProperties) {
                    try {
                        AddTerms(param, keywords, rule, filterProperty, forceStringComparison);
                    }
                    catch {
                        // E.g. when "abc" is passed to an Int32, ignore when part of keyword/wildcard search.
                    }
                }
                if(keywords.Count != 0) {
                    if(keywords.Count == 1) {
                        terms.Add(keywords.First());
                    }
                    else {
                        terms.Add(AnyOf([.. keywords]));
                    }
                }
            }
            else if(property != null) {
                if(rule.LowerBound != BoundRule.None && rule.UpperBound != BoundRule.None && rule.Values.Count == 2) {
                    // range of values
                    var fields = RangeExpression(param, property.Property, rule);
                    terms.Add(AllOf(fields));
                }
                else {
                    AddTerms(param, terms, rule, property, forceStringComparison);
                }
            }
            else {
                // unrecognized filter => force to empty set.
                terms.Add(EmptySetExpression);
            }
        }
        if(terms.Count != 0) {
            var cnf = AllOf([.. terms]);
            var lambda = Expression.Lambda<Func<T, bool>>(cnf, param);
            return source.Where(lambda);
        }
        return source;
    }

    private static void AddTerms(ParameterExpression param, List<Expression> terms, FilterRule rule, FilterProperty property, StringComparison? forceStringComparison)
    {
        if(rule.Values is []) {
            throw new DryException("Filter expression for property was not provided.", $"Unable to apply filter for Property '{property.Property.Name}'. Make sure there are no spaces after '{property.Property.Name}:'  0x0F4042BE");
        }

        if(property.Property.PropertyType == typeof(string)) {
            var fields = rule.Values.Select(e => StringExpression(param, property.Property, property.Filter.Type, e, forceStringComparison)).ToArray();
            terms.Add(AnyOf(fields));
        }
        else {
            var fields = rule.Values.Select(e => ComparisonExpression(param, property.Property, e)).ToArray();
            terms.Add(AnyOf(fields));
        }
    }

    private static Expression AnyOf(Expression[] expressions)
    {
        var left = expressions.First();
        foreach(var right in expressions.Skip(1)) {
            left = Expression.OrElse(left, right);
        }
        return left;
    }

    private static Expression AllOf(Expression[] expressions)
    {
        var left = expressions.First();
        foreach(var right in expressions.Skip(1)) {
            left = Expression.AndAlso(left, right);
        }
        return left;
    }

    private static Expression ComparisonExpression(ParameterExpression parameter, PropertyInfo propertyInfo, string value)
    {
        try {
            var property = Expression.Property(parameter, propertyInfo);
            var valueConstant = Expression.Constant(ParseToType(propertyInfo.PropertyType, value));
            var equality = Expression.Equal(property, valueConstant);
            return equality;
        }
        catch(DryException) {
            // Can't construct or parse value, comparison is impossible and can't convert to expression.
            // Replace with a similarly impossible but syntactically correct expression.
            return EmptySetExpression;
        }
    }

    private static Expression EmptySetExpression => Expression.Equal(Expression.Constant(0), Expression.Constant(1));

    private static Expression[] RangeExpression(ParameterExpression parameter, PropertyInfo propertyInfo, FilterRule rule)
    {
        if(!propertyInfo.PropertyType.IsValueType) {
            throw new DryException("Filters that specify a range expression may only be used with value types that have comparisons defined.", "Unable to apply filter. 0x0F4DC1B1");
        }
        var type = propertyInfo.PropertyType;
        var expressions = new List<Expression>();
        if(!string.IsNullOrWhiteSpace(rule.Values[0])) {
            var valueExpr = ConstantToExpression(rule.Values[0], type);
            var propertyExpr = Expression.Property(parameter, propertyInfo);
            var lowerBoundExpr = rule.LowerBound switch {
                BoundRule.Exclusive => Expression.GreaterThan(propertyExpr, valueExpr),
                _ => Expression.GreaterThanOrEqual(propertyExpr, valueExpr),
            };
            expressions.Add(lowerBoundExpr);
        }
        if(!string.IsNullOrWhiteSpace(rule.Values[1])) {
            var valueExpr = ConstantToExpression(rule.Values[1], type);
            var propertyExpr = Expression.Property(parameter, propertyInfo);
            var upperBoundExpr = rule.UpperBound switch {
                BoundRule.Inclusive => Expression.LessThanOrEqual(propertyExpr, valueExpr),
                _ => Expression.LessThan(propertyExpr, valueExpr),
            };
            expressions.Add(upperBoundExpr);
        }
        if(expressions.Count == 0) {
            throw new DryException("Filters that specify a range, must include at least either a lower bound or an upper bound.", "Unable to apply filter. 0x0F30C48A");
        }
        return [.. expressions];

        static Expression ConstantToExpression(string value, Type type)
        {
            var expression = (Expression)Expression.Constant(ParseToType(type, value));
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                // If nullable property, need to convert non nullable values from ParseToType into nullable equivalents.
                expression = Expression.Convert(expression, type);
            }
            return expression;
        }
    }

    private static object ParseToType(Type type, string value)
    {
        try {
            // If nullable type, e.g. DateTime?, then parse the inner type, i.e. DateTime.
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = type.GetGenericArguments()[0];
            }
            if(type.IsEnum) {
                return Enum.Parse(type, value, ignoreCase: true);
            }
            else {
                var methodInfo = type.GetMethod("Parse", [typeof(string)])
                    ?? throw new DryException($"Can only filter on types that contain a Parse method, type '{type.Name}'.");
                // Parse contract will have a result and not nullable
                var result = methodInfo.Invoke(null, [value])!;
                return result;
            }
        }
        catch {
            throw new DryException("Filter expression was not of the correct type.", $"Unable to apply filter. Could not cast value '{value}' to type '{type.Name}'. 0x0F4A1089");
        }
    }

    private static MethodCallExpression StringExpression(ParameterExpression parameter, PropertyInfo propertyInfo, FilterType filterType, string value, StringComparison? forceStringComparison)
    {
        var property = Expression.Property(parameter, propertyInfo);
        var valueConstant = Expression.Constant(value);
        if(forceStringComparison != null) {
            var method = filterType switch {
                FilterType.Contains => StringContainsWithComparisonMethod,
                FilterType.StartsWith => StringStartsWithWithComparisonMethod,
                _ => StringEqualsWithComparisonMethod,
            };
            var ignoreConstant = Expression.Constant(forceStringComparison);
            return Expression.Call(property, method, valueConstant, ignoreConstant);
        }
        else {
            var method = filterType switch {
                FilterType.Contains => StringContainsMethod,
                FilterType.StartsWith => StringStartsWithMethod,
                _ => StringEqualsMethod,
            };
            return Expression.Call(property, method, valueConstant);
        }
    }

    private static MethodInfo StringContainsMethod => typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

    private static MethodInfo StringEqualsMethod => typeof(string).GetMethod(nameof(string.Equals), [typeof(string)])!;

    private static MethodInfo StringStartsWithMethod => typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!;

    private static MethodInfo StringContainsWithComparisonMethod => typeof(string).GetMethod(nameof(string.Contains), [typeof(string), typeof(StringComparison)])!;

    private static MethodInfo StringEqualsWithComparisonMethod => typeof(string).GetMethod(nameof(string.Equals), [typeof(string), typeof(StringComparison)])!;

    private static MethodInfo StringStartsWithWithComparisonMethod => typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string), typeof(StringComparison)])!;

    private enum OrderType
    {
        OrderBy,
        ThenBy,
        OrderByDescending,
        ThenByDescending,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EqualityType
    {
        GreaterThan,
        EqualTo,
    }

}
