namespace ExtraDry.Blazor;

public interface IGroupProvider<T> {

    /// <summary>
    /// Return the group parent for the item, or null if the item is a top level item.
    /// </summary>
    public T? GetGroup(T item);

    public string GroupColumn { get; }

}
