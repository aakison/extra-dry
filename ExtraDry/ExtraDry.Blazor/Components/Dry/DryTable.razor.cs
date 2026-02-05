using System.Diagnostics.CodeAnalysis;
using ExtraDry.Blazor.Components.Internal;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ExtraDry.Blazor;

public partial class DryTable<TItem> : ComponentBase, IDisposable, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    [Parameter]
    public ICollection<TItem>? Items { get; set; }

    [Parameter]
    public IListClient<TItem>? ItemsClient { get; set; }

    [Parameter]
    public Func<TItem, TItem>? GroupFunc { get; set; }

    [Parameter]
    public string? GroupColumn { get; set; }

    /// <summary>
    /// Indicates if command buttons should be shown for each row.  Commands must be contextual
    /// commands to appear on each row.  That is, they take a single item as a parameter.
    /// </summary>
    [Parameter]
    public bool ShowCommands { get; set; } = true;

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private DecoratorInfo decorator = null!; // Set in OnInitialized

    [Inject]
    private ILogger<DryTable<TItem>> Logger { get; set; } = null!;

    /// <summary>
    /// The total number of items in the list.  If -1, the total is unknown.
    /// </summary>
    public int Total { get; set; } = -1;

    /// <summary>
    /// Every table gets a unique identifier to allow multiple tables on one page, each with custom styles for that table only.
    /// </summary>
    private string TableId { get; init; } = $"DryTable{TableCount++}";

    private static int TableCount { get; set; }

    private string CustomStyle {
        get {
            var widths = string.Join(" ", decorator.TableProperties.Select(e => $"{10 * (int)e.Size}fr"));
            var check = HasCheckboxColumn | HasRadioColumn ? "auto " : "";
            var commands = HasCommandsColumn ? " auto" : "";
            var styles = $@"
            #{TableId} dry-tr {{
                grid-template-columns: {check}{widths}{commands};
            }}
            ";
            return styles;
        }
    }

    private bool HasCheckboxColumn => decorator.ListSelectMode == ListSelectMode.Multiple;

    private bool HasRadioColumn => decorator.ListSelectMode == ListSelectMode.Single;

    private bool HasCommandsColumn => decorator.ContextCommands.Any();

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "sortable", ModelClass, FilteredClass, StateClass, CssClass);

    private string ModelClass => decorator.ModelType?.Name?.ToLowerInvariant() ?? "";

    private string FilteredClass => string.IsNullOrWhiteSpace(QueryBuilderAccessor?.QueryBuilder.Build().Filter) ? "unfiltered" : "filtered";

    private string StateClass => (ItemsClient?.IsEmpty, ItemsClient?.IsLoading) switch {
        (null, _) => "loading",
        (false, _) => "full",
        (true, _) => "empty",
    };
    //!ItemsService.IsEmpty ? "full" : 
    //changing ? "changing"
    //: validationError ? "invalid-filter"
    //: firstLoadCompleted ? "empty"
    //: "loading";

    private Virtualize<ListItemInfo<TItem>>? VirtualContainer { get; set; }

    private SelectionSetAccessor? SelectionAccessor { get; set; }

    private int ColumnCount => decorator.TableProperties.Count +
        ((HasCheckboxColumn || HasRadioColumn) ? 1 : 0) +
        (HasCommandsColumn ? 1 : 0);

    protected override void OnInitialized()
    {
        Decorator ??= this;
        decorator = new DecoratorInfo(typeof(TItem), Decorator);
    }

    protected override void OnParametersSet()
    {
        AssertItemsMutualExclusivity();
        if(SelectionAccessor == null) {
            SelectionAccessor = new SelectionSetAccessor(Decorator);
            SelectionAccessor.SelectionSet.MultipleSelect = decorator.ListSelectMode == ListSelectMode.Multiple;
            SelectionAccessor.SelectionSet.Changed += Selection_Changed;
        }
        if(QueryBuilderAccessor == null) {
            QueryBuilderAccessor = new QueryBuilderAccessor(Decorator);
            QueryBuilderAccessor.QueryBuilder.OnChanged += Query_Changed;
        }
        if(ItemsListClient == null && ItemsClient != null) {
            CachingItemsListClient = new CachingListClient<TItem>(ItemsClient);
            var projectionListClient = new ProjectionListClient<TItem, ListItemInfo<TItem>>(CachingItemsListClient,
                e => new ListItemInfo<TItem> { Item = e, IsLoaded = true });
            ItemsListClient = projectionListClient;
        }
    }

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Enforce that there are many options.")]
    private IListClient<ListItemInfo<TItem>>? ItemsListClient { get; set; }

    // HACK: Keep a reference to the caching client so it we can invalidate portions. 
    // however it's also wrapped in a ProjectionListClient for the Table.  Would prefer a cleaner way.
    private CachingListClient<TItem>? CachingItemsListClient { get; set; }

    private void Selection_Changed(object? sender, SelectionSetChangedEventArgs e)
    {
        Logger.LogConsoleVerbose("Got selection notification");
        // Checking/unchecking a row could affect the column checkbox...
        StateHasChanged();
    }

    private async void Query_Changed(object? sender, EventArgs e)
    {
        Logger.LogConsoleVerbose("Got notification");
        await RefreshAsync();
    }

    public async Task RefreshAsync()
    {
        StateHasChanged();
        CachingItemsListClient?.ClearCache();
        if(VirtualContainer != null) {
            await VirtualContainer.RefreshDataAsync();
        }
        StateHasChanged();
    }

    private void AssertItemsMutualExclusivity()
    {
        if(Items != null && ItemsClient != null) {
            throw new DryException("Only one of `Items` and `ItemsService` is allowed to be set");
        }
    }

    private void PerformInitialSort()
    {
        if(!string.IsNullOrWhiteSpace(QueryBuilderAccessor?.QueryBuilder.Sort.SortProperty)) {
            var property = decorator.TableProperties.FirstOrDefault(e => string.Equals(e.Property.Name, QueryBuilderAccessor.QueryBuilder.Sort.SortProperty, StringComparison.OrdinalIgnoreCase));
            if(property == null) {
                QueryBuilderAccessor.QueryBuilder.Sort.SortProperty = "";
            }
            else if(QueryBuilderAccessor.QueryBuilder.Sort.SortProperty != property.Property.Name) {
                SortBy(property, false);
                StateHasChanged();
            }
        }
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

    }


    private bool AllSelected => SelectionAccessor?.SelectionSet.All() ?? false;

    private void SortBy(PropertyDescription property, bool reverseOrder = true)
    {
        if(QueryBuilderAccessor == null || property.Sort?.Type == SortType.NotSortable) {
            return;
        }

        var sort = property.Property.Name;
        if(sort == QueryBuilderAccessor.QueryBuilder.Sort.SortProperty) {
            if(reverseOrder) {
                QueryBuilderAccessor.QueryBuilder.Sort.Ascending = !QueryBuilderAccessor.QueryBuilder.Sort.Ascending;
            }
        }
        else {
            QueryBuilderAccessor.QueryBuilder.Sort.SortProperty = property.Property.Name;
            QueryBuilderAccessor.QueryBuilder.Sort.Ascending = true;
        }

        if(Items != null) {
            // Client side sort, we've got all items.
            IComparer<ListItemInfo<TItem>> comparer = new ItemComparer<TItem>(property, QueryBuilderAccessor.QueryBuilder.Sort.Ascending);
            if(GroupFunc != null) {
                comparer = new GroupComparer<TItem>(comparer);
            }
            //InternalItems.Sort(comparer);
        }
        else {
            QueryBuilderAccessor.QueryBuilder.NotifyChanged();
        }
    }

    private void Toggle(ListItemInfo<TItem> item)
    {
        Console.WriteLine($"Code {item.Item?.GetHashCode()}");
        item.IsExpanded = !item.IsExpanded;
        StateHasChanged();
    }

    //private IEnumerable<ListItemInfo<TItem>> ShownItems => InternalItems.Where(e => e.IsShown);

    public bool TryRefreshItem(TItem updatedItem, Func<TItem, bool>? matchPredicate = null)
    {
        var replaced = CachingItemsListClient?.TryRefreshItem(updatedItem, matchPredicate) ?? false;
        StateHasChanged();
        return replaced;
    }

    public async Task<bool> TryRemoveItemAsync(TItem removedItem, IEqualityComparer<TItem>? comparer = null)
    {
        StateHasChanged();

        var removed = CachingItemsListClient?.TryRemoveItem(removedItem) ?? false;
        if(VirtualContainer != null) {
            await VirtualContainer.RefreshDataAsync();
        }

        StateHasChanged();

        return removed;
    }

    private async ValueTask<ItemsProviderResult<ListItemInfo<TItem>>> GetItemsAsync(ItemsProviderRequest request)
    {
        if(ItemsClient == null || ItemsListClient == null || QueryBuilderAccessor == null) {
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        await serviceLock.WaitAsync();
        var builder = QueryBuilderAccessor.QueryBuilder;
        try {
            request.CancellationToken.ThrowIfCancellationRequested();
            builder.Skip = request.StartIndex;
            builder.Take = request.Count;
            var infos = await ItemsListClient.GetItemsAsync(builder.Build(), request.CancellationToken);
            var result = new ItemsProviderResult<ListItemInfo<TItem>>(infos.Items, infos.Total);
            return result;
        }
        catch(OperationCanceledException) {
            // KLUDGE: The CancellationTokenSource is initiated in the Virtualize component, but it
            // can't handle the exception. Catch the exception here and return an empty result
            // instead.
            Logger.LogConsoleVerbose("Loading cancelled by request");
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        catch(DryException dex) {
            if(dex.ProblemDetails != null && dex.ProblemDetails.Status == 400) {
                Logger.LogConsoleError($"Error applying filter: {dex?.ProblemDetails?.Title}");
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
    }

    private int TotalColumns => (HasCheckboxColumn ? 1 : 0) +
        (HasRadioColumn ? 1 : 0) +
        (HasCommandsColumn ? 1 : 0) +
        decorator.TableProperties.Count;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        // If table is removed but decorator remains, disconnect from events.
        if(SelectionAccessor != null) {
            SelectionAccessor.SelectionSet.Changed -= Selection_Changed;
            SelectionAccessor = null;
        }
        if(QueryBuilderAccessor != null) {
            QueryBuilderAccessor.QueryBuilder.OnChanged -= Query_Changed;
            QueryBuilderAccessor = null;
        }
    }

    private readonly SemaphoreSlim serviceLock = new(1, 1);

}
