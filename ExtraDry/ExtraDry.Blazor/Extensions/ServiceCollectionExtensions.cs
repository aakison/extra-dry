using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Blazor;

/// <summary>
/// Provides extension methods to simplify the registration of Extra Dry services to the service
/// collection for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the core Extra Dry services to the Blazor application.
    /// </summary>
    public static IServiceCollection AddExtraDry(this IServiceCollection services)
    {
        services.AddScoped<ExtraDryJavascriptModule>();
        return services;
    }







    /// <summary>
    /// Adds a strongly typed <see cref="StatService{T}" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddStatService<T>(this IServiceCollection services, string endpointTemplate)
    {
        services.AddStatService<T>(options => {
            options.StatEndpoint = endpointTemplate;
        });
        return services;
    }

    public static IServiceCollection AddStatService<T>(this IServiceCollection services, Action<StatServiceOptions> config)
    {
        var options = new StatServiceOptions();
        config(options);

        DataValidator.ThrowIfInvalid(options);

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<StatService<T>>>();
            var service = new StatService<T>(client, options, logger);
            return service;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> that provides a <see
    /// cref="FilteredCollection{T}" /> to the service collection. Also registers the service using
    /// the interfaces <see cref="IListClient{T}" /> and <see cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddFilteredListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListClientMode.Filter;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> that provides a <see
    /// cref="SortedCollection{T}" /> to the service collection. Also registers the service using
    /// the interfaces <see cref="IListClient{T}" /> and <see cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddSortedListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListClientMode.FilterAndSort;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> to the service collection. Also
    /// registers the service using the interfaces <see cref="IListClient{T}" /> and <see
    /// cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddPagedListService<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListService<T>(options => {
            options.ListEndpoint = endpoint;
            options.ListMode = ListClientMode.FilterSortAndPage;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> to the service collection. Also
    /// registers the service using the interfaces <see cref="IListClient{T}" /> and <see
    /// cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddListService<T>(this IServiceCollection services, Action<ListClientOptions> config)
    {
        var options = new ListClientOptions();
        config(options);

        new DataValidator().ValidateObject(options);

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<ListClient<T>>>();
            var service = new ListClient<T>(client, options, logger);
            return service;
        });
        services.AddScoped(e => {
            IListClient<T> upcasted = e.GetRequiredService<ListClient<T>>();
            return upcasted;
        });
        services.AddScoped(e => {
            IOptionProvider<T> upcasted = new ListServiceOptionProvider<T>(e.GetRequiredService<ListClient<T>>());
            return upcasted;
        });

        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> to the service collection. Also
    /// registers the service using the interfaces <see cref="IListClient{T}" /> and <see
    /// cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddKeyedListService<T>(this IServiceCollection services, string key, Action<ListClientOptions> config)
    {
        var options = new ListClientOptions();
        config(options);

        new DataValidator().ValidateObject(options);

        services.AddKeyedScoped(key, (e, key) => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<ListClient<T>>>();
            var service = new ListClient<T>(client, options, logger);
            return service;
        });
        services.AddKeyedScoped(key, (e, key) => {
            IListClient<T> upcasted = e.GetRequiredService<ListClient<T>>();
            return upcasted;
        });
        services.AddKeyedScoped(key, (e, key) => {
            IOptionProvider<T> upcasted = new ListServiceOptionProvider<T>(e.GetRequiredService<ListClient<T>>());
            return upcasted;
        });

        return services;
    }


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
