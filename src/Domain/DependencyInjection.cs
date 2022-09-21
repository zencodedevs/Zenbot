using Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Zen.Domain;

namespace Zenbot.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddDomainShared();

            return services;
        }
    }
}
