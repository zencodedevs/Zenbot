using BotCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using NLog;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using Zenbot.Application;
using Zenbot.Domain;
using Zenbot.Domain.Shared.Common;
using Zenbot.Infrastructure;
using Zenbot.Infrastructure.Shared.Persistence;
using Zenbot.WebUI.Controllers;
using Zenbot.WebUI.CurrentTenantMiddlewares;
using Zenbot.WebUI.DiscordOAuth;
using Zenbot.WebUI.Filters;
using Zenbot.WebUI.Helpers;
using Zenbot.WebUI.Middlewares;
using Zenbot.WebUI.Processors;

namespace Zenbot.WebUI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //set nlog connection string
            GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));

            //set nlog inster clause variable
            LogManager.Configuration.Variables["registerClause"] = Constants.Nlog.WebUiDbRegisterClause;

            // Adding services from Discord bot to start the bot from this Layer
            var bot = new BotService().ConfigServices(services);

            services.AddSingleton(bot);

            services.AddDomain();
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddWebUi();

            // add microsoft feature managment
            services.AddFeatureManagement();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation();

            services.AddRazorPages();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Constants.SettingsSecurityPolicy,
            //        policy => policy.RequireClaim(Permissions.PermissionType.SettingPerm));
            //});

            services.AddAuthentication(DiscordDefaults.AuthenticationScheme)
                .AddDiscord(options =>
                {
                    options.AppId = Configuration["Discord:AppId"];
                    options.AppSecret = Configuration["Discord:AppSecret"];
                    options.Scope.Add("guilds");
                });

            #region Open api  

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Zenbot Open API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                configure.OperationProcessors.Add(new SysLanguageHeaderOperationProcessor());
            });

            #endregion Open api  

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            ////
            //// Swagger versioning 
            ////
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddVersionedApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddMvc()
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = nameof(ErrorController.Code404).GetActionRoute(nameof(ErrorController));
                    await next();
                }
            });

            // Configuraiton with Discord bot
            await app.ApplicationServices.GetRequiredService<BotService>()
               .RunAsync(app.ApplicationServices);

            // app.UseExtensionCurrentTenantMiddleware();
            app.UseSysLanguageMiddleware();
            app.UseCurrentTenantMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler(nameof(ErrorController.Code500).GetActionRoute(nameof(ErrorController)));
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/swagger/v1/swagger.json";
                settings.DocExpansion = "list";
            });

            app.UseOpenApi(options =>
            {
                options.PostProcess = (document, request) =>
                {
                    // Patch server URL for Swagger UI
                    var prefix = string.Empty;
                    document.Servers.First().Url += prefix;
                };
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            #region Localization

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo(Constants.SystemCultureNames.English),
                    new CultureInfo(Constants.SystemCultureNames.Georgian)
                };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: Constants.SystemCultureNames.Georgian, uiCulture: Constants.SystemCultureNames.Georgian),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(localizationOptions);

            var requestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(0, requestProvider);
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            #endregion
        }
    }
}
