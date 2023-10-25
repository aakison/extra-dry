namespace ExtraDry.Core;

/// <summary>
/// Represents a hierarchy query to filter against a list of items.
/// </summary>
public class HierarchyQuery : FilterQuery
{

    public int Level { get; set; }

    public List<string> ExpandedNodes { get; set; } = new();

    public List<string> CollapsedNodes { get; set; } = new();

}
