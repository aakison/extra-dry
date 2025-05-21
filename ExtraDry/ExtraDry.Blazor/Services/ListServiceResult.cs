namespace ExtraDry.Blazor;

public readonly struct ListServiceResult<TItem>(IEnumerable<TItem> items, int count, int total)
{
    /// <summary>
    /// The items to provide.
    /// </summary>
    public IEnumerable<TItem> Items { get; } = items;

    /// <summary>
    /// The total item count in the source generating the items provided.
    /// </summary>    
    public int Total { get; } = total;

    public int Count { get; } = count;

}
