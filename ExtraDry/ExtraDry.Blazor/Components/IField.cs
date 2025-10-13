using ExtraDry.Blazor;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A base interface for all fields.
/// </summary>
public interface IField
{
    /// <summary>
    /// Indicates if the field is read-only.
    /// </summary>
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Clash is between CSS and C#, more confusing to change it from the CSS name.")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// The CSS class or classes to apply to the field, merged with field semantic classes.
    /// </summary>
    public string CssClass { get; set; }

    /// <summary>
    /// The key for the icon to display with the field, if any.  Placed in front of the field.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Determines if the icon is shown when an icon is provided.
    /// </summary>
    public bool ShowIcon { get; set; }

    /// <summary>
    /// The key for the affordance icon to display with the field, if any.  Placed after the field.
    /// </summary>
    public string Affordance { get; set; }

    /// <summary>
    /// Determines if the affordance icon is shown when an affordance is provided.
    /// </summary>
    public bool ShowAffordance { get; set; }

    /// <summary>
    /// The placeholder text that is displayed when the field is empty.
    /// </summary>
    public string Placeholder { get; set; }

    /// <summary>
    /// The 'T-Shirt' size of the field, which may affect its height and width.
    /// </summary>
    public PropertySize Size { get; set; }

    /// <summary>
    /// The description to display with the field when the user selected the description affordance.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Determines if the description icon is shown when a description is provided.
    /// </summary>
    public bool ShowDescription { get; set; }

    /// <summary>
    /// The label to display with the field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Determines if the label is shown when a label is provided.
    /// </summary>
    public bool ShowLabel { get; set; }
}

public interface IField<T>
    : IField
{
    public T Value { get; set; }

    public EventCallback<T> ValueChanged { get; set; }
}
