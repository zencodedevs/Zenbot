using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.WebUI.Helpers;

namespace Zenbot.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: /Account/DiscordLogin
        [HttpGet]
        public IActionResult DiscordLogin()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(PanelController.Index), nameof(PanelController).GetControllerRoute());

            var challengeAuthenticationProperties = new AuthenticationProperties()
            {
                RedirectUri = nameof(AccountController.DiscordLoginCallback).GetActionRoute(nameof(AccountController))
            };

            return Challenge(challengeAuthenticationProperties);
        }

        // GET: /Account/DiscordLoginCallback
        [HttpGet]
        public IActionResult DiscordLoginCallback()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerRoute());

            return RedirectToAction(nameof(PanelController.Index), nameof(PanelController).GetControllerRoute());
        }

        // GET: /Account/DiscordLogout
        [HttpGet]
        public async Task<IActionResult> DiscordLogout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).GetControllerRoute());
        }
    }
}
