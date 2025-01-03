using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Core;

/// <summary>
/// Service collection extensions to simplify the registration of services that are common to both
/// Blazor and MVC server-side applications.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="FileValidationService" /> and the <see cref="FileValidator" />
    /// component to enforce good file hygiene when uploading files from the web.
    /// </summary>
    public static void AddFileValidation(this IServiceCollection services, Action<FileValidationOptions>? config = null)
    {
        var options = new FileValidationOptions();
        config?.Invoke(options);
        services.AddSingleton(e => options);
        services.AddSingleton<FileValidationService>();
        services.AddTransient<FileValidator>();
    }
}
