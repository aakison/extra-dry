# Extra Dry - Blazor
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

     <DryTable TItem="Item" ViewModel="@this" ItemsService="@ListService" />
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

## Components in ExtraDry.Blazor
The ExtraDry.Blazor assembly contains blazor based components for composing user interfaces.  These
are broken into three types of components:

  * Standard - Traditional components that take parameters and bind to values.  Can be used without
    the rest of the framework.
  * DRY - Framework specific components that link to Models, ViewModels, and Properties.  Provide
    dynamic features with low-code by using Model annotations.
  * Internal - Components that are useful for composing other components but aren't intended for use
    by the end-user.  These are still public as required by the Blazor framework.

### Authoring Standard Components
When authoring standard components, ensure the following checklist is followed for framework
consistency:

  * Use a single root HTML element, semantic if possible, `div` otherwise.
  * Provide class names for the elements, favoring semantic naming:
    * On the root node, have a class name that is the kebab-case version of the control name (e.g.
      'tri-check' for TriCheck)
    * If the component binds to objects, include the kebab-case version of the type, e.g. when
      binding to a `Company` add a class of 'company'.
    * If the component has multiple representation states, include a semantic name for each state,
      e.g. if both single and multiple select options exist on the control, include 'single' or
      'multiple' classes.
    * Have `CssClass` property which merges with the base div class, e.g.
      ```
      [Parameter]
      public string CssClass { get; set; } = string.Empty;
      ```
    * Use `DataConverter.JoinNonEmpty` helper to consistently join classes in private member
      `CssClasses`, e.g.
      ```
      private string CssClasses => DataConverter.JoinNonEmpty("tri-check", CssClass);
      ```
  * For basic styling of components, use CSS Isolation at the component level.  
    * Prefer the use of SaSS and create the SCSS file with the same name as the component, followed
      by ".scss"
    * Create a build rule in compilerconfig.json to build .scss file to .css file.
    * Avoid using class names to identify elements, use positional elements and semantic tags.
      Doing so allows consumers to use the class names to override the default styles more easily
      (e.g. exactly the opposite of the way bootstrap does it.)

### ExtraDry.Analyzers Rules for Components

DRY1501 & DRY1502 - Public `CssClass` in Components.

Use the following pattern for CssClass to comply with these rules:

```Blazor
<div class="@CssClasses">...</div>

@code {
    /// <inheritdoc cref="IComments.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "tag-name", "semantic-name", CssClass);
}
```

DRY1503 & DRY1504 - Chain unmatched attribute to nested elements

```Blazor
<div @attributes="@UnmatchedAttributes">...</div>

@code {
    /// <inheritdoc cref="IComments.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;
}

```

#### Code Coverage

The coverlet collector has been added to the unit tests for the manual running of code coverage.  To run, install the following prerequisites.  These are global tools for dotnet core.  See https://github.com/danielpalme/ReportGenerator

```
dotnet tool install -g dotnet-reportgenerator-globaltool
```
or, just locally using:
```
dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
```

Once installed, run code coverage statistics using dotnet as:

```
dotnet test --collect:"XPlat Code Coverage"
```

This will store XML coverage files in the 'cobertura' file format.  These can be used by any tools that support that format, in particular the report generator that was installed in the global install steps above.

From the project root directory, run the following to collect the cobertura files and create a static website and browse it:

```
reportgenerator -reports:./*/TestResults/*/*.xml -targetdir:./TestCoverage
./TestCoverage/index.htm
```
