using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A base class for input components that are used to edit a single property of a model. Contains
/// common properties and methods for all input components. In addition, ensure classes that
/// inherit from this class:
/// * Have an enclosing DIV for the input.
/// * Set the CssClass on the DIV with full semantic class names.
/// * Chain the unmatched attributes to the enclosing DIV
/// * Set single class names on the inner elements, e.g. 'value' for the input element.
/// </summary>
/// <typeparam name="T">The type of the property on the model the derived input handles.</typeparam>
public class DryFieldBase<T>
    : ComponentBase, IDryInput<T>, IExtraDryComponent
    where T : class
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public T Model { get; set; } = null!;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public PropertyDescription Property { get; set; } = null!;

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnInput { get; set; }

    /// <summary>
    /// Event that is raised when the input is validated using internal rules. Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }

    /// <summary>
    /// The icon to display next to the input field. If not set, the icon from the property's
    /// InputFormat is used. If that is not set, no icon is displayed.
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = "";

    /// <summary>
    /// The actual icon to display, resolved from the property's InputFormat or the Icon parameter.
    /// </summary>
    protected string ResolvedIcon => Icon == "" ? Property?.InputField?.Icon ?? "" : Icon;

    /// <summary>
    /// The description to display for the input field. If not set, the description from the property's
    /// Display is used.  If that is not set, no description is displayed.
    /// </summary>
    [Parameter]
    public string Description { get; set; } = "";

    protected string ResolvedDescription =>
        Description == "" ? Property?.Display?.Description ?? "" : Description;

    /// <summary>
    /// Determines if the input field is read-only. If not set, the value from the property's
    /// read-only setting is used. If that is not set, the field is editable.
    /// </summary>
    [Parameter]
    public bool? ReadOnly { get; set; }

    protected bool ResolvedReadOnly => ReadOnly /*?? Property.IsReadOnly */?? false;


    protected string ReadOnlyCss => ResolvedReadOnly ? "readonly" : "";

    /// <summary>
    /// Logger for DryInput controls, shares space with DryInput for consistency in logging
    /// messages between a component and its children.
    /// </summary>
    [Inject]
    protected ILogger<DryFieldBase<T>> Logger { get; set; } = null!;

    [Parameter]
    public string Placeholder { get; set; } = "";

    protected string ResolvedPlaceholder => Placeholder == "" ? Property?.Display?.Prompt ?? "" : Placeholder;

    /// <summary>
    /// The icon key for the affordance on the control. Overrides the default or the value in the
    /// property's InputFormat.
    /// </summary>
    [Parameter]
    public string Affordance { get; set; } = "";

    /// <summary>
    /// The maximum length of the input field. If not set, the value from the property's
    /// StringLength or MaxLength attribute is used. If that is not set, defaults to 1,000,000.
    /// </summary>
    [Parameter]
    public int? MaxLength { get; set; }

    protected int ResolvedMaxLength => MaxLength ?? Property.FieldLength ?? 1_000_000;

    protected string ResolvedAffordance =>
        Affordance == "" ? Property?.InputField?.Affordance ?? "" : Affordance;

    /// <summary>
    /// The title for the input field. If not set, the field caption from the property is used.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = "";

    /// <summary>
    /// The title for the control, resolved from the component parameter, or the InputFormat on the
    /// property.
    /// </summary>
    protected string ResolvedLabel =>
        Label == ""
        ? Property?.FieldCaption ?? ""
        : Label;

    /// <summary>
    /// Retrieve the event args by calling <see cref="ValidateProperty" />.
    /// </summary>
    protected async Task InvokeOnValidationAsync(ValidationEventArgs args)
    {
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
    /// <returns>True if property is valid</returns>
    protected ValidationEventArgs ValidateProperty()
    {
        var args = new ValidationEventArgs {
            IsValid = true,
            MemberName = Property!.Property.Name,
            Message = "",
        };
        var validator = new DataValidator();
        validator.ValidateProperties(Model!, Property!.Property.Name);
        args.IsValid = validator.Errors.Count == 0;
        if(!args.IsValid) {
            args.Message = validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Value is not valid";
        }
        return args;
    }
}
