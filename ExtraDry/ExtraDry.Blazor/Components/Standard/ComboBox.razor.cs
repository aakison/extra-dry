namespace ExtraDry.Blazor;

/// <summary>
/// A flexi alternative to a select control. Creates a semantic HTML control with extended
/// capabilities for generating single and multiple select controls on mobile and desktop
/// platforms. Includes list management and filtering.
/// </summary>
/// <typeparam name="TItem">The type for items in the select list.</typeparam>
public partial class ComboBox<TItem> : ComponentBase, IExtraDryComponent, IDisposable where TItem : notnull
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IComments.Icon" />
    [Parameter]
    public string? Icon { get; set; }

    /// <inheritdoc cref="IComments{TItem}.Items" />
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ItemsSource" />
    [Parameter]
    public IListClient<TItem>? ItemsSource { get; set; }

    /// <summary>
    /// Provides a name for the embedded input tag, not generally needed for Blazor applications.
    /// However, might be useful for Blazor/ASP.NET interop.
    /// </summary>
    [Parameter]
    public string Name { get; set; } = null!;

    /// <inheritdoc cref="IComments.Placeholder" />
    [Parameter]
    public string Placeholder { get; set; } = "find...";

    /// <summary>
    /// Indicates if the items in the list are grouped. To turn grouping 'on', the items must be
    /// provided using `Items` (not ItemsSource). Use with `GroupFunc` to define the name of the
    /// group that each items should be in.
    /// </summary>
    [Parameter]
    public bool Group { get; set; } = false;

    /// <summary>
    /// A func that defines the name of the group that items are in when `Group` is `true`.
    /// </summary>
    [Parameter]
    public Func<TItem?, string>? GroupFunc { get; set; }

    /// <summary>
    /// A func that defines the order of groups, typically can be left blank and the `GroupFunc`
    /// will be used.
    /// </summary>
    [Parameter]
    public Func<TItem?, string>? GroupSortFunc { get; set; }

    /// <summary>
    /// Determines if sorting is applied to the items in the component. Defaults to sorting by
    /// title as this is best understood by users. However, this can be turned off if the incoming
    /// collection has a implicitly understood sort order (e.g. day of week). See also `SortFunc`.
    /// </summary>
    [Parameter]
    public bool Sort { get; set; }

    /// <summary>
    /// A func that defines the string that the items are sorted by when `Sort` is `true`. Defaults
    /// to sorting by the title of the item.
    /// </summary>
    [Parameter]
    public Func<TItem?, string>? SortFunc { get; set; }

    /// <inheritdoc cref="IComments{TItem}.Value" />
    [Parameter]
    public TItem? Value { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ValueChanged" />
    [Parameter]
    public EventCallback<TItem?> ValueChanged { get; set; }

    /// <inheritdoc cref="IComments{TItem}.ViewModel" />
    [Parameter]
    public IListItemViewModel<TItem>? ViewModel { get; set; }

    /// <summary>
    /// Template for a non-selectable line that indicates that only a subset of filtered results
    /// are available. Default "plus {0} more...";
    /// </summary>
    [Parameter]
    public string MoreItemsTemplate { get; set; } = "plus {0} more...";

    /// <summary>
    /// When set, renders the combo box with the list of items always rendered to the document. By
    /// default, the list is removed from the DOM for performance, but this is harder to style. Use
    /// of this property is only recommended during development and should not be set in
    /// production.
    /// </summary>
    [Parameter]
    public bool DebugCss { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Inject]
    protected ExtraDryJavascriptModule Javascript { get; set; } = null!;

    public ComboBox()
    {
        Id = $"combo_{GetHashCode()}";
        OptionsId = $"{Id}_options";
    }

    protected async Task DoButtonClick(MouseEventArgs _)
    {
        if(ShowOptions) {
            CancelInput();
            ShowOptions = false;
        }
        else {
            await LoadOptionsAsync();
        }
    }

    protected async Task DoItemClick(TItem item)
    {
        SelectedOption = item;
        await ConfirmInputAsync(item);
    }

    protected Task DoMouseDown(TItem item)
    {
        SelectedOption = item;
        return Task.CompletedTask;
    }

    /// <summary>
    /// When the DIV loses focus we want to Cancel Input. However, the onfocusout event will fire
    /// multiple times _within_ the DIV without it actually losing focus. E.g. when switching from
    /// the button to the input text or scrollbar. No elegant solution can be found, so when the
    /// DIV receives OnFocusOut, let all events complete and see if the DIV immediately got a
    /// onfocusin event. If it loses focus and gains focus both in the same event loop then it
    /// "hasn't actually lost focus" and we don't want to cancel.
    /// </summary>
    private bool haveActuallyLostFocus;

    /// <summary>
    /// Just to check if we got focus back and haven't actually lost focus.
    /// </summary>
    protected Task DoFocusIn(FocusEventArgs _)
    {
        haveActuallyLostFocus = false;
        return Task.CompletedTask;
    }

    /// <summary>
    /// If we've actually lost focus and if so, cancel the input and revert to the current Value.
    /// </summary>
    protected async Task DoFocusOut(FocusEventArgs _)
    {
        haveActuallyLostFocus = true;
        await Task.Yield(); // KLUDGE: process other events, but get right back in queue...
        if(haveActuallyLostFocus) {
            CancelInput();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        Name ??= $"combo_{typeof(TItem).Name}";
        AssertItemsMutualExclusivity();

        InternalItems.SetFuncs(DisplayItemGroupSort, DisplayItemSort, DisplayItemTitle);
        if(Items != null) {
            InternalItems.SetItems(Items, Group, Sort, DisplayFilter);
        }
        else {
            InternalItems.UseFilter = false;
        }

        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// After each render, ensure that the
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await ScrollIntoView(OptionsId);
        if(SelectedOption != null && ShowOptions) {
            var id = DisplayItemID(SelectedOption);
            if(Group && SelectedOption.Equals(InternalItems.FilteredItems.FirstOrDefault())) {
                id = DisplayFirstHeaderId;
            }
            if(MoreCount > 0 && SelectedOption.Equals(InternalItems.FilteredItems.LastOrDefault())) {
                id = DisplayMoreCaptionId;
            }
            await ScrollIntoView(id);
        }
        await base.OnAfterRenderAsync(firstRender);

        async Task ScrollIntoView(string id) => await Javascript.InvokeVoidAsync("DropDown_ScrollIntoView", id);
    }

    /// <summary>
    /// Very important to cancel requests from the client to the server if the result is no longer
    /// needed, such as when typing another letter before the previous result returns. When null,
    /// there is no pending request, if not null and a new request comes in then need to cancel
    /// before continuing. Used by `DoFilterInput` when `ItemsSource` is not null.
    /// </summary>
    private CancellationTokenSource? computeFilterCancellationSource;

    private bool PreventDefault;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "combo-box", CssClass);

    /// <summary>
    /// The current filter displayed the user. Will be the entire title of the SelectedOption once
    /// selection made. The actual filter might vary and is contained in `InternalItems`.
    /// </summary>
    private string DisplayFilter { get; set; } = string.Empty;

    /// <summary>
    /// An Id for the component attached to the outermost Div and used to help the associated
    /// Javascript identify and support this component when many might be on the same page.
    /// </summary>
    private string Id { get; set; }

    /// <summary>
    /// An Id for the options dropdown when its visible, needed to scroll the window into view.
    /// </summary>
    private string OptionsId { get; set; }

    /// <summary>
    /// The currently 'selected' item which varies from the current Value. The selected item can
    /// change as the user is scrolling through the list of options, but doesn't become the `Value`
    /// until the user confirms the settings.
    /// </summary>
    private TItem? SelectedOption { get; set; }

    /// <summary>
    /// Indicates if the options drop-down list is currently being shown.
    /// </summary>
    private bool ShowOptions { get; set; }

    /// <summary>
    /// Used to show a loading progress bar when fetching data from the `ItemsSource`.
    /// </summary>
    /// <remarks>Set and reset before and after async calls to `ItemsSource`.</remarks>
    private bool ShowProgress { get; set; }

    /// <summary>
    /// Used to show that there are more results than are currently in the list.
    /// </summary>
    private int MoreCount { get; set; }

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
    /// Cancels the current input, reverting to the previous `Value` and resetting the
    /// `SelectedOption`.
    /// </summary>
    private void CancelInput()
    {
        Assert(ShowOptions == true, "CancelInput is wasteful if the options aren't shown.");
        ShowOptions = false;
        if(Value == null) {
            SelectedOption = default;
            DisplayFilter = string.Empty;
        }
        else {
            SelectedOption = Value;
            DisplayFilter = DisplayItemTitle(Value);
        }
        ShouldRender();
    }

    private async Task RetrieveFilteredFromItemsSource()
    {
        Assert(ShowOptions == true, "RetrieveFilteredFromItemsSource is wasteful if the options aren't shown.");
        Assert(ItemsSource != null, "RetrieveFilteredFromItemsSource is designed for remote collections.");
        if(ItemsSource == null) {
            return;
        }
        try {
            var showAll = string.IsNullOrWhiteSpace(DisplayFilter) || SelectedOption != null;
            if(computeFilterCancellationSource != null) {
                computeFilterCancellationSource.Cancel();
                await Task.Yield(); // KLUDGE: From .NET6 to .NET7 need to let some event through to keep this working...
            }
            ShowProgress = true;
            using(computeFilterCancellationSource = new CancellationTokenSource()) {
                var cancellationToken = computeFilterCancellationSource.Token;
                var filter = showAll ? string.Empty : DisplayFilter;
                cancellationToken.ThrowIfCancellationRequested();
                var items = await ItemsSource.GetItemsAsync(new Query { Filter = filter }, cancellationToken);
                InternalItems.SetItems(items.Items, Group, Sort, filter);
                MoreCount = items.Total - items.Count;
            }
            ShowProgress = false;
        }
        catch(OperationCanceledException) {
            Console.WriteLine("Operation Cancelled");
        }
        finally {
            computeFilterCancellationSource = null;
        }
    }

    /// <summary>
    /// Confirms the explicit or implicit `SelectedOption` and makes it the current `Value`.
    /// Handles Enter key and Double Clicks.
    /// </summary>
    private async Task ConfirmInputAsync(TItem? selectedItem)
    {
        Console.WriteLine("ConfirmInputAsync"); 
        Assert(ShowOptions == true, "ConfirmInputAsync expects that the options are shown.");
        if(selectedItem != null) {
            // Valid Item selected and want to lock it in
            ShowOptions = false;
            DisplayFilter = DisplayItemTitle(selectedItem);
        }
        else if(InternalItems.FilteredItems.Count == 1) {
            // Filter has left only a single item, select it and lock it in.
            SelectOptionIndex(0);
            ShowOptions = false;
            DisplayFilter = DisplayItemTitle(SelectedOption);
        }
        var hasChanged = Value is null ? SelectedOption is not null : !Value.Equals(SelectedOption);
        if(hasChanged) {
            Value = SelectedOption;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    private void Assert(bool value, string message)
    {
        if(value == false) {
            Logger.LogConsoleWarning($"{Id} ERROR - {message}");
        }
    }

    /// <summary>
    /// The display name for a Group that is related to the item.
    /// </summary>
    private string DisplayItemGroup(TItem? item)
    {
        return GroupFunc?.Invoke(item)
        ?? DisplayItemTitle(item)[0..1].ToUpper(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// A string that determines the sort order of group items, typically the same as the Group
    /// name but can be changed by component consumers.
    /// </summary>
    private string DisplayItemGroupSort(TItem? item)
    {
        return GroupSortFunc?.Invoke(item)
        ?? DisplayItemGroup(item);
    }

    /// <summary>
    /// The sortable name for the item, can be used to sort by something other than title.
    /// </summary>
    private string DisplayItemSort(TItem? item)
    {
        return SortFunc?.Invoke(item)
        ?? DisplayItemTitle(item);
    }

    /// <summary>
    /// Determines a unique Id for the item to enable JavaScript UI manipulation.
    /// </summary>
    /// <remarks>ID should be unique for each item and retained between renderings.</remarks>
    private string DisplayItemID(TItem? item)
    {
        return $"{Id}_item_{item?.GetHashCode()}";
    }

    /// <summary>
    /// Determines the title to display for the item using multiple fallback mechanisms such as the
    /// ViewModel.
    /// </summary>
    private string DisplayItemTitle(TItem? item)
    {
        return item == null ? "null" : null
        ?? ViewModel?.Title(item)
        ?? (item as IListItemViewModel)?.Title
        ?? item.ToString()
        ?? "unnamed";
    }

    /// <summary>
    /// Id for the first header when headers are shown so auto-scoll can target it.
    /// </summary>
    private string DisplayFirstHeaderId => $"{Id}_header";

    /// <summary>
    /// Id for the more footer when more is available so auto-scoll can target it.
    /// </summary>
    private string DisplayMoreCaptionId => $"{Id}_more";

    /// <summary>
    /// The formatted display text for showing that more items are available.
    /// </summary>
    private string DisplayMoreCaption => string.Format(CultureInfo.CurrentCulture, MoreItemsTemplate, MoreCount);

    /// <summary>
    /// When first opening the options set everything up for user-input.
    /// </summary>
    private async Task LoadOptionsAsync()
    {
        ShowOptions = true;
        InternalItems.SetFilter(string.Empty);
        if(SelectedOption != null) {
            DisplayFilter = DisplayItemTitle(SelectedOption);
        }
        if(ItemsSource != null) {
            await RetrieveFilteredFromItemsSource();
        }
    }

    /// <summary>
    /// Process set values against the filter asynchronously so that search can be performed.
    /// </summary>
    /// <remarks>
    /// This is done using one of the Blazor "anti-patterns" as binding doesn't work with Blazor 6
    /// and the fix designed for .NET 7 "missed the RTM window" a couple of times. Eventually this
    /// will be better to use binding with `@bind:after="SetOptionsFilter"` but that's not ready
    /// yet.
    /// </remarks>
    protected async Task DoFilterInput(ChangeEventArgs args)
    {
        await SetOptionsFilter(args.Value?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Set the list of options that should be shown based on the current filter and process UI
    /// side-effects.
    /// </summary>
    private async Task SetOptionsFilter(string filter)
    {
        // change the filter display the user sees
        DisplayFilter = filter;
        // And filter the internal list of items.
        InternalItems.SetFilter(filter);
        // When filter changes, clear the selected option
        if(!DisplayFilter.Equals(DisplayItemTitle(SelectedOption), StringComparison.OrdinalIgnoreCase)) {
            SelectedOption = default;
        }
        // When filtering from scratch, auto-expand options
        if(!string.IsNullOrWhiteSpace(DisplayFilter) && SelectedOption == null) {
            ShowOptions = true;
        }
        // If using ItemsSource provider, re-fetch items for filter condition
        if(ItemsSource != null) {
            await RetrieveFilteredFromItemsSource();
        }
        //ShouldRender();
    }

    /// <summary>
    /// Handle keypresses for navigation, preventing default when they're handled in the component
    /// and the key shouldn't be bubbled up to the page.
    /// </summary>
    protected async Task DoKeyDown(KeyboardEventArgs args)
    {
        PreventDefault = false;
        // 9 lines shown so page up/down should be one less so we have one line overlap for
        // context.
        var pageSize = 8;
        Console.WriteLine(args.Code);
        if(args.Code is "Enter" or "NumpadEnter") {
            PreventDefault = true; // must occur before await, as needed to prevent button clicks.
            if(ShowOptions) {
                await ConfirmInputAsync(SelectedOption);
            }
            else {
                await LoadOptionsAsync();
            }
        }
        if(ShowOptions) {
            switch(args.Code) {
                case "PageUp":
                    SelectOptionOffset(-pageSize);
                    PreventDefault = true;
                    break;

                case "PageDown":
                    SelectOptionOffset(+pageSize);
                    PreventDefault = true;
                    break;

                case "ArrowUp":
                    SelectOptionOffset(-1);
                    PreventDefault = true;
                    break;

                case "ArrowDown":
                    SelectOptionOffset(+1);
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
    }

    /// <summary>
    /// Given an index (relative to the `FilteredItems`), checks bounds and selects item.
    /// </summary>
    private void SelectOptionIndex(int index)
    {
        SelectedOption = index < 0 || index >= InternalItems.FilteredItems.Count
            ? default
            : InternalItems.FilteredItems[index];
        ShouldRender();
    }

    /// <summary>
    /// Selects a new item based on the position of the currently selected item. Enables page up,
    /// page down, item up and item down commands.
    /// </summary>
    private void SelectOptionOffset(int offset)
    {
        if(SelectedOption == null) {
            if(offset > 0) {
                SelectOptionIndex(0);
            }
            else {
                SelectOptionIndex(InternalItems.FilteredItems.Count - 1);
            }
        }
        else {
            var SelectedIndex = SelectedOption == null ? -1 : InternalItems.FilteredItems.IndexOf(SelectedOption);
            var newIndex = SelectedIndex + offset;
            newIndex = Math.Clamp(newIndex, 0, InternalItems.FilteredItems.Count - 1);
            SelectOptionIndex(newIndex);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(computeFilterCancellationSource != null) {
            computeFilterCancellationSource.Cancel();
            computeFilterCancellationSource.Dispose();
            computeFilterCancellationSource = null;
        }
    }

    private SortedFilteredCollection<TItem> InternalItems { get; set; } = new();

    [Inject]
    private ILogger<ComboBox<TItem>> Logger { get; set; } = null!;
}

internal class SortedFilteredCollection<T>
{
    public IEnumerable<T>? SourceItems { get; private set; }

    public void SetFuncs(Func<T?, string> group, Func<T?, string> sort, Func<T?, string> display)
    {
        GroupFunc = group;
        SortFunc = sort;
        DisplayFunc = display;
    }

    public void SetItems(IEnumerable<T> items, bool group = false, bool sort = true, string filter = "")
    {
        if(SourceItems != items || group != Group || sort != Sort || Filter != filter) {
            SourceItems = items;
            Group = group;
            Sort = sort;
            Filter = filter;
            ApplySort();
            ApplyFilter();
        }
    }

    public void SetFilter(string filter)
    {
        if(Filter != filter) {
            Filter = filter;
            ApplyFilter();
        }
    }

    public Func<T?, string> GroupFunc { get; private set; } = e => (e?.ToString() ?? "unnamed")[0..1].ToUpper(CultureInfo.CurrentCulture);

    public Func<T?, string> SortFunc { get; private set; } = e => e?.ToString() ?? "unnamed";

    public Func<T?, string> DisplayFunc { get; private set; } = e => e?.ToString() ?? "unnamed";

    public bool Group { get; private set; }

    public bool Sort { get; private set; }

    public string Filter { get; private set; } = string.Empty;

    public bool UseFilter { get; set; } = true;

    private void ApplySort()
    {
        SortedItems = (SourceItems, Group, Sort) switch {
            (null, _, _) => [],
            (_, false, false) => SourceItems.ToList(),
            (_, true, false) => SourceItems.OrderBy(GroupFunc).ToList(),
            (_, false, true) => SourceItems.OrderBy(SortFunc).ToList(),
            (_, true, true) => SourceItems.OrderBy(GroupFunc).ThenBy(SortFunc).ToList(),
        };
    }

    private void ApplyFilter()
    {
        FilteredItems = UseFilter && !string.IsNullOrWhiteSpace(Filter)
            ? SortedItems.Where(e => DisplayFunc(e).Contains(Filter, StringComparison.OrdinalIgnoreCase)).ToList()
            : SortedItems;
    }

    private List<T> SortedItems { get; set; } = [];

    public List<T> FilteredItems { get; private set; } = [];
}
