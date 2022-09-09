#nullable disable

namespace ExtraDry.Blazor.Components.Internal;

public class Grouping {

    public string GroupingColumn { get; set; }

    public int GroupingDepth { get; set; }

    public bool IsGroup { get; set; }

    public bool IsExpanded { get; set; }

    public Action Toggle { get; set; }

}
