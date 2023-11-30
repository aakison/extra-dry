using ExtraDry.Blazor.Components.Internal;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public partial class DryTable<TItem> : ComponentBase, IDisposable, IExtraDryComponent {

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public object ViewModel { get; set; } = null!; // If not overridden, set to this in OnInitialized.

    [Parameter]
    public ICollection<TItem>? Items { get; set; }

    [Parameter]
    public IListService<TItem>? ItemsService { get; set; }

    [CascadingParameter]
    internal QueryBuilder QueryBuilder { get; set; } = new QueryBuilder();

    [Parameter]
    public Func<TItem, TItem>? GroupFunc { get; set; }

    [Parameter]
    public string? GroupColumn { get; set; }

    /// <summary>
    /// Optional name of a property to sort the table by.
    /// </summary>
    [Parameter]
    public string? Sort { get; set; }

    /// <summary>
    /// If `Sort` specified, determines the order of the sort.
    /// </summary>
    [Parameter]
    public bool SortAscending { get; set; }

    [Parameter]
    public SelectionSet? Selection { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private ViewModelDescription description = null!; // Set in OnInitialized

    [Inject]
    private ILogger<DryTable<TItem>> Logger { get; set; } = null!;

    private bool HasCheckboxColumn => description.ListSelectMode == ListSelectMode.Multiple;

    private bool HasRadioColumn => description.ListSelectMode == ListSelectMode.Single;

    private bool HasCommandsColumn => description.ContextCommands.Any();

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ModelClass, FilteredClass, StateClass, CssClass);

    private string ModelClass => description.ModelType?.Name?.ToLowerInvariant() ?? "";

    private string FilteredClass => string.IsNullOrWhiteSpace(QueryBuilder?.Build()?.Filter) ? "unfiltered" : "filtered";

    private string StateClass => 
        InternalItems.Any() ? "full" 
        : changing ? "changing"
        : firstLoadCompleted ? "empty" 
        : "loading";

    private Virtualize<ListItemInfo<TItem>>? VirtualContainer { get; set; }

    private SelectionSet? resolvedSelection;

    private int ColumnCount => description.TableProperties.Count +
        ((HasCheckboxColumn || HasRadioColumn) ? 1 : 0) +
        (HasCommandsColumn ? 1 : 0);

    protected override void OnInitialized()
    {
        ViewModel ??= this;
        description = new ViewModelDescription(typeof(TItem), ViewModel);
    }

    protected override void OnParametersSet()
    {
        AssertItemsMutualExclusivity();
        resolvedSelection = Selection ?? SelectionSet.Lookup(ViewModel) ?? SelectionSet.Register(ViewModel);
        resolvedSelection.MultipleSelect = description.ListSelectMode == ListSelectMode.Multiple;
        resolvedSelection.Changed += ResolvedSelection_Changed;
        if(QueryBuilder != null) {
            QueryBuilder.OnChanged += Notify_OnChanged;
        }
    }

    private void ResolvedSelection_Changed(object? sender, SelectionSetChangedEventArgs e)
    {
        // Checking/unchecking a row could affect the column checkbox...
        StateHasChanged();
    }

    private bool changing;

    private async void Notify_OnChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("Got notification");
        changing = true;
        StateHasChanged();
        InternalItems.Clear();
        if(VirtualContainer != null) {
            await VirtualContainer.RefreshDataAsync();
        }
        changing = false;
        StateHasChanged();
    }

    private void AssertItemsMutualExclusivity()
    {
        if(Items != null && ItemsService != null) {
            throw new DryException("Only one of `Items` and `ItemsService` is allowed to be set");
        }
    }

    private async Task PerformInitialSort()
    {
        CalculateGroupDepth();
        if(!string.IsNullOrWhiteSpace(Sort)) {
            var property = description.TableProperties.FirstOrDefault(e => string.Equals(e.Property.Name, Sort, StringComparison.OrdinalIgnoreCase));
            if(property == null) {
                Sort = null;
            }
            else {
                await SortBy(property, false);
                StateHasChanged();
            }
        }
    }

    private void CalculateGroupDepth()
    {
        foreach(var item in InternalItems) {
            FindGroup(item);
        }
        GroupBy();
    }

    private void FindGroup(ListItemInfo<TItem> item)
    {
        if(item.Item is IHierarchyEntity hierarchyItem) {
            item.GroupDepth = hierarchyItem.Lineage.GetLevel();
            return;
        }

        if(GroupFunc == null || item.Item == null) {
            return;
        }
        var group = GroupFunc(item.Item);
        if(group == null) {
            return;
        }

        var wrapper = InternalItems.FirstOrDefault(e => e.Item?.Equals(group) ?? false);
        if(wrapper != null) {
            if(wrapper.Group == null) {
                FindGroup(wrapper);
            }
            item.Group = wrapper;
            wrapper.IsGroup = true;
            item.GroupDepth = (wrapper?.GroupDepth ?? 0) + 1;
        }
    }

    private void GroupBy()
    {
        var comparer = new GroupComparer<TItem>();
        InternalItems.Sort(comparer);
    }

    private bool AllSelected => resolvedSelection?.All() ?? false;

    private void ToggleSelectAll()
    {
        if(resolvedSelection == null) {
            return;
        }
        if(AllSelected) {
            resolvedSelection.Clear();
        }
        else {
            resolvedSelection.SelectAll();
        }
    }

    private async Task SortBy(PropertyDescription property, bool reverseOrder = true)
    {
        var sort = property.Property.Name;
        if(sort == Sort) {
            if(reverseOrder) {
                SortAscending = !SortAscending;
            }
        }
        else {
            Sort = property.Property.Name;
            SortAscending = true;
        }
        if(Items != null) {
            // Client side sort, we've got all items.
            IComparer<ListItemInfo<TItem>> comparer = new ItemComparer<TItem>(property, SortAscending);
            if(GroupFunc != null) {
                comparer = new GroupComparer<TItem>(comparer);
            }
            InternalItems.Sort(comparer);
        }
        else {
            // Server side sort, can't assume all items, clear them out and re-request.
            changing = true;
            InternalItems.Clear();
            if(VirtualContainer != null) {
                await VirtualContainer.RefreshDataAsync();
            }
            changing = false;
        }
    }

    private void Toggle(ListItemInfo<TItem> item)
    {
        Console.WriteLine($"Code {item.Item?.GetHashCode()}");
        item.IsExpanded = !item.IsExpanded;
        StateHasChanged();
    }

    private ItemCollection<TItem> InternalItems { get; } = new ItemCollection<TItem>();

    private IEnumerable<ListItemInfo<TItem>> ShownItems => InternalItems.Where(e => e.IsShown);

    private async ValueTask<ItemsProviderResult<ListItemInfo<TItem>>> GetItemsAsync(ItemsProviderRequest request)
    {
        if(ItemsService == null) {
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        await serviceLock.WaitAsync();
        try {
            request.CancellationToken.ThrowIfCancellationRequested();
            if(!InternalItems.Any()) {
                Logger.LogConsoleVerbose("Loading initial items from remote service.");
                var firstPage = PageFor(request.StartIndex);
                var firstIndex = FirstItemOnPage(firstPage);
                QueryBuilder.Skip = firstIndex;
                var container = await ItemsService.GetListItemsAsync(QueryBuilder.Build(), request.CancellationToken);
                var count = container.ItemInfos.Count;
                var total = container.Total;
                QueryBuilder.Level.UpdateMaxLevel(ItemsService.MaxLevel);

                InternalItems.AddRange(container.ItemInfos);
                InternalItems.AddRange(Enumerable.Range(0, total - count).Select(e => new ListItemInfo<TItem>()));
                Logger.LogPartialResults(typeof(TItem), 0, count, total);
            }
            if(AllItemsCached(request.StartIndex, request.Count)) {
                Logger.LogConsoleVerbose("Returning cached results");
                var count = Math.Min(request.Count, InternalItems.Count);
                var items = InternalItems.GetRange(request.StartIndex, count);
                await PerformInitialSort();

                return new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
            }
            else {
                Logger.LogConsoleVerbose("Loading page of items from remote service.");
                var firstPage = PageFor(request.StartIndex);
                var lastPage = PageFor(request.StartIndex + request.Count);
                for(int pageNumber = firstPage; pageNumber <= lastPage; ++pageNumber) {
                    var firstIndex = FirstItemOnPage(pageNumber);
                    QueryBuilder.Skip = firstIndex;
                    if(!AllItemsCached(firstIndex, ItemsService.PageSize)) {
                        var container = await ItemsService.GetListItemsAsync(QueryBuilder.Build(), request.CancellationToken);
                        var count = container.ItemInfos.Count;
                        var total = container.Total;
                        QueryBuilder.Level.UpdateMaxLevel(ItemsService.MaxLevel);
                        var index = firstIndex;
                        foreach(var item in container.ItemInfos) {
                            var info = InternalItems[index++];
                            info.Item = item.Item;
                            info.IsLoaded = item.IsLoaded;
                            info.IsExpanded = item.IsExpanded;
                            info.GroupDepth = item.GroupDepth;
                            info.IsGroup = item.IsGroup;
                        }
                        var lastIndex = firstIndex + ItemsService.PageSize;
                        Logger.LogPartialResults(typeof(TItem), firstIndex, count, total);
                    }
                }
            }
            if(InternalItems.Any()) {
                var count = Math.Min(request.Count, InternalItems.Count);
                var items = InternalItems.GetRange(request.StartIndex, count);
                await PerformInitialSort();
                return new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
            }
            else {
                var x = new ItemsProviderResult<ListItemInfo<TItem>>();
                return x;
            }
        }
        catch(OperationCanceledException) {
            // KLUDGE: The CancellationTokenSource is initiated in the Virtualize component, but
            // it can't handle the exception. Catch the exception here and return an empty
            // result instead.  
            Logger.LogConsoleVerbose("Loading cancelled by request");
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        finally {
            serviceLock.Release();
            firstLoadCompleted = true;
            StateHasChanged(); // update classes affected by InternalItems
        }
        

        bool AllItemsCached(int start, int count) => InternalItems.Skip(start).Take(count).All(e => e.IsLoaded);

        int PageFor(int index) => index / ItemsService.PageSize;

        int FirstItemOnPage(int page) => ItemsService.PageSize * page;
    }

    private bool firstLoadCompleted;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(resolvedSelection != null) {
            resolvedSelection.Changed -= ResolvedSelection_Changed;
        }
    }

    private readonly SemaphoreSlim serviceLock = new(1, 1);

}
