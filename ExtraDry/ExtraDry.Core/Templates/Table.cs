using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Represents a responsive mechanism for displaying tables and columns of tags.
/// </summary>
public class Table {

    /// <summary>
    /// Create a table that has a name as well as a set of columns.
    /// </summary>
    public Table()
    {
        Initialize();
    }

    private void Initialize()
    {
        Columns = new TableColumnCollection();
    }

    /// <summary>
    /// The name of the table, this could be set to the template name of the tag or anything else the user wants to see it as.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The collection of columns that define how the data is to be presented to the user.
    /// </summary>
    public TableColumnCollection Columns { get; private set; }

    /// <summary>
    /// The Sort Index that is used for lexicographically sorting the tags in the table.
    /// </summary>
    public string SortIndex { get; set; }

    /// <summary>
    /// If a DefaultSort is provided, the order in which that sort is performed.
    /// The default order is Ascending
    /// </summary>
    public SortOrderType DefaultOrder { get; set; }

    /// <summary>
    /// The Sort param that is used for lexicographically sorting the tags in the table.
    /// </summary>
    public string DefaultSort { get; set; }
}
