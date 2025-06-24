using ExtraDry.Server.Agents;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Sample.Components.Agent;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Collect options, all stored in AgentOptions for both dependency injection and local use.
builder.Services.Configure<AgentOptions>(builder.Configuration.GetSection(AgentOptions.SectionName));
builder.Services.AddSingleton(e => e.GetRequiredService<IOptions<AgentOptions>>().Value);
var options = builder.Configuration.GetSection(AgentOptions.SectionName).Get<AgentOptions>() ?? new();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
//builder.Services.AddHttpClient();

builder.Services.AddCronJob("*/10 * * * * *", () => Console.WriteLine("Hello second divisible by 10."));
//builder.Services.AddCronJob("* * * * *", "minutely", () => Console.WriteLine("Hello Again, Mr. Minute!"));
//builder.Services.AddCronJob<OptionsDisplayer>("*/3 * * * *");

// Configure MassTransit, see https://masstransit.io/quick-starts/in-memory
builder.Services.AddMassTransit(config => {
    config.SetKebabCaseEndpointNameFormatter();
    var assembly = Assembly.GetEntryAssembly();
    config.AddConsumers(assembly);

    if(options.ServiceBus == ServiceBusTransport.RabbitMQ) {
        config.UsingRabbitMq((context, cfg) => {
            cfg.Host(options.RabbitMQ!.Server, options.RabbitMQ.VirtualHost, host => {
                host.Username(options.RabbitMQ.Username);
                host.Password(options.RabbitMQ.Password);
            });
            cfg.ConfigureEndpoints(context);
        });
    }
    else if(options.ServiceBus == ServiceBusTransport.AzureServiceBus) {
        // Not used/supported in Sample.  
    }
    else if(options.ServiceBus == ServiceBusTransport.InMemory) {
        config.UsingInMemory((context, cfg) => {
            cfg.ConfigureEndpoints(context);
        });
    }
    else {
        throw new NotImplementedException("Unrecognized Service Bus, could not configure Mass Transit.");
    }
});

//builder.Services.AddHttpClient("api", options => {
//    options.BaseAddress = new Uri("https://localhost:7176");
//    options.DefaultRequestHeaders.Add("Accepts", "application/json");
//});

//builder.Services.AddListService<Tenant>(options => {
//    options.HttpClientName = "api";
//    options.ListEndpoint = "tenants";
//    options.ListMode = ListServiceMode.FullCollection;
//});

//builder.Services.AddHostedService<Worker>();

//builder.Services.AddHostedService<AgentService>();

builder.Services.AddHealthChecks()
    .AddCheck<AgentHealthCheck>("Agent");

var host = builder.Build();

// Configure the HTTP request pipeline.

if(host.Environment.IsDevelopment()) {
}

// HealthCheck middleware
host.UseHealthChecks("/healthcheck", new HealthCheckOptions() {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});
host.MapHealthChecks("/healthcheck");

host.UseHttpsRedirection();
host.UseAuthentication();
host.UseAuthorization();
host.MapControllers();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogProperties(options);

host.Run();
