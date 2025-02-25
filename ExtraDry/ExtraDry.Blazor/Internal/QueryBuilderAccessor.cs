namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Create an instance to access the <see cref="QueryBuilder"/> that is associated with the Decorator.
/// </summary>
// Implements a shared mechanism for accessing the QueryBuilder associated with a Decorator. This
// uses weak references to ensure that the Decorator can be garbage collected when no longer in use.
public class QueryBuilderAccessor(object decorator)
{

    /// <summary>
    /// The SelectionSet that is associated with the Decorator.
    /// </summary>
    public QueryBuilder QueryBuilder { get; } = Register(decorator);

    private class WeakRegistration(object decorator)
    {
        public WeakReference<object> Decorator { get; } = new WeakReference<object>(decorator);

        public QueryBuilder QueryBuilder { get; } = new();
    }

    private static List<WeakRegistration> QueryBuilders { get; } = [];

    private static QueryBuilder Register(object decorator)
    {
        //Console.WriteLine($"Cleaning collection of {QueryBuilders.Count} items");
        QueryBuilders.RemoveAll(r => !r.Decorator.TryGetTarget(out _));
        //Console.WriteLine($"Collection now has {QueryBuilders.Count} items");
        //Console.WriteLine($"Looking up {decorator} in collection ");
        var existing = QueryBuilders.FirstOrDefault(r => r.Decorator.TryGetTarget(out var target) && target == decorator);
        if(existing == null) {
            //Console.WriteLine($"Not found, adding new Selection Set ");
            existing = new WeakRegistration(decorator);
            QueryBuilders.Add(existing);
        }
        //Console.WriteLine($"Registration finished with {QueryBuilders.Count} items");
        return existing.QueryBuilder;
    }
}
