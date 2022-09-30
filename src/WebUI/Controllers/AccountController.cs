using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Zenbot.WebUI.Helpers;

namespace Zenbot.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /Account/DiscordLogin
        [HttpGet]
        public IActionResult DiscordLogin()
        {
            var clientId = _configuration.GetSection("DiscordOAuth")["ClientId"];
            var loginUrlFormat = _configuration.GetSection("DiscordOAuth")["LoginUrlFormat"];
            var requestHostUrl = Request.Host.Value;
            var redirectActionRoute = nameof(AccountController.DiscordLoginRedirect).GetActionRoute(nameof(AccountController));
            var redirectUrl = RoutingHelper.GetUrl(requestHostUrl, redirectActionRoute);
            var loginUrl = DiscordOAuthHelper.GetLoginUrl(loginUrlFormat, clientId, redirectUrl);

            return Redirect(loginUrl);
        }

        // GET: /Account/DiscordLoginRedirect
        [HttpGet]
        public IActionResult DiscordLoginRedirect(string code)
        {
            return Content(code);
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
