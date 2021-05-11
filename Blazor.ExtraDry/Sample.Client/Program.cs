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

            builder.Services.AddScoped(e =>
                new CrudService<Company>(e.GetService<HttpClient>(), "/api/companies/{0}"));

            builder.Services.AddScoped<IListService<Company>>(e =>
                new RestfulListService<PagedCollection<Company>, Company>(e.GetService<HttpClient>(), "/api/companies"));

            await builder.Build().RunAsync();
        }
    }
}
