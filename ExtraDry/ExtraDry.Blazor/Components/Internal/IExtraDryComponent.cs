namespace ExtraDry.Blazor;

/// <summary>
/// Interface to determine common functionality for ExtraDry component development.
/// </summary>
public interface IExtraDryComponent
{
    /// <summary>
    /// The CSS Class for the root element of the control. This is added in addition to
    /// component-specific semantic class elements.
    /// </summary>
    string CssClass { get; set; }

    /// <summary>
    /// Additional attributes that are not captured by the control are chained to the root element
    /// of the component.
    /// </summary>
    Dictionary<string, object>? UnmatchedAttributes { get; set; }
}
