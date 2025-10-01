using ExtraDry.Blazor;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor.Components.Standard;

public interface IInputField
{
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Clash is between CSS and C#, more confusing to change it from the CSS name.")]
    public bool ReadOnly { get; set; }

    public string CssClass { get; set; }

    public string Icon { get; set; }

    public string Affordance { get; set; }

    public string Placeholder { get; set; }

    public PropertySize Size { get; set; }

    public string Description { get; set; }

    public bool ShowDescription { get; set; }

    public string Caption { get; set; }
}

public interface IInputField<T>
    : IInputField
{
    public T Value { get; set; }

    public EventCallback<T> ValueChanged { get; set; }
}
