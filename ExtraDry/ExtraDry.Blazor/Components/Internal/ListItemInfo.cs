namespace ExtraDry.Blazor.Components.Internal;

/// <summary>
/// Inside of visual collections, a list item that includes information for display and grouping,
/// along with a reference to the actual item.
/// </summary>
public class ListItemInfo<TItem> {

    /// <summary>
    /// The actual item, typically linked to another collection.
    /// </summary>
    public TItem? Item { get; set; }

    /// <summary>
    /// Indicates if the item has been loaded.  Used when getting pages of data from the server 
    /// and the actual items might be unloaded.
    /// </summary>
    public bool IsLoaded { get; set; }

    /// <summary>
    /// Indicates how far down the grouping this item is.  0 implies not in any group or the head 
    /// of a group.
    /// </summary>
    public int GroupDepth { get; set; }

    /// <summary>
    /// If grouping is enabled, the group that this item is a member of.
    /// </summary>
    public ListItemInfo<TItem>? Group { get; set; }

    /// <summary>
    /// If grouping is enabled, indicates if this item also acts as a group of other items.
    /// </summary>
    public bool IsGroup { get; set; }

    /// <summary>
    /// Indicates if the current node is collapsed, this node is still shown but elements grouped 
    /// under it are not.
    /// </summary>
    public bool IsExpanded { get; set; } = true;

    /// <summary>
    /// Indicates if the item is actually shown based on the visibility of the group it is in.
    /// </summary>
    public bool IsShown => (Group?.IsExpanded ?? true) && (Group?.IsShown ?? true);

}
