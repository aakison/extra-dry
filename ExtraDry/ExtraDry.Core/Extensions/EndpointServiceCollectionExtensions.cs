using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ExtraDry.Core;

/// <summary>
/// Provides extension methods to simplify the registration of Extra Dry clients to the service
/// collection for dependency injection.
/// </summary>
/// <remarks>
/// While AddCrudClient is defined in ExtraDry.Core, AddListClient is defined separately in
/// ExtraDry.Blazor and ExtraDry.Server so that Blazor specific extensions are registered only in
/// Blazor projects.
/// </remarks>
public static class EndpointServiceCollectionExtensions
{

    /// <summary>
    /// Adds a strongly typed <see cref="CrudClient{T}" /> to the service collection. See <see
    /// cref="AddCrudClient{T}(IServiceCollection, Action{CrudClientOptions{T}})" /> for additional
    /// options. Particlularly useful for specifying the HttpClient to use in multi- tenant
    /// deployments.
    /// </summary>
    public static IServiceCollection AddCrudClient<T>(this IServiceCollection services, string endpointTemplate)
        where T : notnull
    {
        services.AddCrudClient<T>(options => {
            options.CrudEndpoint = endpointTemplate;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="CrudClient{T}" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddCrudClient<T>(this IServiceCollection services, Action<CrudClientOptions<T>> config)
        where T : notnull
    {
        var options = new CrudClientOptions<T>();
        config(options);

        DataValidator.ThrowIfInvalid(options);

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<CrudClient<T>>>();
            var service = new CrudClient<T>(client, options, logger);
            return service;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="CrudClient{T}" /> to the service collection.
    /// </summary>
    public static IServiceCollection AddKeyedCrudClient<T>(this IServiceCollection services, string key, Action<CrudClientOptions<T>> config)
        where T : notnull
    {
        var options = new CrudClientOptions<T>();
        config(options);

        DataValidator.ThrowIfInvalid(options);

        services.AddKeyedScoped(key, (e, key) => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<CrudClient<T>>>();
            var service = new CrudClient<T>(client, options, logger);
            return service;
        });
        return services;
    }

    /// <summary>
    /// Adds a strongly typed <see cref="BlobClient{TBlob}" /> to the service collection. Use with
    /// the built-in Blob class, or a custom class that implements <see cref="IBlob" />.
    /// </summary>
    public static IServiceCollection AddBlobClient<T>(this IServiceCollection services, string endpointTemplate) where T : IBlob, new()
    {
        services.AddBlobClient<T>(options => {
            options.BlobEndpoint = endpointTemplate;
        });
        return services;
    }

    /// <inheritdoc cref="AddBlobClient{T}(IServiceCollection, string)" />
    public static IServiceCollection AddBlobClient<T>(this IServiceCollection services, Action<BlobClientOptions> config) where T : IBlob, new()
    {
        var options = new BlobClientOptions();
        config(options);

        var validator = new DataValidator();
        validator.ValidateObject(options);
        validator.ThrowIfInvalid();

        services.AddScoped(e => {
            var client = GetHttpClient(e, options);
            var logger = e.GetRequiredService<ILogger<BlobClient<T>>>();
            var validator = e.GetService<FileValidationService>();
            var service = new BlobClient<T>(client, validator, options, logger);
            return service;
        });
        return services;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Will be obsolete when other methods are moved here (e.g. AddCrudClient)")]
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
