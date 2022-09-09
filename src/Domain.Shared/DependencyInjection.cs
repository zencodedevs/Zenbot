using Microsoft.Extensions.DependencyInjection;
using Zen.Domain;
namespace Domain.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainShared(this IServiceCollection services)
        {

            services.AddZenDomain();

            return services;
        }
    }
}
