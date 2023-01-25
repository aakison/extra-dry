using Microsoft.Extensions.Logging.Abstractions;

namespace ExtraDry.Blazor;

/// <summary>
/// A flexi alternative to a select control.  Creates a semantic HTML control
/// with extended capabilities for generating single and multiple select 
/// controls on mobile and desktop platforms.  Includes list management and
/// filtering.
/// </summary>
/// <typeparam name="TItem">The type for items in the select list.</typeparam>
public partial class ComboBox<TItem> : ComponentBase, IExtraDryComponent where TItem : class {

    /// <summary>
    /// Provides a name for the embedded input tag, not generally needed for Blazor applications.
    /// However, might be useful for Blazor/ASP.NET interop.
    /// </summary>
    [Parameter]
    public string Name { get; set; } = null!;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "select...";

    /// <inheritdoc cref="IComments{TItem}.Items"/>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set;}

    /// <inheritdoc cref="IComments{TItem}.ItemsSource" />
    [Parameter]
    public IListService<TItem>? ItemsSource { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ViewModel" />
    [Parameter]
    public IListItemViewModel<TItem>? ViewModel { get; set; }

    /// <inheritdoc cref="IComments.Icon" />
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Determines the sort order of items in the control.  Defaults to sorting by title as this
    /// is best understood by users.  However, this can be turned off if the incoming collection
    /// has a implicitly understood sort order (e.g. day of week).
    /// </summary>
    [Parameter]
    public ComboBoxSort Sort { get; set; } = ComboBoxSort.Title;

    /// <summary>
    /// Indicates if the items in the list are grouped.  To turn grouping 'on', the items must be
    /// provided using `Items` (not ItemsSource), a `ViewModel` must be provided which 
    /// implements `IGroupingViewModel`.
    /// </summary>
    [Parameter]
    public ComboBoxGrouping Grouping { get; set; } = ComboBoxGrouping.Auto;

    /// <inheritdoc cref="IComments{TItem}.Value" />
    [Parameter]
    public TItem? Value { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ValueChanged" />
    [Parameter]
    public EventCallback<TItem?> ValueChanged { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    [Inject]
    protected ExtraDryJavascriptModule Javascript { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        Console.WriteLine("OnParametersSet()");
        Id ??= $"combo_{GetHashCode()}";
        Name ??= $"combo_{typeof(TItem).Name}";
        AssertItemsMutualExclusivity();

        Grouper = (ViewModel as IGroupingViewModel<TItem>) ?? new NullGroupingViewModel();

        ShowGrouping = Grouping switch {
            ComboBoxGrouping.Off => false,
            _ => ViewModel is IGroupingViewModel<TItem> && ItemsSource == null,
        };

        TryPopulateFromItems();
        var tokenSource = new CancellationTokenSource();
        await TryPopulateFromItemsSourceAsync(string.Empty, tokenSource.Token);

        FilteredItems = SortedItems.ToList();
        await base.OnParametersSetAsync();
    }

    private void TryPopulateFromItems()
    {
        if(Items == null) {
            return;
        }
        if(ShowGrouping) {
            if(Sort == ComboBoxSort.Title) {
                SortedItems = Items.OrderBy(e => DisplayItemGroupSort(e)).ThenBy(e => DisplayItemTitle(e)).ToList();
            }
            else {
                SortedItems = Items.OrderBy(e => DisplayItemGroupSort(e)).ToList();
            }
        }
        else {
            if(Sort == ComboBoxSort.Title) {
                SortedItems = Items.OrderBy(e => DisplayItemTitle(e)).ToList();
            }
            else {
                SortedItems = Items.ToList();
            }
        }
    }

    private async Task TryPopulateFromItemsSourceAsync(string filter, CancellationToken cancellationToken)
    {
        if(ItemsSource == null) {
            return;
        }
        cancellationToken.ThrowIfCancellationRequested();
        ShowProgress = true;
        var items = await ItemsSource.GetItemsAsync(filter, null, null, null, null, cancellationToken);
        ShowProgress = false;
        SortedItems = items.Items.OrderBy(e => DisplayItemGroupSort(e)).ToList();
    }

    /// <summary>
    /// An Id for the component attached to the outermost Div and used to help the associated 
    /// Javascript identify and support this component when many might be on the same page.
    /// </summary>
    private string Id { get; set; } = null!;

    /// <summary>
    /// The currently 'selected' item which varies from the current Value. The selected item can
    /// change as the user is scrolling through the list of options, but doesn't become the `Value`
    /// until the user confirms the settings. 
    /// </summary>
    private TItem? SelectedItem { get; set; }

    private List<TItem> SortedItems { get; set; } = new();

    private List<TItem> FilteredItems { get; set; } = new();

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "drop-down", CssClass);

    /// <summary>
    /// Resolves if grouping should be used based on all parameters for the component.
    /// </summary>
    /// <remarks>Set in `OnParametersSetAsync`</remarks>
    private bool ShowGrouping { get; set; }

    /// <summary>
    /// Used to show a loading progress bar when fetching data from the `ItemsSource`.
    /// </summary>
    /// <remarks>Set and reset before and after async calls to `ItemsSource`.</remarks>
    private bool ShowProgress { get; set; }

    /// <summary>
    /// Indicates if the options drop-down list is currently being shown.
    /// </summary>
    private bool ShowOptions { get; set; }

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

    /// <summary>
    /// Determines a unique Id for the item to enable JavaScript UI manipulation.
    /// </summary>
    /// <remarks>ID should be unique for each item and retained between renderings.</remarks>
    private string DisplayItemID(TItem? item) => 
        $"{Id}_item_{item?.GetHashCode()}";

    /// <summary>
    /// Determines the title to display for the item using multiple fallback mechanisms such as 
    /// the ViewModel.
    /// </summary>
    private string DisplayItemTitle(TItem? item) =>
        item == null ? "null" : null
        ?? ViewModel?.Title(item)
        ?? (item as IListItemViewModel)?.Title
        ?? item.ToString()
        ?? "unnamed";

    /// <summary>
    /// The display name for a Group that is optionally related to the item.
    /// </summary>
    private string DisplayItemGroup(TItem? item) => 
        item == null ? string.Empty : Grouper.Group(item);

    /// <summary>
    /// A string that determines the sort order of group items, typically the same as the Group
    /// name but can be changed by component consumers.
    /// </summary>
    private string DisplayItemGroupSort(TItem? item) =>
        item == null ? string.Empty : Grouper.GroupSort(item);

    /// <summary>
    /// The ViewModel that provides grouping information, always exists as either one passed in by
    /// the Consumer or a default one that is provided.  Set in `OnParametersSetAsync`.
    /// </summary>
    private IGroupingViewModel<TItem> Grouper { get; set; } = null!; 

    /// <summary>
    /// Handle keypresses for navigation, preventing default when they're handled in the component
    /// and the key shouldn't be bubbled up to the page.
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

    /// <summary>
    /// Confirms the explicit or implicit `SelectedItem` and makes it the current `Value`.
    /// Handles Enter key and Double Clicks.
    /// </summary>
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
            Filter = DisplayItemTitle(SelectedItem);
        }
        else {
            ShowOptions = true;
        }
        if(!(Value?.Equals(SelectedItem) ?? SelectedItem == null)) {
            Value = SelectedItem;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    /// <summary>
    /// Cancels the current input, reverting to the previous `Value` and resetting 
    /// the  `SelectedItem`.
    /// </summary>
    private void CancelInput()
    {
        Console.WriteLine("CancelInput");
        ShowOptions = false;
        if(Value == null) {
            SelectedItem = default;
            Filter = string.Empty;
        }
        else {
            SelectItemIndex(FilteredItems.FindIndex(e => e?.Equals(Value) ?? false));
            Filter = SelectedItem == null ? Filter : DisplayItemTitle(SelectedItem);
        }
        ShouldRender();
    }

    /// <summary>
    /// Selects a new item based on the position of the currently selected item.  Enables page up, 
    /// page down, item up and item down commands.
    /// </summary>
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
            var SelectedIndex = SelectedItem == null ? -1 : FilteredItems.IndexOf(SelectedItem);
            var newIndex = SelectedIndex + offset;
            newIndex = Math.Clamp(newIndex, 0, FilteredItems.Count - 1);
            SelectItemIndex(newIndex);
        }
    }

    /// <summary>
    /// Given an index (relative to the `FilteredItems`), checks bounds and selects item.
    /// </summary>
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

    /// <summary>
    /// The current filter displayed the user.  Will be the entire title of the SelectedItem once 
    /// selection made.
    /// </summary>
    private string Filter { get; set; } = string.Empty;

    /// <summary>
    /// Process set values against the filter asynchronously so that search can be performed.
    /// </summary>
    /// <remarks>
    /// This is done using one of the Blazor "anti-patterns" as binding doesn't work with Blazor 6 
    /// and the fix designed for .NET 7 "missed the RTM window" a couple of times.  Eventually this
    /// will be better to use binding with `@bind:after="ProcessFilter"` but that's not ready yet.
    /// </remarks>
    private async Task DoFilterInput(ChangeEventArgs args)
    {
        Console.WriteLine($"DoFilterInput({args.Value})");
        Filter = args.Value?.ToString() ?? string.Empty;
        if(!Filter.Equals(DisplayItemTitle(SelectedItem), StringComparison.CurrentCultureIgnoreCase)) {
            SelectedItem = default;
        }
        if(!string.IsNullOrWhiteSpace(Filter) && SelectedItem == null) {
            ShowOptions = true;
        }
        try {
            cancelSource?.Cancel();
            using(cancelSource = new CancellationTokenSource()) {
                await ComputeFilter(cancelSource.Token);
            }
        }
        catch(OperationCanceledException) {
            Console.WriteLine("Operation Cancelled");
        }
        finally {
            cancelSource = null;
        }
        ShouldRender();
    }

    /// <summary>
    /// Very important to cancel requests from the client to the server if the result is no longer
    /// needed, such as when typing another letter before the previous result returns.  When null,
    /// there is no pending request, if not null and a new request comes in then need to cancel 
    /// before continuing.  Used by `DoFilterInput` when `ItemsSource` is not null.
    /// </summary>
    private CancellationTokenSource? cancelSource = null;

    private async Task ComputeFilter(CancellationToken cancellationToken)
    {
        Console.WriteLine("ComputeFilter()");
        var showAll = string.IsNullOrWhiteSpace(Filter) || SelectedItem != null;
        if(showAll) {
            if(ItemsSource != null) {
                cancellationToken.ThrowIfCancellationRequested();
                await TryPopulateFromItemsSourceAsync("", cancellationToken);
            }
            FilteredItems = SortedItems.ToList();
        }
        else {
            if(ItemsSource != null) {
                cancellationToken.ThrowIfCancellationRequested();
                await TryPopulateFromItemsSourceAsync(Filter, cancellationToken);
            }
            FilteredItems = SortedItems
                .Where(e => DisplayItemTitle(e).Contains(Filter, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }

    private void AssertItemsMutualExclusivity()
    {
        if(Items != null && ItemsSource != null) {
            throw new DryException("Only one of `Items` and `ItemsSource` is allowed to be set");
        }
        if(Items == null && ItemsSource == null) {
            throw new DryException("One of `Items` or `ItemsService` must set");
        }
    }

    /// <summary>
    /// When IGroupingViewModel provided, provide a default option that turns grouping off.
    /// </summary>
    protected class NullGroupingViewModel : IGroupingViewModel<TItem> {
        public string Group(TItem item) => string.Empty;

        public string GroupSort(TItem item) => string.Empty;
    }

}

/// <summary>
/// Defines the sort options for the `ComboBox`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComboBoxSort
{
    /// <summary>
    /// No sorting is applied, the order items is presented to the component is retained.
    /// </summary>
    None,

    /// <summary>
    /// Sorting is done by the title alphabetically (default).  
    /// </summary>
    Title,

}

/// <summary>
/// Defines the grouping options for the `ComboBox`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComboBoxGrouping
{
    /// <summary>
    /// No groupings are shown.
    /// </summary>
    Off,

    /// <summary>
    /// Grouping are shown if the `ViewModel` implements IGroupingViewModel`T (default).
    /// </summary>
    Auto,
}
