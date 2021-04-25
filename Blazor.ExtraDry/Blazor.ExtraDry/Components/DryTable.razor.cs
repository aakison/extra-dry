using Blazor.ExtraDry.Components.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryTable<T> : ComponentBase, IDisposable {

        [Parameter]
        public object ViewModel { get; set; }

        [Parameter]
        public ICollection<T> Items { get; set; }

        [Parameter]
        public IListService<T> ItemsService { get; set; }

        /// <summary>
        /// Optional object that controls how items in the table are grouped.
        /// A typical use is to group children under their parents.
        /// </summary>
        [Parameter]
        public IGroupProvider<T> GroupProvider { get; set; }

        /// <summary>
        /// Optional name of a property to sort the table by.
        /// </summary>
        [Parameter]
        public string Sort { get; set; }

        /// <summary>
        /// If `Sort` specified, determines the order of the sort.
        /// </summary>
        [Parameter]
        public bool SortAscending { get; set; }

        [Parameter]
        public SelectionSet Selection { get; set; }

        private ViewModelDescription description;

        [Inject]
        private ILogger<DryTable<T>> Logger { get; set; }

        private bool HasCheckboxColumn => description.ListSelectMode == ListSelectMode.Multiple;

        private bool HasRadioColumn => description.ListSelectMode == ListSelectMode.Single;

        private bool HasCommandsColumn => description.ContextCommands.Any();

        private Virtualize<ListItemInfo<T>> VirtualContainer { get; set; }

        private SelectionSet resolvedSelection;

        private int ColumnCount => description.TableProperties.Count +
            ((HasCheckboxColumn || HasRadioColumn) ? 1 : 0) +
            (HasCommandsColumn ? 1 : 0);

        protected override void OnInitialized()
        {
            Logger.LogInformation("DryTable.OnInitialized");
            description = new ViewModelDescription(typeof(T), ViewModel);
        }

        protected override void OnParametersSet()
        {
            AssertItemsMutualExclusivity();
            resolvedSelection = Selection ?? SelectionSet.Lookup(ViewModel) ?? SelectionSet.Register(ViewModel);
            resolvedSelection.MultipleSelect = description.ListSelectMode == ListSelectMode.Multiple;
            resolvedSelection.Changed += ResolvedSelection_Changed;
        }

        private void ResolvedSelection_Changed(object sender, SelectionSetChangedEventArgs e)
        {
            // Checking/unchecking a row could affect the column checkbox...
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
            if(GroupProvider != null) {
                foreach(var item in InternalItems) {
                    FindGroup(item);
                }
                GroupBy();
            }
        }

        private void FindGroup(ListItemInfo<T> item)
        {
            var group = GroupProvider.GetGroup(item.Item);
            if(group == null) {
                return;
            }
            var wrapper = InternalItems.First(e => e.Item.Equals(group));
            if(wrapper.Group == null) {
                FindGroup(wrapper);
            }
            item.Group = wrapper;
            wrapper.IsGroup = true;
            item.GroupDepth = (wrapper?.GroupDepth ?? 0) + 1;
        }

        private void GroupBy()
        {
            var comparer = new GroupComparer<T>();
            InternalItems.Sort(comparer);
        }

        private bool AllSelected => resolvedSelection?.All() ?? false;

        private void ToggleSelectAll()
        {
            if(AllSelected) {
                resolvedSelection.Clear();
            }
            else {
                resolvedSelection.SelectAll();
            }
            //StateHasChanged();
        }

        private async Task SortBy(DryProperty property, bool reverseOrder = true)
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
                IComparer<ListItemInfo<T>> comparer = new ItemComparer<T>(property, SortAscending);
                if(GroupProvider != null) {
                    comparer = new GroupComparer<T>(comparer);
                }
                InternalItems.Sort(comparer);
            }
            else {
                // Server side sort, can't assume all items, clear them out and re-request.
                InternalItems.Clear();
                await VirtualContainer.RefreshDataAsync();
            }
        }

        private void Toggle(ListItemInfo<T> item)
        {
            Console.WriteLine($"Code {item.Item.GetHashCode()}");
            item.IsExpanded = !item.IsExpanded;
            StateHasChanged();
        }

        private ItemCollection<T> InternalItems { get; } = new ItemCollection<T>();

        private IEnumerable<ListItemInfo<T>> ShownItems => InternalItems.Where(e => e.IsShown);

        private async ValueTask<ItemsProviderResult<ListItemInfo<T>>> GetItemsAsync(ItemsProviderRequest request)
        {
            Logger.LogInformation(@"DryTable: Getting page of results, from index {0}, fetching {1}", request.StartIndex, request.Count);
            await serviceLock.WaitAsync();
            try {
                request.CancellationToken.ThrowIfCancellationRequested();
                if(!InternalItems.Any()) {
                    Logger.LogInformation("--Loading initial items from remote service...");
                    var firstPage = PageFor(request.StartIndex);
                    var firstIndex = FirstItemOnPage(firstPage);
                    var items = await ItemsService.GetItemsAsync(Sort, SortAscending, firstIndex, ItemsService.FetchSize);
                    var count = items.Items.Count();
                    var total = items.TotalItemCount;
                    InternalItems.AddRange(items.Items.Select(e => new ListItemInfo<T> { Item = e, IsLoaded = true }));
                    InternalItems.AddRange(Enumerable.Range(0, total - count).Select(e => new ListItemInfo<T>()));
                    Logger.LogInformation($"--Loaded items #0 to #{count} of {total}.");
                }
                if(AllItemsCached(request.StartIndex, request.Count)) {
                    Logger.LogInformation("--Returning cached results");
                    var count = Math.Min(request.Count, InternalItems.Count);
                    var items = InternalItems.GetRange(request.StartIndex, count);
                    return new ItemsProviderResult<ListItemInfo<T>>(items, InternalItems.Count);
                }
                else {
                    Logger.LogInformation("--Loading page of items from remote service...");
                    var firstPage = PageFor(request.StartIndex);
                    var lastPage = PageFor(request.StartIndex + request.Count);
                    for(int pageNumber = firstPage; pageNumber <= lastPage; ++pageNumber) {
                        var firstIndex = FirstItemOnPage(pageNumber);
                        if(!AllItemsCached(firstIndex, ItemsService.FetchSize)) {
                            var items = await ItemsService.GetItemsAsync(Sort, SortAscending, firstIndex, ItemsService.FetchSize);
                            var count = items.Items.Count();
                            var total = items.TotalItemCount;
                            var index = firstIndex;
                            foreach(var item in items.Items) {
                                var info = InternalItems[index++];
                                info.Item = item;
                                info.IsLoaded = true;
                            }
                            Logger.LogInformation($"--Loaded items #{firstIndex} to #{firstIndex + ItemsService.FetchSize} of {total}.");
                        }
                    }
                }
                if(InternalItems.Any()) {
                    var count = Math.Min(request.Count, InternalItems.Count);
                    var items = InternalItems.GetRange(request.StartIndex, count);
                    return new ItemsProviderResult<ListItemInfo<T>>(items, InternalItems.Count);
                }
                else {
                    var x = new ItemsProviderResult<ListItemInfo<T>>();
                    return x;
                }
            }
            catch(OperationCanceledException) {
                // KLUDGE: The CancellationTokenSource is initiationed in the Virtualize component, but it can't handle the exception.
                // Catch the exception here and return an empty result instead.  
                Logger.LogInformation("--Loading cancelled");
                return new ItemsProviderResult<ListItemInfo<T>>();
            }
            finally {
                serviceLock.Release();
            }

            bool AllItemsCached(int start, int count) => InternalItems.Skip(start).Take(count).All(e => e.IsLoaded);

            int PageFor(int index) => index / ItemsService.FetchSize;

            int FirstItemOnPage(int page) => ItemsService.FetchSize * page;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            resolvedSelection.Changed -= ResolvedSelection_Changed;
        }

        private readonly SemaphoreSlim serviceLock = new(1, 1);


    }

}
