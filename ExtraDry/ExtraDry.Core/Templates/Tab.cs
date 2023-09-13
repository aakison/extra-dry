namespace ExtraDry.Core; 
/// <summary>
///     Represents a pre-defined search criteria
/// </summary>
public class Tab
{
    /// <summary>
    ///     The Name of the tab
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     The Index to search on
    /// </summary>
    public string Index { get; set; }

    /// <summary>
    ///     A set of search values and their friendly display name
    /// </summary>
    public TabValueCollection Values { get; set; }
}
