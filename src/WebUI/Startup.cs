using ZenAchitecture.Application;
using ZenAchitecture.Domain;
using ZenAchitecture.Infrastructure;
using ZenAchitecture.WebUI.CurrentTenantMiddlewares;
using ZenAchitecture.WebUI.Filters;
using ZenAchitecture.WebUI.Middlewares;
using ZenAchitecture.WebUI.Processors;
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
using ZenAchitecture.Domain.Shared.Common;
using ZenAchitecture.Infrastructure.Shared.Persistence;

namespace ZenAchitecture.WebUI
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

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.SettingsSecurityPolicy,
                    policy => policy.RequireClaim(Permissions.PermissionType.SettingPerm));
            });


            #region Open api  

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "ZenAchitecture Open API";
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

            // TO DO
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }


            app.UseSwaggerUi3( settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
                settings.DocExpansion = "list";

            });

            app.UseOpenApi(options =>
            {
                options.PostProcess = (document, request) =>
                {
                    // Patch server URL for Swagger UI
                    var prefix = "/api/v" + document.Info.Version.Split('.')[0];
                    document.Servers.First().Url += prefix;
                };
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    var config = Configuration["SpaBaseUrl"] ?? "http://localhost:4200";
                    spa.UseProxyToSpaDevelopmentServer(config);
                }
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
