using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtraDry.UploadTools
{
    public static class ServicesExtensions
    {
        public static void AddUploadService(this IServiceCollection services)
        {
            services.AddSingleton<UploadTools>();
        }
    }
}
