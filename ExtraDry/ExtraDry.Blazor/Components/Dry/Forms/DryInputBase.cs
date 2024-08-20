using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A base class for input components that are used to edit a single property of a model.
/// Contains common properties and methods for all input components.
/// In addition, ensure classes that inherit from this class:
///  * Have an enclosing DIV for the input.
///  * Set the CssClass on the DIV with full semantic class names.
///  * Chain the unmatched attributes to the enclosing DIV
///  * Set single class names on the inner elements, e.g. 'value' for the input element.
/// </summary>
/// <typeparam name="T">The type of the property on the model the derived input handles.</typeparam>
public class DryInputBase<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <summary>
    /// Event that is raised when the input is validated using internal rules. Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }

    protected async Task InvokeOnValidationAsync(bool valid)
    {
        var args = new ValidationEventArgs {
            IsValid = valid,
            MemberName = Property!.PropertyType.Name,
            Message = valid ? string.Empty : $"Value is not a valid {Property.PropertyType.Name}",
        };
        await OnValidation.InvokeAsync(args);
    }

    protected async Task InvokeOnChangeAsync(object? value)
    {
        var args = new ChangeEventArgs {
            Value = value,
        };
        await OnChange.InvokeAsync(args);
    }

    /// <summary>
    /// Checks the property against the data annotations and returns true if the property is valid.
    /// </summary>
    /// <returns>true if property is valid</returns>
    protected bool ValidateProperty()
    {
        bool valid = true;
        var validator = new DataValidator();
        validator.ValidateProperties(Model!, Property!.Property.Name);
        valid = validator.Errors.Count == 0;
        return valid;
    }

}
