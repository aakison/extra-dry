using ExtraDry.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sample.Shared;

namespace Sample.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

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
        services.AddFilteredListService<Content>("/api/contents");

        services.AddListService<Region>(options => { 
            options.ListEndpoint = "/api/regions";
            options.HierarchyEndpoint = "/api/regions/hierarchy";
            options.HierarchyMethod = HttpMethod.Get;
            options.HttpClientName = "api";
        });

        services.AddStatService<Sector>("/api/sectors/stats");

        services.AddPagedListService<Employee>("/api/employees");

        services.AddCrudService<Sector>("/api/sectors");
        services.AddCrudService<Company>("/api/companies");
        services.AddCrudService<Content>("/api/contents");
        services.AddCrudService<Region>("/api/regions");

        services.AddScoped<IBlobService>(e => 
            new DryBlobService(e.GetRequiredService<HttpClient>(), "/api/blobs/{0}/{1}", e.GetRequiredService<ILogger<DryBlobService>>()) {
                Scope = BlobScope.Public,
            }
        );

        services.AddFileValidation();

        services.AddScoped<ISubjectViewModel<Employee>, EmployeeViewModel>();

        services.AddScoped<AppViewModel>();

        await builder.Build().RunAsync();
    }
}
