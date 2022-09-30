using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Zenbot.WebUI.Helpers
{
    public static class RoutingHelper
    {
        public static string GetControllerRoute(this string controllerName)
        {
            return controllerName.Replace(nameof(Controller), string.Empty);
        }

        public static string GetActionRoute(this string actionName, string controllerName)
        {
            return $"/{controllerName.GetControllerRoute()}/{actionName}";
        }

        public static string GetUrl(string host, string actionUrl)
        {
            var decodedActionUrl = HttpUtility.UrlDecode(actionUrl);

            return $"https://{host}{decodedActionUrl}";
        }
    }
}
