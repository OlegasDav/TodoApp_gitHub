using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddSingleton<IApiKeyService, ApiKeyService>();
        }
    }
}
