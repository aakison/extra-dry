using ExtraDry.Blazor.Components.Internal;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public partial class DryTable<TItem> : ComponentBase, IDisposable {

    [Parameter]
    public object ViewModel { get; set; } = null!; // If not overridden, set to this in OnInitialized.

    [Parameter]
    public ICollection<TItem>? Items { get; set; }

    [Parameter]
    public IListService<TItem>? ItemsService { get; set; }

    [CascadingParameter]
    internal PageQueryBuilder? PageQueryBuilder { get; set; }

    ///// <summary>
    ///// Optional object that controls how items in the table are grouped.
    ///// A typical use is to group children under their parents.
    ///// </summary>
    //[Parameter]
    //public IGroupProvider<TItem>? GroupProvider { get; set; }

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

    private ViewModelDescription description = null!; // Set in OnInitialized

    [Inject]
    private ILogger<DryTable<TItem>> Logger { get; set; } = null!;

    private bool HasCheckboxColumn => description.ListSelectMode == ListSelectMode.Multiple;

    private bool HasRadioColumn => description.ListSelectMode == ListSelectMode.Single;

    private bool HasCommandsColumn => description.ContextCommands.Any();

    private string TableClasses => $"{ModelClass} {FilteredClass} {StateClass}";

    private string ModelClass => description.ModelType?.Name?.ToLowerInvariant() ?? "";

    private string FilteredClass => string.IsNullOrWhiteSpace(PageQueryBuilder?.Build()?.Filter) ? "unfiltered" : "filtered";

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
        Logger.LogInformation("DryTable.OnInitialized");
        ViewModel ??= this;
        description = new ViewModelDescription(typeof(TItem), ViewModel);
    }

    protected override void OnParametersSet()
    {
        AssertItemsMutualExclusivity();
        resolvedSelection = Selection ?? SelectionSet.Lookup(ViewModel) ?? SelectionSet.Register(ViewModel);
        resolvedSelection.MultipleSelect = description.ListSelectMode == ListSelectMode.Multiple;
        resolvedSelection.Changed += ResolvedSelection_Changed;
        if(PageQueryBuilder != null) {
            PageQueryBuilder.OnChanged += Notify_OnChanged;
        }
    }

    private void ResolvedSelection_Changed(object? sender, SelectionSetChangedEventArgs e)
    {
        // Checking/unchecking a row could affect the column checkbox...
        StateHasChanged();
    }

    private bool changing = false;

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
        if(GroupFunc != null) {
            foreach(var item in InternalItems) {
                FindGroup(item);
            }
            GroupBy();
        }
    }

    private void FindGroup(ListItemInfo<TItem> item)
    {
        if(GroupFunc == null) {
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
        if(InternalItems != null) {
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
        Logger.LogInformation(@"DryTable: Getting page of results, from index {StartIndex}, fetching {Count}", request.StartIndex, request.Count);
        if(ItemsService == null) {
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        await serviceLock.WaitAsync();
        try {
            request.CancellationToken.ThrowIfCancellationRequested();
            if(!InternalItems.Any()) {
                Logger.LogInformation("--Loading initial items from remote service...");
                var firstPage = PageFor(request.StartIndex);
                var firstIndex = FirstItemOnPage(firstPage);
                var items = await ItemsService.GetItemsAsync(PageQueryBuilder?.Query?.Filter, Sort, SortAscending, firstIndex, ItemsService.FetchSize);
                var count = items.Items.Count();
                var total = items.TotalItemCount;
                                

                InternalItems.AddRange(items.Items.Select(e => new ListItemInfo<TItem> { Item = e, IsLoaded = true }));
                InternalItems.AddRange(Enumerable.Range(0, total - count).Select(e => new ListItemInfo<TItem>()));
                Logger.LogInformation(@"DryTable: --Loaded items #0 to #{count} of {total}.", count, total);
            }
            if(AllItemsCached(request.StartIndex, request.Count)) {
                Logger.LogInformation("--Returning cached results");
                var count = Math.Min(request.Count, InternalItems.Count);
                var items = InternalItems.GetRange(request.StartIndex, count);
                await PerformInitialSort();

                return new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
            }
            else {
                Logger.LogInformation("--Loading page of items from remote service...");
                var firstPage = PageFor(request.StartIndex);
                var lastPage = PageFor(request.StartIndex + request.Count);
                for(int pageNumber = firstPage; pageNumber <= lastPage; ++pageNumber) {
                    var firstIndex = FirstItemOnPage(pageNumber);
                    if(!AllItemsCached(firstIndex, ItemsService.FetchSize)) {
                        var items = await ItemsService.GetItemsAsync(PageQueryBuilder?.Query?.Filter, Sort, SortAscending, firstIndex, ItemsService.FetchSize);
                        var count = items.Items.Count();
                        var total = items.TotalItemCount;
                        var index = firstIndex;
                        foreach(var item in items.Items) {
                            var info = InternalItems[index++];
                            info.Item = item;
                            info.IsLoaded = true;
                        }
                        var lastIndex = firstIndex + ItemsService.FetchSize;
                        Logger.LogInformation(@"--Loaded items #{firstIndex} to #{lastIndex} of {total}.", firstIndex, lastIndex, total);
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
            // KLUDGE: The CancellationTokenSource is initiationed in the Virtualize component, but it can't handle the exception.
            // Catch the exception here and return an empty result instead.  
            Logger.LogInformation("--Loading cancelled");
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        finally {
            serviceLock.Release();
            firstLoadCompleted = true;
            StateHasChanged(); // update classes affected by InternalItems
        }
        

        bool AllItemsCached(int start, int count) => InternalItems.Skip(start).Take(count).All(e => e.IsLoaded);

        int PageFor(int index) => index / ItemsService.FetchSize;

        int FirstItemOnPage(int page) => ItemsService.FetchSize * page;
    }

    private bool firstLoadCompleted = false;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(resolvedSelection != null) {
            resolvedSelection.Changed -= ResolvedSelection_Changed;
        }
    }

    private readonly SemaphoreSlim serviceLock = new(1, 1);

}
