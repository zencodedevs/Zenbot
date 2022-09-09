using System;
using System.Linq;
using Zen.MultiTenancy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;

namespace ZenAchitecture.WebUI.Middlewares
{
    public class ExtensionCurrentTenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICurrentTenant _currentTenant;
        public ExtensionCurrentTenantMiddleware(RequestDelegate next, ICurrentTenant currentTenant)
        {
            _next = next;
            _currentTenant = currentTenant;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("client_id", out var _clientId);
            httpContext.Request.Headers.TryGetValue("authorization", out var _params);

            if (!_clientId.Any()) return _next(httpContext);

            if (_clientId.First() != "pointerSourceExtension") return _next(httpContext);

            if (_params.Any())
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
        public static IApplicationBuilder UseExtensionCurrentTenantMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExtensionCurrentTenantMiddleware>();
        }
    }
}
