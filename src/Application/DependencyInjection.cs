using Application.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Zenbot.Application.Common.Behaviours;
using Zenbot.Application.Common.Services;
using Zenbot.Domain.Interfaces;

namespace Zenbot.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddApplicationShared(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());


            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            //app services
            services.AddTransient<IActivityService, ActivityService>();

            return services;
        }
    }
}
