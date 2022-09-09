using Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Zen.Domain;

namespace ZenAchitecture.Domain
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
