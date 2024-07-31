using Microsoft.Extensions.DependencyInjection;

namespace ExtraDry.Server.Internal
{
    /// <summary>
    /// Provides access to the service provider for access from static extension methods. 
    /// Prefer use of established DI principles over this. 
    /// </summary>
    internal static class StaticServiceProvider
    {
        public static void Initialize(IServiceCollection serviceCollection)
        {
            Provider ??= serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider? Provider { get; private set; }
    }
}
