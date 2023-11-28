using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExtraDry.Core;

public static class ServicesExtensions
{
    /// <summary>
    /// Registers the upload tools to services, able to be injected in future FileValidators
    /// </summary>
    public static void AddUploadService(this IServiceCollection services, UploadConfiguration options)
    {
        services.AddSingleton(new UploadService(options));
    }
}
