using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sample.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Client {

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddFilteredListService<Sector>("/api/sectors");
            builder.Services.AddFilteredListService<Company>("/api/companies");
            builder.Services.AddFilteredListService<Content>("/api/contents");
            builder.Services.AddFilteredListService<Region>("/api/regions");

            builder.Services.AddPagedListService<Employee>("/api/employees");

            builder.Services.AddCrudService<Sector>("/api/sectors/{0}");
            builder.Services.AddCrudService<Company>("/api/companies/{0}");
            builder.Services.AddCrudService<Content>("/api/contents/{0}");
            builder.Services.AddCrudService<Region>("/api/regions/{0}");

            builder.Services.AddScoped<IBlobService>(e => 
                new DryBlobService(e.GetService<HttpClient>(), "/api/blobs/{0}/{1}") {
                    Scope = BlobScope.Public,
                }
            );

            await builder.Build().RunAsync();
        }
    }
}
