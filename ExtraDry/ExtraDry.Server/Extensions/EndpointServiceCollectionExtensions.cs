using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtraDry.Server;

/// <summary>
/// Provides extension methods to simplify the registration of Extra Dry services to the service
/// collection for dependency injection.
/// </summary>
/// <remarks>
/// AddListClient is defined in ExtrayDry.Server while AddCrudClient is defined in ExtraDry.Core.
/// The list clients for Blazor register UI specific features not used in server projects.
/// </remarks>
public static class EndpointServiceCollectionExtensions
{
    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> that provides a <see
    /// cref="FilteredCollection{T}" /> to the service collection. Also registers the service using
    /// the interfaces <see cref="IListClient{T}" /> and <see cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddFilteredListClient<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListClient<T>(options => {
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
    public static IServiceCollection AddSortedListClient<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListClient<T>(options => {
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
    public static IServiceCollection AddPagedListClient<T>(this IServiceCollection services, string endpoint)
    {
        services.AddListClient<T>(options => {
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
    public static IServiceCollection AddListClient<T>(this IServiceCollection services, Action<ListClientOptions> config)
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

        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="ListClient{TItem}" /> to the service collection. Also
    /// registers the service using the interfaces <see cref="IListClient{T}" /> and <see
    /// cref="IOptionProvider{T}" />.
    /// </summary>
    public static IServiceCollection AddKeyedListClient<T>(this IServiceCollection services, string key, Action<ListClientOptions> config)
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

        return services;
    }

    private static HttpClient GetHttpClient(IServiceProvider provider, ListClientOptions options)
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
