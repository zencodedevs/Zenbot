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
            var challengeAuthenticationProperties = new AuthenticationProperties()
            {
                RedirectUri = nameof(PanelController.Index).GetActionRoute(nameof(PanelController))
            };

            return Challenge(challengeAuthenticationProperties);
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
