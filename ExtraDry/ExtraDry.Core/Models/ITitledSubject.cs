#nullable enable

namespace ExtraDry.Core;

/// <summary>
/// Represents fields that, if present, allow for additional formatting
/// options when displaying.
/// </summary>
public interface ITitleSubject {

    /// <summary>
    /// The title for the card.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// An optional subtitle to be displayed with a MiniCard.
    /// </summary>
    public string? Subtitle { get; set; }

}
