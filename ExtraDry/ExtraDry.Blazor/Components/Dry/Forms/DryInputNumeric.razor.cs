using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a numeric input field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputNumeric<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
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

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Event that is raised when the input is validated using internal rules. Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private string Icon => Property?.InputFormat?.Icon ?? "";

    private string Affordance => Property?.InputFormat?.Affordance ?? "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private string Value {
        get {
            return _Value;
        }
        set {
            HandleChange(value);
        }
    }

    private string InputTitle => Property?.FieldCaption ?? "";

    [Parameter]
    public string? Placeholder { get; set; }

    private string PlaceholderDisplay => Placeholder ?? Property?.Display?.Prompt ?? "";

    /// <summary>
    /// Because we are mutating the value that is displayed within the handle change (to strip or
    /// calculate values), we need to implement differently The OnChange functionality will not
    /// allow for this (there is a hack to assign the backing field to null, then sleep, then
    /// repopulate) so using the binding functionality is the recommended way
    /// https://github.com/dotnet/aspnetcore/issues/17099
    /// </summary>
    private void HandleChange(string newValue)
    {
        if(Property == null || Model == null) {
            return;
        }

        // In the future, if we are to allow basic calculations in numeric fields, this is where
        // that would go. Note that this will run synchronously in the setter of Value and
        // therefore needs to be quick.

        var value = Regex.Replace(newValue, @"[^\d.,]", "");

        if(!decimal.TryParse(value, CultureInfo.CurrentCulture, out var dec)) {
            dec = 0;
        }

        // Future enhancement: Allow for the consumer to provide the display format.
        if(Property.InputType == typeof(int)) {
            _Value = dec.ToString("#,#", CultureInfo.CurrentCulture);
            Property.SetValue(Model, int.Parse(value, CultureInfo.InvariantCulture));
        }
        else {
            _Value = dec.ToString("#,0.00", CultureInfo.CurrentCulture);
            Property.SetValue(Model, value);
        }
    }

    private async Task CallOnChange()
    {
        var task = OnChange?.InvokeAsync();
        if(task != null) {
            await task;
        }
    }

    private string _Value = "";

    /// <summary>
    /// Only allow digits, commas, periods and navigation keys. 
    /// </summary>
    private static string DisableInvalidCharacters => @"if(!(/[0-9.,]/.test(event.key) || event.key == 'Backspace' || event.key == 'Delete' || event.key == 'ArrowLeft' || event.key == 'ArrowRight' || event.key == 'Tab')) return false;";

}
