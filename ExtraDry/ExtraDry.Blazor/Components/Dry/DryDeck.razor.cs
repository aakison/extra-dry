using ExtraDry.Blazor.Components.Internal;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Globalization;

namespace ExtraDry.Blazor;

/// <summary>
/// A deck component that displays data as virtualized cards, following the same architectural 
/// patterns as DryTable but rendering items as cards using a customizable RenderFragment.
/// </summary>
/// <typeparam name="TItem">The type of items to display in the deck.</typeparam>
public partial class DryDeck<TItem> : ComponentBase, IDisposable, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The decorator object that provides metadata for the component. If not provided, defaults to 'this'.
    /// </summary>
    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    /// <summary>
    /// Direct collection of items to display. Mutually exclusive with ItemsService.
    /// </summary>
    [Parameter]
    public ICollection<TItem>? Items { get; set; }

    /// <summary>
    /// Service for loading items dynamically. Mutually exclusive with Items.
    /// </summary>
    [Parameter]
    public IListClient<TItem>? ItemsService { get; set; }

    /// <summary>
    /// The RenderFragment that defines the content of each card.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<TItem>? CardContent { get; set; }

    /// <summary>
    /// The size of each item for virtualization optimization. Default is 200 pixels.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 200f;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private QueryBuilderAccessor? QueryBuilderAccessor { get; set; }

    private SelectionSetAccessor? SelectionAccessor { get; set; }

    private DecoratorInfo description = null!; // Set in OnInitialized

    private Virtualize<ListItemInfo<TItem>>? VirtualContainer { get; set; }

    private readonly List<ListItemInfo<TItem>> InternalItems = [];

    private bool changing;

    private bool firstLoadCompleted;

    private bool validationError;

    private readonly SemaphoreSlim serviceLock = new(1, 1);

    [Inject]
    private ILogger<DryDeck<TItem>> Logger { get; set; } = null!;

    /// <summary>
    /// Indicates if the deck has checkbox selection enabled.
    /// </summary>
    private bool HasCheckboxSelection => description.ListSelectMode == ListSelectMode.Multiple;

    /// <summary>
    /// Indicates if the deck has radio button selection enabled.
    /// </summary>
    private bool HasRadioSelection => description.ListSelectMode == ListSelectMode.Single;

    /// <summary>
    /// Indicates if the deck has commands to render.
    /// </summary>
    private bool HasCommands => description.ContextCommands.Any();

    /// <summary>
    /// Combined CSS classes for the deck container.
    /// </summary>
    private string CssClasses => DataConverter.JoinNonEmpty(" ", "deck", ModelClass, FilteredClass, StateClass, CssClass);

    /// <summary>
    /// CSS class based on the model type name.
    /// </summary>
    private string ModelClass => description.ModelType?.Name?.ToLowerInvariant() ?? "";

    /// <summary>
    /// CSS class indicating filter state.
    /// </summary>
    private string FilteredClass => string.IsNullOrWhiteSpace(QueryBuilderAccessor?.QueryBuilder.Build().Filter) ? "unfiltered" : "filtered";

    /// <summary>
    /// CSS class indicating the current state of the deck.
    /// </summary>
    private string StateClass =>
        InternalItems.Count > 0 ? "full"
        : changing ? "changing"
        : validationError ? "invalid-filter"
        : firstLoadCompleted ? "empty"
        : "loading";

    /// <summary>
    /// Radio button scope for single selection mode.
    /// </summary>
    private string RadioButtonScope => $"deck-{GetHashCode()}";

    protected override void OnInitialized()
    {
        Decorator ??= this;
        description = new DecoratorInfo(typeof(TItem), Decorator);
    }

    protected override void OnParametersSet()
    {
        AssertItemsMutualExclusivity();
        
        if (SelectionAccessor == null)
        {
            SelectionAccessor = new SelectionSetAccessor(Decorator);
            SelectionAccessor.SelectionSet.MultipleSelect = description.ListSelectMode == ListSelectMode.Multiple;
            SelectionAccessor.SelectionSet.Changed += Selection_Changed;
        }
        
        if (QueryBuilderAccessor == null)
        {
            QueryBuilderAccessor = new QueryBuilderAccessor(Decorator);
            QueryBuilderAccessor.QueryBuilder.OnChanged += Query_Changed;
        }
    }

    /// <summary>
    /// Validates that only one of Items or ItemsService is set.
    /// </summary>
    private void AssertItemsMutualExclusivity()
    {
        if (Items != null && ItemsService != null)
        {
            throw new DryException("Only one of `Items` and `ItemsService` is allowed to be set");
        }
    }

    /// <summary>
    /// Handles selection changes and triggers state update.
    /// </summary>
    private void Selection_Changed(object? sender, SelectionSetChangedEventArgs e)
    {
        Logger.LogConsoleVerbose("Got selection notification");
        StateHasChanged();
    }

    /// <summary>
    /// Handles query changes and refreshes data.
    /// </summary>
    private async void Query_Changed(object? sender, EventArgs e)
    {
        Logger.LogConsoleVerbose("Got notification");
        await RefreshAsync();
    }

    /// <summary>
    /// Refreshes the deck data.
    /// </summary>
    public async Task RefreshAsync()
    {
        changing = true;
        StateHasChanged();
        InternalItems.Clear();
        if (VirtualContainer != null)
        {
            await VirtualContainer.RefreshDataAsync();
        }
        changing = false;
        SelectionAccessor?.SelectionSet.SetVisible(InternalItems.Where(e => e.Item is not null).Select(e => (object)e.Item!));
        StateHasChanged();
    }

    /// <summary>
    /// Gets the UUID for an item for tracking purposes.
    /// </summary>
    private static string GetItemUuid(TItem? item)
    {
        if (item is IUniqueIdentifier uniqueItem)
        {
            return uniqueItem.Uuid.ToString();
        }
        return item?.GetHashCode().ToString(CultureInfo.InvariantCulture) ?? "unknown";
    }

    /// <summary>
    /// Gets the CSS class for a card based on the item type and selection state.
    /// </summary>
    private string GetCardCssClass(ListItemInfo<TItem> itemInfo)
    {
        var classes = new List<string> { ModelClass };
        
        if (itemInfo.Item != null && IsSelected(itemInfo.Item))
        {
            classes.Add("selected");
        }
        
        return string.Join(" ", classes);
    }

    /// <summary>
    /// Checks if an item is selected.
    /// </summary>
    private bool IsSelected(TItem? item)
    {
        if (item == null || SelectionAccessor == null)
            return false;
            
        return SelectionAccessor.SelectionSet.Contains(item);
    }

    /// <summary>
    /// Toggles selection state for an item.
    /// </summary>
    private void ToggleSelection(TItem? item, ChangeEventArgs e)
    {
        if (item == null || SelectionAccessor == null)
            return;

        var isChecked = e.Value as bool? ?? false;
        
        if (isChecked)
        {
            SelectionAccessor.SelectionSet.Add(item);
        }
        else
        {
            SelectionAccessor.SelectionSet.Remove(item);
        }
    }

    /// <summary>
    /// Handles card click for selection in single selection mode.
    /// </summary>
    private void HandleCardClick(ListItemInfo<TItem> itemInfo)
    {
        if (itemInfo.Item == null || SelectionAccessor == null)
            return;

        if (description.ListSelectMode == ListSelectMode.Single)
        {
            SelectionAccessor.SelectionSet.Add(itemInfo.Item);
        }
    }

    /// <summary>
    /// Provider method for virtualized item loading.
    /// </summary>
    private async ValueTask<ItemsProviderResult<ListItemInfo<TItem>>> GetItemsAsync(ItemsProviderRequest request)
    {
        if (Items != null)
        {
            // Handle direct items collection
            if (InternalItems.Count == 0)
            {
                InternalItems.AddRange(Items.Select(item => new ListItemInfo<TItem> { Item = item, IsLoaded = true }));
            }
            
            var count = Math.Min(request.Count, InternalItems.Count - request.StartIndex);
            if (count <= 0)
            {
                return new ItemsProviderResult<ListItemInfo<TItem>>();
            }
            
            var items = InternalItems.GetRange(request.StartIndex, count);
            firstLoadCompleted = true;
            
            SelectionAccessor?.SelectionSet.SetVisible(InternalItems.Where(e => e.Item is not null).Select(e => (object)e.Item!));
            
            return new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
        }

        if (ItemsService == null || QueryBuilderAccessor == null)
        {
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }

        await serviceLock.WaitAsync();
        var builder = QueryBuilderAccessor.QueryBuilder;
        
        try
        {
            request.CancellationToken.ThrowIfCancellationRequested();
            
            if (InternalItems.Count == 0)
            {
                Logger.LogConsoleVerbose("Loading initial items from remote service.");
                var firstPage = PageFor(request.StartIndex);
                var firstIndex = FirstItemOnPage(firstPage);
                builder.Skip = firstIndex;
                var items = await ItemsService.GetItemsAsync(builder.Build(), request.CancellationToken);
                var infos = new ListItemsProviderResult<TItem>(items);
                var count = infos.ItemInfos.Count;
                var total = infos.Total;

                InternalItems.AddRange(infos.ItemInfos);
                InternalItems.AddRange(Enumerable.Range(0, total - count).Select(e => new ListItemInfo<TItem>()));
                Logger.LogPartialResults(typeof(TItem), 0, count, total);
            }
            
            if (AllItemsCached(request.StartIndex, request.Count))
            {
                Logger.LogConsoleVerbose("Returning cached results");
            }
            else
            {
                Logger.LogConsoleVerbose("Loading page of items from remote service.");
                var firstPage = PageFor(request.StartIndex);
                var lastPage = PageFor(request.StartIndex + request.Count);
                
                for (int pageNumber = firstPage; pageNumber <= lastPage; ++pageNumber)
                {
                    var firstIndex = FirstItemOnPage(pageNumber);
                    builder.Skip = firstIndex;
                    
                    if (!AllItemsCached(firstIndex, ItemsService.PageSize))
                    {
                        var items = await ItemsService.GetItemsAsync(builder.Build(), request.CancellationToken);
                        var infos = new ListItemsProviderResult<TItem>(items);
                        var count = infos.ItemInfos.Count;
                        var total = infos.Total;
                        var index = firstIndex;
                        
                        foreach (var item in infos.ItemInfos)
                        {
                            var info = InternalItems[index++];
                            info.Item = item.Item;
                            info.IsLoaded = item.IsLoaded;
                            info.IsExpanded = item.IsExpanded;
                            info.GroupDepth = item.GroupDepth;
                            info.IsGroup = item.IsGroup;
                        }
                        
                        Logger.LogPartialResults(typeof(TItem), firstIndex, count, total);
                    }
                }
            }

            ItemsProviderResult<ListItemInfo<TItem>> result;
            
            if (InternalItems.Count > 0)
            {
                var count = Math.Min(request.Count, InternalItems.Count);
                var items = InternalItems.GetRange(request.StartIndex, count);
                result = new ItemsProviderResult<ListItemInfo<TItem>>(items, InternalItems.Count);
            }
            else
            {
                result = new();
            }
            
            firstLoadCompleted = true;
            validationError = false;
            SelectionAccessor?.SelectionSet.SetVisible(InternalItems.Where(e => e.Item is not null).Select(e => (object)e.Item!));
            
            return result;
        }
        catch (OperationCanceledException)
        {
            Logger.LogConsoleVerbose("Loading cancelled by request");
            return new ItemsProviderResult<ListItemInfo<TItem>>();
        }
        catch (DryException dex)
        {
            if (dex.ProblemDetails != null && dex.ProblemDetails.Status == 400)
            {
                validationError = true;
                Logger.LogConsoleError($"Error applying filter: {dex?.ProblemDetails?.Title}");
                return new ItemsProviderResult<ListItemInfo<TItem>>();
            }
            else
            {
                throw;
            }
        }
        finally
        {
            serviceLock.Release();
            StateHasChanged();
        }

        bool AllItemsCached(int start, int count) => InternalItems.Skip(start).Take(count).All(e => e.IsLoaded);
        int PageFor(int index) => index / ItemsService.PageSize;
        int FirstItemOnPage(int page) => ItemsService.PageSize * page;
    }

    /// <summary>
    /// Tries to refresh a specific item in the deck.
    /// </summary>
    public bool TryRefreshItem(TItem updatedItem, IEqualityComparer<TItem>? comparer = null)
    {
        var itemInfo = (comparer, updatedItem) switch
        {
            (var c, _) when c is not null => InternalItems.FirstOrDefault(itemInfo => c.Equals(itemInfo.Item, updatedItem)),
            (_, var item) when item is IUniqueIdentifier uniquelyIdentifiableItem => InternalItems.FirstOrDefault(itemInfo => (itemInfo.Item as IUniqueIdentifier)?.Uuid == uniquelyIdentifiableItem.Uuid),
            (_, _) => throw new DryException($"Refreshing requires {nameof(TItem)} to implement {nameof(IUniqueIdentifier)} or an {nameof(IEqualityComparer)} to be provided.")
        };
        
        if (itemInfo == null)
        {
            return false;
        }
        
        itemInfo.Item = updatedItem;
        return true;
    }

    /// <summary>
    /// Tries to remove a specific item from the deck.
    /// </summary>
    public async Task<bool> TryRemoveItemAsync(TItem removedItem, IEqualityComparer<TItem>? comparer = null)
    {
        var itemInfo = (comparer, removedItem) switch
        {
            (var c, _) when c is not null => InternalItems.FirstOrDefault(itemInfo => c.Equals(itemInfo.Item, removedItem)),
            (_, var item) when item is IUniqueIdentifier uniquelyIdentifiableItem => InternalItems.FirstOrDefault(itemInfo => (itemInfo.Item as IUniqueIdentifier)?.Uuid == uniquelyIdentifiableItem.Uuid),
            (_, _) => throw new DryException($"Removing requires {nameof(TItem)} to implement {nameof(IUniqueIdentifier)} or an {nameof(IEqualityComparer)} to be provided.")
        };
        
        if (itemInfo == null)
        {
            return false;
        }

        changing = true;
        StateHasChanged();
        var removed = InternalItems.Remove(itemInfo);
        
        if (VirtualContainer != null)
        {
            await VirtualContainer.RefreshDataAsync();
        }
        
        changing = false;
        StateHasChanged();

        return removed;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        // Disconnect from events if accessors exist
        if (SelectionAccessor != null)
        {
            SelectionAccessor.SelectionSet.Changed -= Selection_Changed;
            SelectionAccessor = null;
        }
        
        if (QueryBuilderAccessor != null)
        {
            QueryBuilderAccessor.QueryBuilder.OnChanged -= Query_Changed;
            QueryBuilderAccessor = null;
        }
        
        serviceLock.Dispose();
    }
}
