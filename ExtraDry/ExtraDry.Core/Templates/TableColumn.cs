using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// A column of information that is displayed when a table of tags is displayed.
/// Defines the header and caption fields as well of the display of elements.
/// </summary>
[XmlType(TypeName = "Column")]
public class TableColumn {

    /// <summary>
    /// The caption for the column, may be presented at the top of the table above the column.
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// The field name for the element that is shown in the column.
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// The display for the content, this can be a mix of various values as well as values from related tags.
    /// This tag also allows for static text and multiple values to be joined into a single element.
    /// This is only valid when the ControlType is 'Display'.
    /// Note: Display will be shown verbatim and is not to be used to inject HTML or other control codes.
    /// </summary>
    public string Display { get; set; }

    /// <summary>
    /// If the cell should link to a URI or even to a new page, this is optionally used to wrap the field.
    /// </summary>
    public string Hyperlink { get; set; }

    /// <summary>
    /// The type of control that should be used to present the element.
    /// Commonly this is Display, which just shows the element, Icon can be used to display the associated icon instead.
    /// </summary>
    public TableControlType ControlType { get; set; }

    /// <summary>
    /// The requested alignment for this Table Column.
    /// </summary>
    public Alignment Alignment { get; set; }
}
