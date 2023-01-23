namespace ExtraDry.Blazor;

/// <summary>
/// A flexi alternative to a select control.  Creates a semantic HTML control
/// with extended capabilities for generating single and multiple select 
/// controls on mobile and desktop platforms.  Includes list management and
/// filtering.
/// </summary>
/// <typeparam name="TItem">The type for items in the select list.</typeparam>
public partial class ComboBox<TItem> : ComponentBase, IExtraDryComponent where TItem : notnull {

    [Parameter]
    public string Id { get; set; } = null!;

    [Parameter]
    public string Name { get; set; } = null!;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "select...";

    /// <inheritdoc cref="FlexiSelectForm{TItem}.Data"/>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set;}

    [Parameter]
    public IListService<TItem>? ItemsService { get; set; }

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
        Id ??= $"combo_{GetHashCode()}";
        Name ??= $"combo_{typeof(TItem).Name}";
        AssertItemsMutualExclusivity();
        
        if(Items.Any() && !FilteredItems.Any()) {
            FilteredItems = Items.ToList();
        }
        base.OnParametersSet();
    }

    private bool ShowOptions { get; set; }

    private TItem? SelectedItem { get; set; }

    private int SelectedIndex => FilteredItems.IndexOf(SelectedItem);

    private List<TItem> FilteredItems { get; set; } = new();

    private string ValueString => Value?.ToString() ?? "null";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "drop-down", CssClass);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(SelectedItem != null && ShowOptions) {
            await Javascript.InvokeVoidAsync("DropDown_ScrollIntoView", DisplayItemID(SelectedItem));
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected Task DoFocusOut(FocusEventArgs _)
    {
        CancelInput();
        return Task.CompletedTask;
    }

    protected Task DoClick(MouseEventArgs _)
    {
        ShowOptions = !ShowOptions;
        return Task.CompletedTask;
    }

    private string DisplayItemID(TItem? item) => 
        $"{Id}_item_{item?.GetHashCode()}";

    private string DisplayItemTitle(TItem? item) =>
        item == null ? "null" : null
        ?? ViewModel?.Title(item)
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
        if(args.Code == "Enter" || args.Code == "NumpadEnter") {
            await ConfirmInputAsync(SelectedItem);
        }
        if(ShowOptions) {
            switch(args.Code) {
                case "PageUp":
                    SelectItemOffset(-pageSize);
                    PreventDefault = true;
                    break;
                case "PageDown":
                    SelectItemOffset(+pageSize);
                    PreventDefault = true;
                    break;
                case "ArrowUp":
                    SelectItemOffset(-1);
                    PreventDefault = true;
                    break;
                case "ArrowDown":
                    SelectItemOffset(+1);
                    PreventDefault = true;
                    break;
                case "Escape":
                case "Tab":
                    CancelInput();
                    break;
                default:
                    break;
            }
        }
        Console.WriteLine(DisplayItemTitle(SelectedItem));
    }

    private bool PreventDefault = false;

    private async Task ConfirmInputAsync(TItem? selectedItem)
    {
        if(selectedItem != null && ShowOptions) {
            // Valid Item selected and want to lock it in
            ShowOptions = false;
            Filter = DisplayItemTitle(selectedItem);
        }
        else if(FilteredItems.Count == 1) {
            // Filter has left only a single item, select it and lock it in.
            SelectItemIndex(0);
            ShowOptions = false;
            Filter = selectedItem == null ? Filter : DisplayItemTitle(selectedItem);
        }
        else {
            ShowOptions = true;
        }
        if(!(Value?.Equals(selectedItem) ?? SelectedItem == null)) {
            Value = selectedItem;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private void CancelInput()
    {
        Console.WriteLine("CancelInput");
        ShowOptions = false;
        if(Value == null) {
            SelectedItem = default;
            Filter = string.Empty;
        }
        else {
            SelectItemIndex(FilteredItems.FindIndex(e => e.Equals(Value)));
            Filter = SelectedItem == null ? Filter : DisplayItemTitle(SelectedItem);
        }
        ShouldRender();
    }

    private void SelectItemOffset(int offset)
    {
        Console.WriteLine($"SelectOffset({offset})");
        if(SelectedItem == null) {
            if(offset > 0) {
                SelectItemIndex(0);
            }
            else {
                SelectItemIndex(FilteredItems.Count - 1);
            }
        }
        else {
            var newIndex = SelectedIndex + offset;
            SelectItemIndex(newIndex);
        }
    }

    private void SelectItemIndex(int index)
    {
        Console.WriteLine($"SelectIndex({index})");
        if(index < 0 || index >= FilteredItems.Count) {
            SelectedItem = default;
        }
        else {
            SelectedItem = FilteredItems[index];
        }
        ShouldRender();
    }

    private void ComputeFilter(CancellationToken cancellationToken)
    {
        Console.WriteLine("ComputeFilter()");
        var showAll = string.IsNullOrWhiteSpace(Filter) || SelectedItem != null;
        if(showAll) {
            if(Items != null) {
                FilteredItems = Items.ToList();
            }
            else {
                throw new NotImplementedException();
                //var results = await ItemsService!.GetItemsAsync(cancellationToken);
                //if(results.Items.Count() != results.TotalItemCount) {
                //    throw new NotImplementedException("Add in text to show user how much they are seeing.");
                //}
            }
        }
        else {
            if(Items != null) {
                FilteredItems = Items
                    .Where(e => DisplayItemTitle(e).Contains(Filter, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }
            else {
                throw new NotImplementedException();
            }
        }
    }

    private string Filter {
        get => filter;
        set {
            Console.WriteLine($"New filter: {value}");
            filter = value;
            if(!filter.Equals(DisplayItemTitle(SelectedItem), StringComparison.CurrentCultureIgnoreCase)) {
                SelectedItem = default;
            }
            if(!string.IsNullOrWhiteSpace(filter) && SelectedItem == null) {
                ShowOptions = true;
            }
            var src = new CancellationTokenSource();
            ComputeFilter(src.Token);
            ShouldRender();
        }
    }
    private string filter = string.Empty;

    private void AssertItemsMutualExclusivity()
    {
        if(Items != null && ItemsService != null) {
            throw new DryException("Only one of `Items` and `ItemsService` is allowed to be set");
        }
        if(Items == null && ItemsService == null) {
            throw new DryException("One of `Items` or `ItemsService` must set");
        }
    }

    public class ItemInfo {

        public ItemInfo(TItem source, string title) {
            Item = source;
            Title = title;
        }

        public TItem Item { get; set; }

        public string Title { get; set; }

    }

}
