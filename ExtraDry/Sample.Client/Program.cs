using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
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

            var services = builder.Services;

            services.AddExtraDry();

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            services.AddFilteredListService<Sector>("/api/sectors");
            services.AddFilteredListService<Company>("/api/companies");
            services.AddFilteredListService<Content>("/api/contents");
            services.AddFilteredListService<Region>("/api/regions");

            services.AddPagedListService<Employee>("/api/employees");

            services.AddCrudService<Sector>("/api/sectors");
            services.AddCrudService<Company>("/api/companies");
            services.AddCrudService<Content>("/api/contents");
            services.AddCrudService<Region>("/api/regions");

            services.AddScoped<IBlobService>(e => 
                new DryBlobService(e.GetService<HttpClient>(), "/api/blobs/{0}/{1}") {
                    Scope = BlobScope.Public,
                }
            );

            services.AddScoped<ISubjectViewModel<Employee>, EmployeeViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
