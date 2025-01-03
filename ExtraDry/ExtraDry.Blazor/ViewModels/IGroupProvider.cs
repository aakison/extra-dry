namespace ExtraDry.Blazor;

public interface IGroupProvider<T>
{
    public T GetGroup(T item);

    public string GroupColumn { get; }
}
