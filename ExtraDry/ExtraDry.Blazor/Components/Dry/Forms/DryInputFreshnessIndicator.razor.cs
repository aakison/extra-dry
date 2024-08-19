using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a freshness indicator field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputFreshnessIndicator<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
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
    public EditMode EditMode { get; set; } = EditMode.ReadOnly;

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

    protected override async Task OnParametersSetAsync()
    {
        if(Model == null || Property == null) {
            return;
        }

        var property = Property?.GetValue(Model);

        if(property == null) {
            return;
        }

        var userGuidString = ((UserTimestamp)property).User;
        var timeStamp = ((UserTimestamp)property).Timestamp;

        //if(Guid.TryParse(userGuidString, out Guid guid)) {
        //    var user = await userService.RetrieveAsync(userGuidString);
        //}

        Value = $"Last updated by {userGuidString} {DataConverter.DateToRelativeTime(timeStamp)}";
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    //[Inject]
    //private CrudService<T> userService { get; set; }

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

        _Value = newValue;
        //Property.SetValue(Model, newValue);
    }

    private async Task CallOnChange()
    {
        var task = OnChange?.InvokeAsync();
        if(task != null) {
            await task;
        }
    }

    private string _Value = "";
}
