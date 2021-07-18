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


            builder.Services.AddScoped<IListService<Company>>(e =>
                new RestfulListService<FilteredCollection<Company>, Company>(e.GetService<HttpClient>(), "/api/companies"));

            builder.Services.AddScoped<IListService<Content>>(e =>
                new RestfulListService<FilteredCollection<Content>, Content>(e.GetService<HttpClient>(), "/api/contents"));

            builder.Services.AddScoped<IListService<Employee>>(e =>
                new RestfulListService<PagedCollection<Employee>, Employee>(e.GetService<HttpClient>(), "/api/employees"));

            builder.Services.AddScoped(e => new RestfulListService<FilteredCollection<Sector>, Sector>(e.GetService<HttpClient>(), "/api/sectors"));
            builder.Services.AddScoped<IListService<Sector>>(e => e.GetService<RestfulListService<FilteredCollection<Sector>, Sector>>());
            builder.Services.AddScoped<IOptionProvider<Sector>>(e => e.GetService<RestfulListService<FilteredCollection<Sector>, Sector>>());

            builder.Services.AddScoped(e =>
                new CrudService<Company>(e.GetService<HttpClient>(), "/api/companies/{0}"));

            builder.Services.AddScoped(e =>
                new CrudService<Content>(e.GetService<HttpClient>(), "/api/contents/{0}"));

            builder.Services.AddScoped<IBlobService>(e => 
                new DryBlobService(e.GetService<HttpClient>(), "/api/blobs/{0}/{1}") {
                    Scope = BlobScope.Public,
                }
            );

            await builder.Build().RunAsync();
        }
    }
}
