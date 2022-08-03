#nullable enable

namespace ExtraDry.Blazor.Models;

/// <summary>
/// Represents fields that, if present, allow for additional formatting
/// options when displaying a MiniCard.  MiniCards are displayed in a variety
/// of locations, such as in a FlexiSelect component.
/// </summary>
public interface IMiniCardItem {

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

    /// <summary>
    /// The title for the card.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// An optional subtitle to be displayed with a MiniCard.
    /// </summary>
    public string? Subtitle { get; set; }

}
