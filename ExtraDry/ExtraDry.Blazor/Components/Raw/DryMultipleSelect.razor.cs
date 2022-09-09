using ExtraDry.Blazor.Components.Internal;
using System.Collections;
using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Components.Raw;

public partial class DryMultipleSelect<T> : ComponentBase {

    [Parameter, EditorRequired]
    public T Model { get; set; } = default!;

    [Parameter]
    public PropertyDescription? Property { get; set; }

    [Parameter]
    public List<object>? Values { get; set; } 

    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    [Parameter]
    public string? CssClass { get; set; }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        InitializeList();
        InitializeOptions();
        ListCollection();
    }

    private void ListCollection()
    {
        Logger.LogWarning("List contents:");
        if(Property?.GetValue(Model) is IList list) {
            foreach(var item in list) {
                Logger.LogWarning("List Item {item}", item);
            }
        }
        else {
            Logger.LogWarning("List is empty");
        }
    }

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
            var key = index++.ToString();
            var selected = PropertyList?.Contains(value) ?? false;
            AllOptions.Add(key, new OptionInfo(key, value?.ToString() ?? "-empty-", value) { 
                Selected = selected,
            });
        }
        Logger.LogDebug("DryMultiSelect initialized with {Count} values", Values?.Count);
    }

    private async Task SelectOption(ChangeEventArgs args)
    {
        Logger.LogDebug("DryMultiSelect Add Option by Key '{Value}'", args.Value);
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
            ListCollection();
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
        Logger.LogDebug("DryMultiSelect Remove Option by Key '{key}'", key);
        var option = AllOptions[key];
        option.Selected = false;
        if(option.Value != null) {
            Property?.RemoveValue(Model!, option.Value);
        }
        await InvokeOnChange(new ChangeEventArgs { Value = key });
        ListCollection();
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
}
