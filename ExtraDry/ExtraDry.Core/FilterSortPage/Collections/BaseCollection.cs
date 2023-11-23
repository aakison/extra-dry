namespace ExtraDry.Core;

/// <summary>
/// A basic collection of items from the API.
/// </summary>
public class BaseCollection<T>
{
    /// <summary>
    /// The UTC date/time that the collection was created.  
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The total number of items in the result set.
    /// </summary>
    /// <example>1</example>
    public int Count => Items.Count;

    /// <summary>
    /// The actual collection of items.
    /// </summary>
    public IList<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Create a new <see cref="BaseCollection{T}" /> with the items cast to a base class or interface.
    /// </summary>
    public BaseCollection<TCast> Cast<TCast>() => new() {
        Created = Created,
        Items = Items.Cast<TCast>().ToList(),
    };

}

