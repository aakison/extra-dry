# blazor-extra-dry
Performant blazor client library extending DRY principles.

### Adding an Item Service

TODO:

### Adding an Item Controller

TODO:

### Adding a Razor List Item Page

Add a Razor page that lists items (examples assumt T is `Item`)

  1. Ensure HttpClient is registered in services, only needs to be done once.
    * E.g. 
  2. Add list provider into services.  
    * For short lists where all items are returned every time:
    ```cSharp
      builder.Services.AddScoped<IListService<Item>>(e => new RestfulListService<List<Item>, Item>(e.GetService<HttpClient>(), "/api/items"));
    ```
    * For longer lists where paging is required:
    ```cSharp
      builder.Services.AddScoped<IListService<Item>>(e => new RestfulListService<PartialCollection<Item>, Item>(e.GetService<HttpClient>(), "/api/items"));
    ```
    * Extra Dry just needs an IListService, so you can always roll your own:
    ```cSharp
      builder.Services.AddScoped<IListService<Item>, MyCustomItemLister>();
    ```
  3. Add a page to handle the request, e.g. ItemList.razor:
    ```cSharp
        @page "/items"
        @inject IListService<Item> ListService;

        <h2>Items</h2>

        <DryTable T="Item" ViewModel="@this" ItemsService="@ListService" />
    ```

### Adding a Razor Edit Item Page

TODO:

