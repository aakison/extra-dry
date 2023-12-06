using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Blazor;

/// <summary>
/// Provides extension methods to simplify the registration of Extra Dry services to the
/// service collection for dependency injection.
/// </summary>
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
    /// Adds a strongly typed <see cref="CrudService{T}" /> to the service collection.
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
    /// Adds a strongly typed <see cref="StatService{T}"/> to the service collection.
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
    /// Adds a strongly typed <see cref="ListService{TCollection, TItem}"/> that provides a 
    /// <see cref="FilteredCollection{T}"/> to the service collection.  Also registers the 
    /// service using the interfaces <see cref="IListService{T}"/> 
    /// and <see cref="IOptionProvider{T}"/>.
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
    /// Adds a strongly typed <see cref="ListService{TCollection, TItem}"/> to the service 
    /// collection.  Also registers the service using the interfaces <see cref="IListService{T}"/> 
    /// and <see cref="IOptionProvider{T}"/>.
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


    /// <summary>
    /// Adds a strongly typed <see cref="BlobService{TBlob}"/> to the service collection.  Use with
    /// the built-in Blob class, or a custom class that implements <see cref="IBlob"/>.
    /// </summary>
    public static IServiceCollection AddBlobService<T>(this IServiceCollection services, string endpointTemplate) where T : IBlob, new()
    {
        services.AddBlobService<T>(options => {
            options.BlobEndpoint = endpointTemplate;
        });
        return services;
    }

    /// <inheritdoc cref="AddBlobService{T}(IServiceCollection, string)"/>
    public static IServiceCollection AddBlobService<T>(this IServiceCollection services, Action<BlobServiceOptions> config) where T : IBlob, new()
    {
        var options = new BlobServiceOptions();
        config(options);

        var validator = new DataValidator();
        validator.ValidateObject(options);
        validator.ThrowIfInvalid();

        services.AddScoped(e => {
            var client = e.GetRequiredService<HttpClient>();
            var logger = e.GetRequiredService<ILogger<BlobService<T>>>();
            var validator = e.GetService<FileValidationService>();
            var service = new BlobService<T>(client, validator, options, logger);
            return service;
        });
        return services;
    }

}
