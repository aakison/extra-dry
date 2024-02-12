using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sample.Spa.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Console.WriteLine("Huzzah");

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

services.AddCrudService<Sector>("/api/sectors");
services.AddCrudService<Company>("/api/companies");
services.AddCrudService<Content>("/api/contents");
services.AddCrudService<Region>("/api/regions");

services.AddBlobService<Blob>(config => {
    config.BlobEndpoint = "/api/blobs";
    config.MaxBlobSize = 1 * 1024 * 1024;
});

services.AddScoped<ISubjectViewModel<Employee>, EmployeeViewModel>();

builder.Services.AddScoped<AppViewModel>();

Console.WriteLine("Huzzah 2");

services.AddFileValidation();

await builder.Build().RunAsync();
