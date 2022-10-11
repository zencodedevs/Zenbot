using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Zenbot.WebUI.Extensions
{
    public static class CookieClaimsReader
    {
        public static long GetOperatorUserId(this HttpContext context)
        {
            return Convert.ToInt64(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetOperatorUsername(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetOperatorEmailAddress(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
