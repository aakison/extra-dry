namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A filter builder used by the PageQueryBuilder that supports user-entered free text.
/// </summary>
public class TextFilterBuilder : FilterBuilder
{
    /// <summary>
    /// The free-text to be sent to the server, typically just space separated keywords.
    /// </summary>
    public string Keywords { get; set; } = "";

    /// <inheritdoc cref="FilterBuilder.Build" />
    public override string Build()
    {
        if(FilterName == "") {
            return Keywords.Trim();
        }
        else {
            return $"{FilterName}:{Keywords.Trim()}";
        }
    }

    /// <inheritdoc cref="FilterBuilder.Reset" />
    public override void Reset()
    {
        Keywords = string.Empty;
    }
}
