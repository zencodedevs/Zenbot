using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zen.EventProcessor;
using Zen.Infrastructure;
using Zen.Infrastructure.Interfaces;
using Zen.MultiTenancy;
using Zen.Uow;
using ZenAchitecture.Domain.Shared.Interfaces;
using ZenAchitecture.Infrastructure.Shared.Persistence;
using ZenAchitecture.Infrastructure.Shared.Services;

namespace Infrastructure.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureShared(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ZenAchitecture"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection"),
                       b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddZenEventProcessor();

            services.AddZenInfrastructure();

            services.AddZenMultiTenancy();

            services.RegisterUow();

            services.TryAddScoped<IAppDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.TryAddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
