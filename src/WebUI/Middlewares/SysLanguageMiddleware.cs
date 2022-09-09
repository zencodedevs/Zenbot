using ZenAchitecture.WebUI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using ZenAchitecture.Domain.Shared.Common;

namespace ZenAchitecture.WebUI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SysLanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public SysLanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            CultureInfo culture = new CultureInfo(Constants.SystemCultureNames.Georgian);
            if (httpContext.Request.GetSysLanguage() != null)
                culture = new CultureInfo(httpContext.Request.GetSysLanguage());
            else if (httpContext.Request.Query["lang"].ToString() != Constants.NullValues.StringNullValue)
            {
                var targetLang = httpContext.Request.Query["lang"].ToString();
                if (targetLang == "en")
                    culture = new CultureInfo(Constants.SystemCultureNames.English);
                else if (targetLang == "ka")
                    culture = new CultureInfo(Constants.SystemCultureNames.Georgian);
            }
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SysLanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseSysLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SysLanguageMiddleware>();
        }
    }
}
