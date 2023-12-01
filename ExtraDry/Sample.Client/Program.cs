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

        var services = builder.Services;

        services.AddExtraDry();

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        services.AddFilteredListService<Sector>("/api/sectors");
        services.AddFilteredListService<Company>("/api/companies");
        services.AddFilteredListService<Content>("/api/contents");
        services.AddFilteredListService<Region>("/api/regions");

        services.AddStatService<Sector>("/api/sectors/stats");

        services.AddPagedListService<Employee>("/api/employees");

        services.AddCrudService<Sector>("/api/sectors");
        services.AddCrudService<Company>("/api/companies");
        services.AddCrudService<Content>("/api/contents");
        services.AddCrudService<Region>("/api/regions");

        services.AddScoped<IBlobService>(e => 
            new DryBlobService(e.GetRequiredService<HttpClient>(), "/api/blobs/{0}/{1}") {
                Scope = BlobScope.Public,
            }
        );
        services.AddBlobService("/api/blobs");

        services.AddScoped<ISubjectViewModel<Employee>, EmployeeViewModel>();

        services.AddScoped<AppViewModel>();

        services.AddFileValidation();

        await builder.Build().RunAsync();
    }
}
