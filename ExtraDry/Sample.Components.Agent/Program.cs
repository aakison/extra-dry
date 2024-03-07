using ExtraDry.Server.Agents;
using GettingStarted;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Components.Agent;
using System.Reflection;

Console.WriteLine("Hello, World!");

var builder = Host.CreateApplicationBuilder(args);

Console.WriteLine(builder.Configuration.GetValue<string>("DOTNET_ENVIRONMENT"));

// Collect options, all stored in AgentOptions for both dependency injection and local use.
builder.Services.Configure<AgentOptions>(builder.Configuration.GetSection(AgentOptions.SectionName));
builder.Services.AddSingleton(e => e.GetRequiredService<IOptions<AgentOptions>>().Value);
var options = builder.Configuration.GetSection(AgentOptions.SectionName).Get<AgentOptions>() ?? new();

builder.Services.AddHttpClient();

//builder.Services.AddCronJob("bad-cron-expression", () => Console.WriteLine("Should never see this."));
builder.Services.AddCronJob("*/5 * * * * *", () => Console.WriteLine("Hello at 5 seconds!"));
builder.Services.AddCronJob("* * * * *", "minutely", () => Console.WriteLine("Hello Again, Mr. Minute!"));
builder.Services.AddCronJob<OptionsDisplayer>("*/10 * * * * *");
//builder.Services.AddCronJob<CronTriggerHost>(options => options.Lifecycle = HostedLifecycle.Singleton);

// Configure MassTransit, see https://masstransit.io/quick-starts/in-memory
//builder.Services.AddMassTransit(config => {
//    //config.SetKebabCaseEndpointNameFormatter();
//    //config.SetInMemorySagaRepositoryProvider();
//    var assembly = Assembly.GetEntryAssembly();
//    config.AddConsumers(assembly);
//    //config.AddSagaStateMachines(assembly);
//    //config.AddSagas(assembly);
//    //config.AddActivities(assembly);

//    //config.UsingInMemory((context, cfg) => {
//    //    cfg.ConfigureEndpoints(context);
//    //});

//    if(options.ServiceBus == ServiceBusTransport.RabbitMQ) {
//        config.UsingRabbitMq((context, cfg) => {
//            cfg.Host(options.RabbitMQOptions!.Server, options.RabbitMQOptions.VirtualHost, host => {
//                host.Username(options.RabbitMQOptions.Username);
//                host.Password(options.RabbitMQOptions.Password);
//            });
//            cfg.ConfigureEndpoints(context);
//        });
//    }
//    else if(options.ServiceBus == ServiceBusTransport.AzureServiceBus) {

//    }
//    else if(options.ServiceBus == ServiceBusTransport.InMemory) {
//        config.UsingInMemory((context, cfg) => {
//            cfg.ConfigureEndpoints(context);
//        });
//    }
//    else {
//        throw new NotImplementedException("Unrecognized Transport, could not configure Service Bus");
//    }

//});

builder.Services.AddHttpClient("api", options => {
    options.BaseAddress = new Uri("https://localhost:7176");
    options.DefaultRequestHeaders.Add("Accepts", "application/json");
});

//builder.Services.AddHostedService<Worker>();

//builder.Services.AddHostedService<AgentService>();

IHost host = builder.Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
//var options = host.Services.GetRequiredService<AgentOptions>();
logger.LogProperties(options);
host.Run();
