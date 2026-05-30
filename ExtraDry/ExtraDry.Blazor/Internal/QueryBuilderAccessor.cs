namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Create an instance to access the <see cref="QueryBuilder"/> that is associated with the Decorator.
/// </summary>
public class QueryBuilderAccessor(object decorator)
{

    /// <summary>
    /// The SelectionSet that is associated with the Decorator.
    /// </summary>
    public QueryBuilder QueryBuilder { get; } = Register(decorator);

    private class Registration(object decorator)
    {
        public QueryBuilder QueryBuilder { get; } = new();

        public string DecoratorType { get; init; } = decorator.GetType().FullName ?? "";
    }

    private static List<Registration> QueryBuilders { get; } = [];

    private static QueryBuilder Register(object decorator)
    {
        var type = decorator.GetType().FullName ?? "";

        // Last one in stack is what we're looking for, multiple accessors on the same page.
        if(QueryBuilders.LastOrDefault()?.DecoratorType == type) {
            return QueryBuilders.Last().QueryBuilder;
        }

        // Query builder not already in stack, new navigation, add it.
        if(!QueryBuilders.Any(e => e.DecoratorType == type)) {
            var builder = new Registration(decorator);
            QueryBuilders.Add(builder);
            return builder.QueryBuilder;
        }

        // Navigating back to previous page, remove last and use previous.
        if(QueryBuilders.Count > 1 && QueryBuilders[^2].DecoratorType == type) {
            QueryBuilders.RemoveAt(QueryBuilders.Count - 1);
            return QueryBuilders.Last().QueryBuilder;
        }

        // Navigating back to earlier page, clear the whole stack 
        if(QueryBuilders.Any(e => e.DecoratorType == type)) {
            QueryBuilders.Clear();
            var builder = new Registration(decorator);
            QueryBuilders.Add(builder);
            return builder.QueryBuilder;
        }

        QueryBuilders.Clear();
        var bad = new Registration(decorator);
        QueryBuilders.Add(bad);
        return bad.QueryBuilder;
    }
}
