using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Components.Internal;

public partial class DryInputSingleSelect<T> : ComponentBase, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public T? Model { get; set; }

    [Parameter]
    public PropertyDescription? Property { get; set; }

    [Parameter]
    public List<object>? Values { get; set; } 

    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected override void OnParametersSet()
    {
        if(Model != null) {
            SelectedValue = Property?.GetValue(Model);
        }
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private async Task SelectOption(ChangeEventArgs args)
    {
        if(Values == null || !int.TryParse(args.Value as string, out var index)) {
            return; // selected blank line
        }
        SelectedValue = Values[index];
        if(Model != null) {
            Property?.SetValue(Model, SelectedValue);
            await InvokeOnChange(args);
        }
    }

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    private bool ReadOnly => EditMode == EditMode.ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private object? SelectedValue { get; set; }

}
