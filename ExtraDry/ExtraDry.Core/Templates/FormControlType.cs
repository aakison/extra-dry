namespace ExtraDry.Core;

/// <summary>
/// The type requested for the form control.  Typically this will just be Input or Display.
/// Other enumerated types are available to override the default behaviour.
/// </summary>
public enum FormControlType {

    /// <summary>
    /// The control type is not known or not specified.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The control should be used for input based on the settings of the associated field.
    /// </summary>
    Input,

    /// <summary>
    /// The control should be used for display (no editing), format is determined by the settings of the associated field.
    /// </summary>
    Display,

    /// <summary>
    /// The control should be a single line text field, independent of the underlying type.
    /// The type Input should be preferred over this.
    /// </summary>
    Text,

    /// <summary>
    /// The control should be a date control, independent of the underlying type.
    /// The type Input should be preferred over this.
    /// </summary>
    Date,

    /// <summary>
    /// The control should be displayed as multi-line text, independent of the underlying type.
    /// The type Input should be preferred over this.
    /// </summary>
    MultilineText,

    /// <summary>
    /// The control should be displayed as a pick-list, independent of the underlying type.
    /// This may not have any meaning if the forms builder cannot determine a set of valid values.
    /// The type Input should be preferred over this.
    /// </summary>
    PickList,

    /// <summary>
    /// This control should render a filmstrip control with an icon to call the camera
    /// </summary>
    Filmstrip,

    /// <summary>
    /// This control renders a Dummy Field for iOS to capture the RFID/Barcode data.
    /// Used by Capture
    /// </summary>
    DummyTextField,

    /// <summary>
    /// The control should be a datetime control, independent of the underlying type.
    /// </summary>
    DateTime,

    /// <summary>
    /// The control should be a Time control, independent of the underlying type.
    /// </summary>
    Time,

    /// <summary>
    /// The control should be displayed as multi-line text.
    /// </summary>
    StaticText,

    /// <summary>
    /// The control should be displayed as multi-line text with a warning icon.
    /// </summary>
    Warning,

    /// <summary>
    /// Checklist Control
    /// </summary>
    CheckList,

    /// <summary>
    /// The control should be displayed as an integer with the % symbol. When the control is of type Percentage it's irrelevant to know whether it's Display/Input
    /// </summary>
    Percentage
}
