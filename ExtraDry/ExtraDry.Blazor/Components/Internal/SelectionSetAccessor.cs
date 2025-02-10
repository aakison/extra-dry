namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Create an instance to access the <see cref="SelectionSet"/> that is associated with the Decorator.
/// </summary>
// Implements a shared mechanism for accessing the SelectionSet associated with a Decorator. This
// uses weak references to ensure that the Decorator can be garbage collected when no longer in use.
public class SelectionSetAccessor(object decorator)
{

    /// <summary>
    /// The SelectionSet that is associated with the Decorator.
    /// </summary>
    public SelectionSet SelectionSet { get; } = Register(decorator);

    private class WeakRegistration(object decorator)
    {
        public WeakReference<object> Decorator { get; } = new WeakReference<object>(decorator);

        public SelectionSet SelectionSet { get; } = new();
    }

    private static List<WeakRegistration> SelectionSets { get; } = [];

    private static SelectionSet Register(object decorator)
    {
        Console.WriteLine($"Cleaning collection of {SelectionSets.Count} items");
        SelectionSets.RemoveAll(r => !r.Decorator.TryGetTarget(out _));
        Console.WriteLine($"Collection now has {SelectionSets.Count} items");
        Console.WriteLine($"Looking up {decorator} in collection ");
        var existing = SelectionSets.FirstOrDefault(r => r.Decorator.TryGetTarget(out var target) && target == decorator);
        if(existing == null) {
            Console.WriteLine($"Not found, adding new Selection Set ");
            existing = new WeakRegistration(decorator);
            SelectionSets.Add(existing);
        }
        Console.WriteLine($"Registration finished with {SelectionSets.Count} items");
        return existing.SelectionSet;
    }
}

