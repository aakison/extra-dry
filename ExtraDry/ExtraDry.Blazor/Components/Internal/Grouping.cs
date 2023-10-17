#nullable disable

namespace ExtraDry.Blazor.Components.Internal;

[Obsolete("Properties have been moved to ListItemInfo<T>, or so I believe!")]
public class Grouping {

    public string GroupingColumn { get; set; }

    public int GroupingDepth { get; set; }

    public bool IsGroup { get; set; }

    public bool IsExpanded { get; set; }

    public Action Toggle { get; set; }

}
