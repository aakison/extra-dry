using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Core;

/// <summary>
/// A filtered collection of items from the API.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Not a C# collection, but an over-the-wire collection.")]
public class FilteredCollection<T> : BaseCollection<T>
{
    /// <summary>
    /// If the full collection is a subset of all items, this is the query that was used to filter
    /// the full collection.
    /// </summary>
    /// <example>term property:value</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Filter {
        get => filter;
        set => filter = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private string? filter;

    /// <summary>
    /// Create a new <see cref="FilteredCollection{T}" /> with the items cast to a base class or
    /// interface.
    /// </summary>
    public new FilteredCollection<TCast> Cast<TCast>() => new() {
        Filter = Filter,
        Created = Created,
        Items = Items.Cast<TCast>().ToList(),
    };
}
