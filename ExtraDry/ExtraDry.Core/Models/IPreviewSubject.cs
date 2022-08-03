#nullable enable

namespace ExtraDry.Core;

/// <summary>
/// Represents fields that, if present, allow for additional formatting
/// options when displaying on a MiniCard.  MiniCards are displayed in a 
/// variety of locations, such as in a FlexiSelect component.
/// </summary>
public interface IPreviewSubject {

    /// <summary>
    /// Additional CSS classes to be included with the item when it is 
    /// shown as part of a card.  Useful for additional formatting when
    /// only a few types of cards are available.  
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// An optional thumbnail to be presented with each of the cards.
    /// </summary>
    public string? Thumbnail { get; set; }

}
