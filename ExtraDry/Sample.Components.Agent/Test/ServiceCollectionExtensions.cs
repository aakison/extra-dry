using ExtraDry.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Blazor.Extensions;

/// <summary>
/// Provides extension methods to simplify the registration of Extra Dry services to the
/// service collection for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions {

    ///// <summary>
    ///// Add the core Extra Dry services to the Blazor application.
    ///// </summary>
    //public static IServiceCollection AddExtraDry(this IServiceCollection services)
    //{
    //    services.AddScoped<ExtraDryJavascriptModule>();
    //    return services;
    //}

    /// <summary>
    /// Adds a strongly typed <see cref="CrudService{T}" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddCrudService<T>(this IServiceCollection services, string endpointTemplate)
    {
        services.AddCrudService<T>(options => {
            options.CrudEndpoint = endpointTemplate;
        });
        return services;
    }

    public static IServiceCollection AddCrudService<T>(this IServiceCollection services, Action<CrudServiceOptions> config)
    {
        var options = new CrudServiceOptions();
        config(options);

        new DataValidator().ValidateObject(options);

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<CrudService<T>>>();
            var service = new CrudService<T>(client, options, logger);
            return service;
        });
        return services;
    }

    ///// <summary>
    ///// Adds a strongly typed <see cref="StatService{T}"/> to the service collection.
    ///// </summary>
    //public static IServiceCollection AddStatService<T>(this IServiceCollection services, string endpointTemplate)
    //{
    //    services.AddStatService<T>(options => {
    //        options.StatEndpoint = endpointTemplate;
    //    });
    //    return services;
    //}

    //public static IServiceCollection AddStatService<T>(this IServiceCollection services, Action<StatServiceOptions> config)
    //{
    //    var options = new StatServiceOptions();
    //    config(options);

    //    new DataValidator().ValidateObject(options);

    //    services.AddScoped(e => {
    //        var client = GetHttpClient(e, options);
    //        var logger = e.GetRequiredService<ILogger<StatService<T>>>();
    //        var service = new StatService<T>(client, options, logger);
    //        return service;
    //    });
    //    return services;
    //}

    /// <summary>
    /// Adds a strongly typed <see cref="ListService{TItem}"/> that provides a 
    /// <see cref="FilteredCollection{T}"/> to the service collection.  Also registers the 
    /// service using the interfaces <see cref="IListService{T}"/> 
    /// and <see cref="IOptionProvider{T}"/>.
    /// </summary>
    public static IServiceCollection AddFilteredListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.Filter;
        });
        return services;
    }

    public static IServiceCollection AddFilteredListService<T, THttpClient>(this IServiceCollection services, string endpoint)
        where THttpClient : HttpClient
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.Filter;
            options.HttpClientType = typeof(THttpClient);
        });
        return services;
    }

    public static IServiceCollection AddSortedListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.FilterAndSort;
        });
        return services;
    }

    public static IServiceCollection AddSortedListService<T, THttpClient>(this IServiceCollection services, string endpoint)
        where THttpClient : HttpClient
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.FilterAndSort;
            options.HttpClientType = typeof(THttpClient);
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListService{TItem}"/> to the service 
    /// collection.  Also registers the service using the interfaces <see cref="IListService{T}"/> 
    /// and <see cref="IOptionProvider{T}"/>.
    /// </summary>
    public static IServiceCollection AddPagedListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.FilterSortAndPage;
        });
        return services;
    }

    public static IServiceCollection AddPagedListService<T, THttpClient>(this IServiceCollection services, string endpoint)
        where THttpClient : HttpClient
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListServiceMode.FilterSortAndPage;
            options.HttpClientType = typeof(THttpClient);
        });
        return services;
    }

    public static IServiceCollection AddListService<T>(this IServiceCollection services, Action<ListServiceOptions> config)
    {
        var options = new ListServiceOptions();
        config(options);

        new DataValidator().ValidateObject(options);

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<ListService<T>>>();
            var service = new ListService<T>(client, options, logger);
            return service;
        });
        services.AddScoped(e => {
            IListService<T> upcasted = e.GetRequiredService<ListService<T>>();
            return upcasted;
        });
        services.AddScoped(e => {
            IOptionProvider<T> upcasted = e.GetRequiredService<ListService<T>>();
            return upcasted;
        });

        return services;
    }


    ///// <summary>
    ///// Adds a strongly typed <see cref="BlobService{TBlob}"/> to the service collection.  Use with
    ///// the built-in Blob class, or a custom class that implements <see cref="IBlob"/>.
    ///// </summary>
    //public static IServiceCollection AddBlobService<T>(this IServiceCollection services, string endpointTemplate) where T : IBlob, new()
    //{
    //    services.AddBlobService<T>(options => {
    //        options.BlobEndpoint = endpointTemplate;
    //    });
    //    return services;
    //}

    ///// <inheritdoc cref="AddBlobService{T}(IServiceCollection, string)"/>
    //public static IServiceCollection AddBlobService<T>(this IServiceCollection services, Action<BlobServiceOptions> config) where T : IBlob, new()
    //{
    //    var options = new BlobServiceOptions();
    //    config(options);

    //    var validator = new DataValidator();
    //    validator.ValidateObject(options);
    //    validator.ThrowIfInvalid();

    //    services.AddScoped(e => {
    //        var client = e.GetRequiredService<HttpClient>();
    //        var logger = e.GetRequiredService<ILogger<BlobService<T>>>();
    //        var validator = e.GetService<FileValidationService>();
    //        var service = new BlobService<T>(client, validator, options, logger);
    //        return service;
    //    });
    //    return services;
    //}


    private static HttpClient GetHttpClient(IServiceProvider provider, IHttpClientOptions options)
    {
        if(options.HttpClientType != null) {
            // Validation above ensures convertible to HttpClient
            return (provider.GetRequiredService(options.HttpClientType) as HttpClient)!;
        }
        else if(options.HttpClientName != string.Empty) {
            return provider.GetRequiredService<IHttpClientFactory>().CreateClient(options.HttpClientName);
        }
        else {
            return provider.GetRequiredService<HttpClient>();
        }
    }

}
