namespace ExtraDry.Blazor.Internal;

/// <summary>
/// A hierarchy builder as used by the <see cref="QueryBuilder"/>.  Supports expanding and 
/// collapsing individual noes of a hierarchy.
/// </summary>
public class HierarchyBuilder
{
    /// <summary>
    /// Expands the node with the specified slug, if possible.
    /// </summary>
    public bool Expand(string slug)
    {
        if(ExpandNodes.Contains(slug)) {
            return false;
        }
        if(CollapseNodes.Contains(slug)) {
            CollapseNodes.Remove(slug);
        }
        ExpandNodes.Add(slug);
        return true;
    }

    /// <summary>
    /// Collapse the node with the specified slug, if possible.
    /// </summary>
    public bool Collapse(string slug)
    {
        if(CollapseNodes.Contains(slug)) {
            return false;
        }
        if(ExpandNodes.Contains(slug)) {
            ExpandNodes.Remove(slug);
        }
        CollapseNodes.Add(slug);
        return true;
    }

    /// <summary>
    /// Resets all the expanded and collapsed nodes.
    /// </summary>
    public void Reset()
    {
        ExpandNodes.Clear();
        CollapseNodes.Clear();
    }

    internal List<string> ExpandNodes { get; } = new();

    internal List<string> CollapseNodes { get; } = new();

}
