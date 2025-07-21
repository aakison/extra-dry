using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sample.Spa.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddFilter("Microsoft.AspNetCore.Components.RenderTree.*", LogLevel.None);

var services = builder.Services;

services.AddExtraDry();

services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

services.AddHttpClient("api", client => {
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

services.AddFilteredListService<Sector>("/api/sectors");

services.AddFilteredListService<Company>("/api/companies");
services.AddPagedListService<Employee>("/api/employees");

services.AddFilteredListService<Content>("/api/contents");

services.AddListService<Region>(options => {
    options.ListEndpoint = "/api/regions";
    options.HierarchyEndpoint = "/api/regions/hierarchy";
    options.HierarchyMethod = HttpMethod.Get;
    options.HttpClientName = "api";
});

services.AddStatService<Sector>("/api/sectors/stats");

services.AddCrudClient<Sector>("/api/sectors");
services.AddCrudClient<Company>("/api/companies");
services.AddCrudClient<Content>("/api/contents");
services.AddCrudClient<Region>("/api/regions");
services.AddCrudClient<Employee>("/api/employees");

services.AddBlobClient<Blob>(config => {
    config.BlobEndpoint = "/api/blobs";
    config.MaxBlobSize = 1 * 1024 * 1024;
});

services.AddScoped<ISubjectViewModel<Employee>, EmployeeViewModel>();
services.AddScoped<IDisplayNameProvider, DisplayNameProvider>();

builder.Services.AddScoped<AppViewModel>();

services.AddFileValidation();

await builder.Build().RunAsync();
