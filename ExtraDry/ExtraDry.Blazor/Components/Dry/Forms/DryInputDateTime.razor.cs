namespace ExtraDry.Blazor.Forms;

public partial class DryInputDateTime<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
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

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        var prop = Property.GetValue(Model);
        Value = prop is DateTime ? ((DateTime)prop).ToLocalTime().ToString("yyyy-MM-ddThh:mm", CultureInfo.InvariantCulture) : "";
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        if(DateTime.TryParse(value?.ToString(), out var datetime)) {
            Property.SetValue(Model, datetime.ToUniversalTime());
        }

        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    [Parameter]
    public bool ReadOnly { get; set; }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = string.Empty;
}
