namespace ExtraDry.Blazor;

/// <summary>
/// A flexi alternative to a select control.  Creates a semantic HTML control
/// with extended capabilities for generating single and multiple select 
/// controls on mobile and desktop platforms.  Includes list management and
/// filtering.
/// </summary>
/// <typeparam name="TItem">The type for items in the select list.</typeparam>
public partial class ComboBox<TItem> : ComponentBase, IExtraDryComponent where TItem : notnull {

    public string Id = $"Id{Guid.NewGuid()}";

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "select...";

    /// <inheritdoc cref="FlexiSelectForm{TItem}.Data"/>
    [Parameter]
    public IEnumerable<TItem> Data { get; set;} = new List<TItem>();

    [Parameter]
    public IListItemViewModel<TItem>? ViewModel { get; set; }

    /// <inheritdoc cref="MiniDialog.OnSubmit" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnSubmit { get; set; }
  
    /// <inheritdoc cref="MiniDialog.OnCancel" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnCancel { get; set; }

    /// <inheritdoc cref="MiniDialog.AnimationDuration" />
    [Parameter]
    public int AnimationDuration { get; set; } = 100;

    /// <inheritdoc cref="FlexiSelectForm{TItem}.Value" />
    [Parameter]
    public TItem? Value { get; set; }

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ValueChanged" />
    [Parameter]
    public EventCallback<TItem?> ValueChanged { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    [Inject]
    protected ExtraDryJavascriptModule Javascript { get; set; } = null!;

    public async void DoValueChanged(TItem item) {
        Console.WriteLine(item);
        await ValueChanged.InvokeAsync(item);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if(Data.Any() && !Items.Any()) {
            Items = Data.Select(e => new ItemInfo(e, DisplayTitle(e))).ToList();
        }
        base.OnParametersSet();
    }

    private bool ShowOptions { get; set; }

    private int SelectedIndex { get; set; } = -1;

    private ItemInfo? SelectedItem => SelectedIndex < 0 ? null : Items[SelectedIndex];

    private List<ItemInfo> Items { get; set; } = new();

    private string ValueString => Value?.ToString() ?? "null";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "drop-down", CssClass);

    private Task DoClick(MouseEventArgs _)
    {
        ShowOptions = !ShowOptions;
        return Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(SelectedItem != null && ShowOptions) {
            await Javascript.InvokeVoidAsync("DropDown_ScrollIntoView", SelectedItem.Uuid.ToString());
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private string DisplayTitle(TItem item) =>
        ViewModel?.Title(item)
        ?? (item as IListItemViewModel)?.Title
        ?? item.ToString()
        ?? "unnamed";

    /// <summary>
    /// Handle keypresses
    /// </summary>
    private async Task DoKeyPress(KeyboardEventArgs args)
    {
        Console.WriteLine(args.Code);
        PreventDefault = false;
        var pageSize = 8; // 9 lines shown so page up/down should be one less so you have one line overlap for context.
        switch(args.Code) {
            case "Enter":
            case "NumpadEnter":
                await ConfirmAsync(SelectedItem);
                break;
            case "PageUp":
                SelectOffset(-pageSize);
                PreventDefault = true;
                break;
            case "PageDown":
                SelectOffset(+pageSize);
                PreventDefault = true;
                break;
            case "End":
                SelectOffset(-1, Data.Count());
                break;
            case "Home":
                SelectOffset(+1, -1);
                break;
            case "ArrowUp":
                SelectOffset(-1);
                PreventDefault = true;
                break;
            case "ArrowDown":
                SelectOffset(+1);
                PreventDefault = true;
                break;
            case "Escape":
                CancelInput();
                break;
            default:
                break;
        }
        Console.WriteLine(SelectedIndex.ToString());
    }

    private bool PreventDefault = false;

    private async Task ConfirmAsync(ItemInfo? selectedItem)
    {
        if(selectedItem != null && ShowOptions) {
            // Valid Item selected and want to lock it in
            ShowOptions = false;
            Filter = selectedItem.Title;
        }
        else if(Items.Count(e => e.Visible) == 1) {
            // Filter has left only a single item, select it and lock it in.
            SelectOffset(+1, -1);
            ShowOptions = false;
            Filter = SelectedItem?.Title ?? Filter;
        }
        else {
            ShowOptions = true;
        }
        if(SelectedItem != null) {
            Value = SelectedItem.Item;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private void CancelInput()
    {
        ShowOptions = false;
        if(Value == null) {
            SelectedIndex = -1;
            Filter = string.Empty;
        }
        else {
            SelectItem(Items.FindIndex(e => e.Item.Equals(Value)));
            Filter = SelectedItem?.Title ?? Filter;
        }
        ShouldRender();
    }

    private void SelectOffset(int offset, int start = int.MinValue)
    {
        var step = Math.Sign(offset);
        var count = Math.Abs(offset);
        var newIndex = start == int.MinValue ? SelectedIndex : start;
        if(newIndex == -1) {
            if(step > 0) {
                newIndex = 0;
            }
            else {
                newIndex = Items.Count - 1;
            }
        }
        else {
            for(int index = newIndex + step; count > 0 && index >= 0 && index < Items.Count; index += step) {
                if(Items[index].Visible) {
                    newIndex = index;
                    --count;
                }
            }
        }
        SelectItem(newIndex);
    }

    private void SelectItem(int index)
    {
        Console.WriteLine($"Index: {index}");
        if(SelectedIndex != index) {
            SelectedIndex = index;
            ShouldRender();
        }
    }

    private void ComputeFilter()
    {
        var showAll = string.IsNullOrWhiteSpace(Filter) || SelectedItem != null;
        foreach(var item in Items) {
            if(showAll || item.Title.Contains(Filter, StringComparison.CurrentCultureIgnoreCase)) {
                item.Visible = true;
            }
            else {
                item.Visible = false;
            }
        }
    }

    private string Filter {
        get => filter;
        set {
            Console.WriteLine($"New filter: {value}");
            filter = value;
            if(!filter.Equals(SelectedItem?.Title, StringComparison.CurrentCultureIgnoreCase)) {
                SelectedIndex = -1;
            }
            if(!string.IsNullOrWhiteSpace(filter) && SelectedItem == null) {
                ShowOptions = true;
            }
            ComputeFilter();
            ShouldRender();
        }
    }
    private string filter = string.Empty;


    public class ItemInfo {

        public ItemInfo(TItem source, string title) {
            Item = source;
            Title = title;
        }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        public TItem Item { get; set; }

        public string Title { get; set; }

        public bool Visible { get; set; } = true;

    }

}
