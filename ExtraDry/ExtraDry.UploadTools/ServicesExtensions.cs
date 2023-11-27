using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExtraDry.UploadTools
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Registers the upload tools to services, able to be injected in future FileValidators
        /// </summary>
        public static void AddUploadService(this IServiceCollection services, IOptions<UploadConfiguration> options)
        {
            var fileService = new FileService();
            services.AddSingleton(new UploadTools(fileService, options.Value));
        }
    }
}
