using ExtraDry.Core;
using ExtraDry.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sample.Data;
using Sample.Data.Services;
using Sample.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Server {
    public class Startup {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SampleContext>(opt => opt.UseInMemoryDatabase("sample"));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSwaggerGen(openapi => {
                openapi.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "Sample API",
                    Description = "A sample API for Blazor.ExtraDry",
                });
                foreach(var docfile in new string[] { "Sample.Shared.xml", "Sample.Server.xml" }) {
                    var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
                    openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
                }
            });

            services.AddScoped<EmployeeService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ContentsService>();
            services.AddScoped<SectorService>();
            services.AddScoped<BlobService>();
            services.AddScoped<RuleEngine>();
            
            services.AddScoped<IEntityResolver<Sector>>(e => e.GetService<SectorService>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SampleContext context)
        {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.InjectStylesheet("/css/swagger-ui-extensions.css");
                c.DocumentTitle = "Sample Blazor.ExtraDry APIs";
                c.EnableTryItOutByDefault();
            });
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();

                // Calls to API endpoints shouldn't fallback to Blazor
                endpoints.Map("api/{**slug}", context => { 
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return Task.CompletedTask;
                });
                endpoints.MapFallbackToFile("{**slug}", "index.html");
            });

            var sampleData = new DummyData();
            sampleData.PopulateServices(context);
            sampleData.PopulateCompanies(context, 50);
            sampleData.PopulateEmployees(context, 5000);
            sampleData.PopulateContents(context);
        }

    }
}
