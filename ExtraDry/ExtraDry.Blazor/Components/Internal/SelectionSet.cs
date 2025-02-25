using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Represents a list of items that can be selected, supporting both single-select and multi-select
/// lists. Provides an efficient mechanism for the common selecting scenarios in the user
/// interface. Selection-Sets are tied to decorators and need to be accessed through <see
/// cref="SelectionSetAccessor" /> which is a lightweight object for accessing the shared
/// SelectionSet in multiple locations.
/// </summary>
public class SelectionSet 
{
    internal SelectionSet() { }

    public void SetVisible(IEnumerable<object> items)
    {
        //Console.WriteLine($"SetVisible {items.Count()}");
        visibleItems.Clear();
        visibleItems.AddRange(items);
    }

    public IEnumerable<object> Items => selectedItems.Intersect(visibleItems);

    public bool MultipleSelect { get; set; }

    public event EventHandler<SelectionSetChangedEventArgs> Changed = null!;

    public void Add(object item)
    {
        Console.WriteLine($"Add {item.GetHashCode()}");
        if(selectedItems.Contains(item)) {
            return;
        }
        if(!visibleItems.Contains(item)) {
            return;
        }
        var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Added };
        if(!MultipleSelect) {
            args.Removed.AddRange(selectedItems);
            if(selectedItems.Count != 0) {
                args.Type = SelectionSetChangedType.Changed;
                selectedItems.Clear();
            }
        }
        args.Added.Add(item);
        selectedItems.Add(item);
        Changed?.Invoke(this, args);
    }

    public bool All() => visibleItems.Count > 0 && visibleItems.All(selectedItems.Contains);

    public bool Any() => visibleItems.Count > 0 && selectedItems.Count > 0;

    public void Clear()
    {
        Console.WriteLine($"Clear");
        if(selectedItems.Count == 0) {
            return;
        }
        var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Cleared };
        args.Removed.AddRange(selectedItems);
        selectedItems.Clear();
        Changed?.Invoke(this, args);
    }

    public bool Contains(object item) => selectedItems.Contains(item);

    public void Remove(object item)
    {
        Console.WriteLine($"Remove {item.GetHashCode()}");
        if(Contains(item)) {
            var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Removed };
            args.Removed.Add(item);
            selectedItems.Remove(item);
            Changed?.Invoke(this, args);
        }
    }

    /// <summary>
    /// Selects all of the visible items.  This does not remove items that are shadow-selected.
    /// </summary>
    public void SelectAll()
    {
        Console.WriteLine($"Select All");
        if(MultipleSelect == false) { 
            throw new InvalidOperationException("Can't perform SelectAll() on a single selection set.");
        }
        var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.SelectAll };
        foreach(var item in visibleItems) {
            if(!Contains(item)) {
                args.Added.Add(item);
                selectedItems.Add(item);
            }
        }
        if(args.Added.Count > 0) {
            Changed?.Invoke(this, args);
        }
    }


    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Good enough for LINQ, good enough here.")]
    public bool Single() => selectedItems.Count == 1;

    private readonly List<object> selectedItems = [];

    private readonly List<object> visibleItems = [];
}

