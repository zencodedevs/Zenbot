using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Zen.MultiTenancy;

namespace ZenAchitecture.WebUI.CurrentTenantMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CurrentTenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICurrentTenant _currentTenant;


        public CurrentTenantMiddleware(RequestDelegate next, ICurrentTenant currentTenant)
        {
            _next = next;
            _currentTenant = currentTenant;
        }

        public Task Invoke(HttpContext httpContext)
        {

            httpContext.Request.Headers.TryGetValue("client_id", out var _clientId);
            httpContext.Request.Headers.TryGetValue("authorization", out var _params);

            if (_params.Any() && !_clientId.Any())
            {
                var jwt = _params.First().Replace("Bearer ", string.Empty).Replace("bearer ", string.Empty);
                SecurityToken jsonToken = default;
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    jsonToken = handler.ReadToken(jwt);
                }
                catch
                {
                    return _next(httpContext);
                }

                JwtSecurityToken tokenProfile = jsonToken as JwtSecurityToken;

                var targetClaim = tokenProfile.Claims.FirstOrDefault(c => c.Type == "x-tenant-id");

                if (!string.IsNullOrEmpty(targetClaim?.Value))
                {
                    _currentTenant.Change(new Guid(targetClaim.Value));
                }

            }


            return _next(httpContext);
        }
    }

    // Extension method used to add the CurrentTenantMiddleware to the HTTP request pipeline.
    public static class CurrentTenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentTenantMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentTenantMiddleware>();
        }
    }
}
