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

    private bool HasCommandsColumn => description.ContextCommands.Count != 0;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ModelClass, FilteredClass, StateClass, CssClass);

    private string ModelClass => description.ModelType?.Name?.ToLowerInvariant() ?? "";

    private string FilteredClass => string.IsNullOrWhiteSpace(QueryBuilder?.Build()?.Filter) ? "unfiltered" : "filtered";

    private string StateClass => 
        InternalItems.Any() ? "full" 
        : changing ? "changing"
        : validationError ? "invalid-filter"
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
        if(resolvedSelection == null) {
            resolvedSelection = Selection ?? SelectionSet.Lookup(ViewModel) ?? SelectionSet.Register(ViewModel);
            resolvedSelection.MultipleSelect = description.ListSelectMode == ListSelectMode.Multiple;
            resolvedSelection.Changed += ResolvedSelection_Changed;
        }
        if(QueryBuilder != null && !queryBuilderEventSet) {
            QueryBuilder.OnChanged += Notify_OnChanged;
            queryBuilderEventSet = true;
        }
    }

    private void ResolvedSelection_Changed(object? sender, SelectionSetChangedEventArgs e)
    {
        Logger.LogConsoleVerbose("Got selection notification");
        // Checking/unchecking a row could affect the column checkbox...
        StateHasChanged();
    }

    private bool changing;

    private async void Notify_OnChanged(object? sender, EventArgs e)
    {
        Logger.LogConsoleVerbose("Got notification");
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

    private void PerformInitialSort()
    {
        CalculateGroupDepth();
        if(!string.IsNullOrWhiteSpace(QueryBuilder.Sort.SortProperty)) {
            var property = description.TableProperties.FirstOrDefault(e => string.Equals(e.Property.Name, QueryBuilder.Sort.SortProperty, StringComparison.OrdinalIgnoreCase));
            if(property == null) {
                QueryBuilder.Sort.SortProperty = "";
            }
            else if(QueryBuilder.Sort.SortProperty != property.Property.Name){                
                SortBy(property, false);
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

    private bool IsHierarchyList { 
        get {
            return InternalItems.Any(e => e.Item is IHierarchyEntity);
        } 
    }

    private void SortBy(PropertyDescription property, bool reverseOrder = true)
    {
        if(IsHierarchyList || property.Sort?.Type == SortType.NotSortable) {
            return;
        }

        var sort = property.Property.Name;
        if(sort == QueryBuilder.Sort.SortProperty) {
            if(reverseOrder) {
                QueryBuilder.Sort.Ascending = !QueryBuilder.Sort.Ascending;
            }
        }
        else {
            QueryBuilder.Sort.SortProperty = property.Property.Name;
            QueryBuilder.Sort.Ascending = true;
        }
        
        if(Items != null) {
            // Client side sort, we've got all items.
            IComparer<ListItemInfo<TItem>> comparer = new ItemComparer<TItem>(property, QueryBuilder.Sort.Ascending);
            if(GroupFunc != null) {
                comparer = new GroupComparer<TItem>(comparer);
            }
            InternalItems.Sort(comparer);
        }
        else {
            QueryBuilder.NotifyChanged();
        }
    }

    private void Toggle(ListItemInfo<TItem> item)
    {
        Console.WriteLine($"Code {item.Item?.GetHashCode()}");
        item.IsExpanded = !item.IsExpanded;
        StateHasChanged();
    }

    private ItemCollection<TItem> InternalItems { get; } = [];

    private IEnumerable<ListItemInfo<TItem>> ShownItems => InternalItems.Where(e => e.IsShown);

    public bool TryRefreshItem(TItem updatedItem, IEqualityComparer<TItem>? comparer = null)
    {
        var itemInfo = (comparer, updatedItem) switch {
            (var c, _) when c is not null => InternalItems.FirstOrDefault(itemInfo => c.Equals(itemInfo.Item, updatedItem)),
            (_, var item) when item is IUniqueIdentifier uniquelyIdentifiableItem => InternalItems.FirstOrDefault(itemInfo => (itemInfo.Item as IUniqueIdentifier)?.Uuid == uniquelyIdentifiableItem.Uuid),
            (_, _) => throw new DryException($"Refreshing requires {nameof(TItem)} to implement {nameof(IUniqueIdentifier)} or an {nameof(IEqualityComparer)} to be provided.")
        };
        if( itemInfo == null ) { 
            return false;
        }
        itemInfo.Item = updatedItem;
        return true;
    }

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
                QueryBuilder.Level.UpdateMinLevel(ItemsService.MinLevel);

                InternalItems.AddRange(container.ItemInfos);
                InternalItems.AddRange(Enumerable.Range(0, total - count).Select(e => new ListItemInfo<TItem>()));
                Logger.LogPartialResults(typeof(TItem), 0, count, total);
            }
            if(AllItemsCached(request.StartIndex, request.Count)) {
                Logger.LogConsoleVerbose("Returning cached results");
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
                        QueryBuilder.Level.UpdateMinLevel(ItemsService.MinLevel);
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
            ItemsProviderResult<ListItemInfo<TItem>> result;
            if(InternalItems.Any()) {
                var count = Math.Min(request.Count, InternalItems.Count);
                var items = InternalItems.GetRange(request.StartIndex, count);
                if(!IsHierarchyList) {
                    PerformInitialSort();
                }
                result = new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
            }
            else {
                result = new();
            }
            firstLoadCompleted = true;
            validationError = false;
            return result;
        }
        catch(OperationCanceledException) {
            // KLUDGE: The CancellationTokenSource is initiated in the Virtualize component, but
            // it can't handle the exception. Catch the exception here and return an empty
            // result instead.  
            Logger.LogConsoleVerbose("Loading cancelled by request");
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        catch(DryException dex) {
            if(dex.ProblemDetails != null && dex.ProblemDetails.Status == 400) {
                validationError = true;
                return new ItemsProviderResult<ListItemInfo<TItem>>();
            }
            else {
                throw;
            }
        }
        finally {
            serviceLock.Release();
            StateHasChanged(); // update classes affected by InternalItems
        }
        

        bool AllItemsCached(int start, int count) => InternalItems.Skip(start).Take(count).All(e => e.IsLoaded);

        int PageFor(int index) => index / ItemsService.PageSize;

        int FirstItemOnPage(int page) => ItemsService.PageSize * page;
    }

    private bool firstLoadCompleted;

    private bool validationError;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(resolvedSelection != null) {
            resolvedSelection.Changed -= ResolvedSelection_Changed;
        }
        if(QueryBuilder != null && queryBuilderEventSet) {
            QueryBuilder.OnChanged -= Notify_OnChanged;
        }
    }

    private readonly SemaphoreSlim serviceLock = new(1, 1);
    private bool queryBuilderEventSet;
}
