namespace ExtraDry.Core;

/// <summary>
/// A sorted and filtered collection of items from the API.
/// </summary>
public class SortedCollection<T> : FilteredCollection<T>
{
    /// <summary>
    /// The order and name of the Property the sort is performed on.
    /// </summary>
    /// <example>property</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Sort {
        get => sort;
        set => sort = string.IsNullOrWhiteSpace(value) ? null : value;
    }
    private string? sort;

    /// <summary>
    /// Create a new <see cref="FilteredCollection{T}" /> with the items cast to a base class or interface.
    /// </summary>
    public new SortedCollection<TCast> Cast<TCast>() => new() {
        Filter = Filter,
        Created = Created,
        Sort = Sort,
        Items = Items.Cast<TCast>().ToList(),
    };

}

