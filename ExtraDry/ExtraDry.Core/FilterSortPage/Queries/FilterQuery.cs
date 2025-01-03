namespace ExtraDry.Core;

/// <summary>
/// Represents a basic query to filter against a list or hierarchy of items.
/// </summary>
public class FilterQuery
{
    /// <summary>
    /// The entity specific text filter for the collection.
    /// </summary>
    public string? Filter { get; set; }
}
