using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

public partial class DryInputMultipleSelect<T> : ComponentBase, IDryInput<T>, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public T? Model { get; set; }

    [Parameter]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc cref="DryInputSingleSelect{T}.Values" />
    [Parameter]
    public List<object>? Values { get; set; } 

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    /// <inheritdoc />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        InitializeList();
        InitializeOptions();
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private void InitializeList()
    {
        PropertyList = Property?.GetValue(Model) as IList;
    }

    private void InitializeOptions()
    {
        if(Values == null) {
            AllOptions.Clear();
            return;
        }
        int index = 100;
        foreach(var value in Values) {
            var key = $"{index++}";
            var selected = PropertyList?.Contains(value) ?? false;
            AllOptions.Add(key, new OptionInfo(key, value?.ToString() ?? "-empty-", value) { 
                Selected = selected,
            });
        }
    }

    private async Task SelectOption(ChangeEventArgs args)
    {
        var key = args.Value as string;
        if(string.IsNullOrWhiteSpace(key)) {
            return; // selected blank line
        }
        var option = AllOptions[key];
        option.Selected = true;
        if(option.Value != null) {
            Property?.AddValue(Model!, option.Value);
            await InvokeOnChange(args);
            await SelectBlankRow();
        }
    }

    private async Task SelectBlankRow()
    {
        // HACK: Want to re-select the blank row
        BlankSelected = false;
        await Task.Delay(16); // one frame, let selected be removed then re-add to force DOM update.
        BlankSelected = true;
    }

    private async Task DeselectOption(string key)
    {
        var option = AllOptions[key];
        option.Selected = false;
        if(option.Value != null) {
            Property?.RemoveValue(Model!, option.Value);
        }
        await InvokeOnChange(new ChangeEventArgs { Value = key });
    }

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }

    private IList? PropertyList { get; set; }

    private bool BlankSelected { get; set; } = true;

    private Dictionary<string, OptionInfo> AllOptions { get; } = new Dictionary<string, OptionInfo>();

    private IEnumerable<OptionInfo> SelectedOptions => AllOptions.Values.Where(e => e.Selected == true);

    private IEnumerable<OptionInfo> AvailableOptions => AllOptions.Values.Where(e => e.Selected == false);

    private bool ReadOnly => EditMode == EditMode.ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

}
