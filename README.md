# blazor-extra-dry
Performant blazor client library extending DRY principles.

### Adding an Item Service

TODO:

### Adding an Item Controller

TODO:

### Adding a Razor List Item Page

Add a Razor page component that lists items (examples assume T is `Item`)

  1. Ensure `HttpClient` is registered in services, only needs to be done once.
  2. In `Program.ConfigureServices`, add list provider into services.  
     * For short lists where all items are returned every time:
       ```cSharp
       builder.Services.AddScoped<IListService<Item>>(e => new RestfulListService<List<Item>, Item>(e.GetService<HttpClient>(), "/api/items"));
       ```
     * Or, for longer lists where paging is required, you can use `PartialCollection`:
       ```cSharp
       builder.Services.AddScoped<IListService<Item>>(e => new RestfulListService<PartialCollection<Item>, Item> (e.GetService<HttpClient>(), "/api/items"));
       ```
     * Or, Extra Dry just needs an `IListService`, so you can always roll your own and register it instead:
       ```cSharp
       builder.Services.AddScoped<IListService<Item>, MyCustomItemLister>();
       ```
  3. Add a page to handle the request, e.g. ItemList.razor:
     ```html
     @page "/items"
     @inject IListService<Item> ListService;

     <h2>Items</h2>

     <DryTable T="Item" ViewModel="@this" ItemsService="@ListService" />
      ```

### Adding a Razor Edit Item Page

Add a Razor page component that edits the item (examples assume T is `Item`)

  1. Ensure `HttpClient` is registered in services, only needs to be done once.
  2. In `Program.ConfigureServices` add service to retrieve and update the `Item`, for example using `CrudService<Item>`:
     ```cSharp
     builder.Services.AddScoped(e => new CrudService<Item>(e.GetService<HttpClient>(), "/api/items/{0}"));
     ```
  3. Add a page to edit the item, e.g. ItemEdit.razor:
     ```html
     @page "/companies/{uniqueId:guid}"
     @inject CrudService<Company> Service

     <h3>Company</h3>

     <DryForm ViewModel="@this" Model="@Company" />

     @code {

         [Parameter]
         public Guid UniqueId { get; set; }

         private Company Company { get; set; }

         protected override async Task OnInitializedAsync()
         {
             Company = await Service.RetrieveAsync(UniqueId);
         }

     }

     ```

