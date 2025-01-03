namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A base filter builder as used by the PageQueryBuilder. Base classes support each of the
/// different filter tyes, such as free-text, enum select lists, etc.
/// </summary>
public abstract class FilterBuilder
{
    /// <summary>
    /// The name of the filter as will be sent to the server.
    /// </summary>
    public string FilterName { get; set; } = string.Empty;

    /// <summary>
    /// When the filter has changed, builds up the ExtraDry filter query fragment that is to be
    /// sent to the server.
    /// </summary>
    public abstract string Build();

    /// <summary>
    /// Resets the local storage of the filter so that no filters are applied.
    /// </summary>
    public abstract void Reset();
}
