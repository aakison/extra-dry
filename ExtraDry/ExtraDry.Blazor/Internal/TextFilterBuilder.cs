namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A filter builder used by the PageQueryBuilder that supports user-entered free text.
/// </summary>
public class TextFilterBuilder : FilterBuilder {

    /// <summary>
    /// The free-text to be sent to the server, typically just space separated keywords.
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <inheritdoc cref="FilterBuilder.Build" />
    public override string Build() => Keywords.Trim();

    /// <inheritdoc cref="FilterBuilder.Reset" />
    public override void Reset() => Keywords = string.Empty;
}
