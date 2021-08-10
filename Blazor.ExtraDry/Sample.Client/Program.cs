using Blazor.ExtraDry;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddFilteredListService<Company>("/api/companies");
            builder.Services.AddFilteredListService<Content>("/api/contents");
            builder.Services.AddFilteredListService<Sector>("/api/sectors");

            builder.Services.AddPagedListService<Employee>("/api/employees");

            builder.Services.AddCrudService<Company>("/api/companies/{0}");
            builder.Services.AddCrudService<Content>("/api/contents/{0}");

            builder.Services.AddScoped<IBlobService>(e => 
                new DryBlobService(e.GetService<HttpClient>(), "/api/blobs/{0}/{1}") {
                    Scope = BlobScope.Public,
                }
            );

            await builder.Build().RunAsync();
        }
    }
}
