namespace ExtraDry.Core;

/// <summary>
/// Represents a Field of a Template, this defines the name, data type, etc. for Values that are assigned Tags.
/// </summary>
public class ExpandoField {

    public Guid Uuid { get; set; } = Guid.NewGuid();

    public string Label { get; set; } = "label";

    /// <summary>
    /// A description for the field that can present more information than the name.
    /// This may be presented to users in the form of a tooltip or similar.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The data type for the field, these are high level data types like found in JavaScript.
    /// </summary>
    public ExpandoDataType DataType { get; set; }

    /// <summary>
    /// The default value for the field, to be populated by the client when a form is loaded.
    /// If the form is "cancelled", then the default should be reverted.
    /// </summary>
    public object? Value { get; set; }

    public string Icon { get; set; } = string.Empty;

    public string Placeholder { get; set; } = string.Empty;

    public List<Constraint> Constraints { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating how the Data Warehouse should interpret this value.
    /// </summary>
    public WarehouseBehavior WarehouseBehavior { get; set; }

    public int Order { get; set; }

    public ExpandoState State { get; set; } = ExpandoState.Draft;

}

