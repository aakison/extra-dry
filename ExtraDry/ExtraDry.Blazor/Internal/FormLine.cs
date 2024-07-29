using System.Collections.ObjectModel;

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Indicates a logical sub-group of properties that can occur on a single line in a form.
/// Some `FormLine`s will contain a single property or a single header.
/// Others will contain several short properties that can be stacked together.
/// </summary>
internal class FormLine(
    object model)
{
    public Collection<PropertyDescription> FormProperties { get; } = new();

    public Collection<FormCommand> Commands { get; } = new();

    public object Model { get; set; } = model;

}
