using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server;

public static class ExtraDryServiceCollectionExtensions {

    /// <summary>
    /// Add the Core ExtraDry functionality to the server side application
    /// </summary>
    public static IServiceCollection AddExtraDry(this IServiceCollection service, Action<ExtraDryOptions>? config = null)
    {
        service.AddSingleton(provider => {
            var options = new ExtraDryOptions();
            var configuration = provider.GetRequiredService<IConfiguration>();
            configuration.GetSection(ExtraDryOptions.SectionName).Bind(options);
            config?.Invoke(options);

            // manually inject where DI doesn't work
            QueryableExtensions.Options = options; 

            return options;
        });

        return service;
    }
}
