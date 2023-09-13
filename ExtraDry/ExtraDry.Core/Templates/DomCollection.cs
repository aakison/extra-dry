using System.Collections.ObjectModel;

namespace ExtraDry.Core;

/// <summary>
/// A collection that is used for the Domain Object Model that is portable across Unity and Xamarin.
/// This is similar in concept to ObservableCollection but uses lambda expressions instead of events for updates.
/// </summary>
public class DomCollection<T> : Collection<T> {

    /// <summary>
    /// Action that is called whenever an item is added to this collection.
    /// Note: unlike events in ObservableCollection, only one Action can exist on each collection.
    /// </summary>
    public Action<T> OnInsert { get; set; }

    /// <summary>
    /// Action that is called whenever an item is removed from this collection.
    /// Note: unlike events in ObservableCollection, only one Action can exist on each collection.
    /// </summary>
    public Action<T> OnRemove { get; set; }

    protected override void ClearItems()
    {
        var allItems = this.ToArray();
        base.ClearItems();
        foreach(var item in allItems) {
            if(OnRemove != null) {
                OnRemove(item);
            }
        }
    }

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        if(OnInsert != null) {
            OnInsert(item);
        }
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        base.RemoveItem(index);
        if(OnRemove != null) {
            OnRemove(item);
        }
    }

    protected override void SetItem(int index, T item)
    {
        var oldItem = this[index];
        base.SetItem(index, item);
        if(OnRemove != null) {
            OnRemove(oldItem);
        }
        if(OnInsert != null) {
            OnInsert(item);
        }
    }

    /// <summary>
    /// Adds all of the items to this collection.  Typical AddRange behavior.
    /// </summary>
    public void AddRange(IEnumerable<T> items)
    {
        if(items == null) {
            throw new ArgumentNullException("items");
        }
        foreach(var item in items) {
            Add(item);
        }
    }
}
