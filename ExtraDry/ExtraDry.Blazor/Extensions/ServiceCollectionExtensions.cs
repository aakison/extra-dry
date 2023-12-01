using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Blazor;

public static class ServiceCollectionExtensions {

    /// <summary>
    /// Add the core Extra Dry services to the Blazor application.
    /// </summary>
    public static IServiceCollection AddExtraDry(this IServiceCollection services)
    {
        services.AddScoped<ExtraDryJavascriptModule>();
        return services;
    }

    /// <summary>
    /// Adds a strongly typed CrudService`T to the service collection.
    /// </summary>
    public static IServiceCollection AddCrudService<T>(this IServiceCollection services, string endpointTemplate)
    {
        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<CrudService<T>>>();
            var service = new CrudService<T>(client, endpointTemplate, logger);
            return service;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed StatService`T to the service collection.
    /// </summary>
    public static IServiceCollection AddStatService<T>(this IServiceCollection services, string endpointTemplate)
    {
        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<StatService<T>>>();
            var service = new StatService<T>(client, endpointTemplate, logger);
            return service;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed ListService`FilteredCollection`T to the service collection.
    /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
    /// </summary>
    public static IServiceCollection AddFilteredListService<T>(this IServiceCollection services, string endpointTemplate) 
    {
        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<ListService<FilteredCollection<T>, T>>>();
            var service = new ListService<FilteredCollection<T>, T>(client, endpointTemplate, logger);
            return service;
        });
        services.AddScoped(e => {
            IListService<T> upcasted = e.GetRequiredService<ListService<FilteredCollection<T>, T>>();
            return upcasted;
        });
        services.AddScoped(e => {
            IOptionProvider<T> upcasted = e.GetRequiredService<ListService<FilteredCollection<T>, T>>();
            return upcasted;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed ListService`PagedCollection`T to the service collection.
    /// Also registered the service using the interfaces IListService`T and IOptionProvider`T.
    /// </summary>
    public static IServiceCollection AddPagedListService<T>(this IServiceCollection services, string endpointTemplate)
    {
        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<ListService<PagedCollection<T>, T>>>();
            var service = new ListService<PagedCollection<T>, T>(client, endpointTemplate, logger);
            return service;
        });
        services.AddScoped(e => {
            IListService<T> upcasted = e.GetRequiredService<ListService<PagedCollection<T>, T>>();
            return upcasted;
        });
        services.AddScoped(e => {
            IOptionProvider<T> upcasted = e.GetRequiredService<ListService<PagedCollection<T>, T>>();
            return upcasted;
        });
        return services;
    }


    public static IServiceCollection AddBlobService(this IServiceCollection services, string endpointTemplate)
    {
        services.AddBlobService(options => {
            options.BlobEndpoint = endpointTemplate;
        });
        return services;
    }

    public static IServiceCollection AddBlobService(this IServiceCollection services, Action<BlobServiceOptions> config)
    {
        var options = new BlobServiceOptions();
        config(options);

        var validator = new DataValidator();
        validator.ValidateObject(options);
        validator.ThrowIfInvalid();

        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<BlobService>>();
            var service = new BlobService(client, options, logger);
            return service;
        });
        return services;
    }

}
