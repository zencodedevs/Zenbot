using Askmethat.Aspnet.JsonLocalizer.Localizer;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;
using Zen.Application;
using Zenbot.Application.Shared.Behaviours;
using Zenbot.Application.Shared.Localization;
using Zenbot.Domain.Shared.Common;

namespace Application.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationShared(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());

            //zen application
            services.AddZenApplication();

            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "Resources";
            });
            services.TryAddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.TryAddTransient<IStringLocalizer, JsonStringLocalizer>();
            CultureInfo.CurrentCulture = new CultureInfo(Constants.SystemCultureNames.Georgian);


            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));


            return services;
        }
    }
}
