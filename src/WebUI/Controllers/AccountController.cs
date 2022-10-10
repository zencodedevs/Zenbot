using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Zenbot.Application.Account.Queries;

namespace Zenbot.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public AccountController(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        // GET: /Account/DiscordLogin
        [HttpGet]
        public async Task<IActionResult> DiscordLogin()
        {
            var formattedDiscordBaseAuthorizationUrl = await _mediator.Send(new GetFormattedDiscordBaseAuthorizationUrlQuery());
            return Redirect(formattedDiscordBaseAuthorizationUrl);
        }

        // GET: /Account/DiscordLoginRedirect
        [HttpGet]
        public async Task<IActionResult> DiscordLoginRedirect(string code)
        {
            var discordToken = await _mediator.Send(new GetDiscordTokenQuery
            {
                Code = code
            });

            return Content(code);
        }

        // GET: /Account/DiscordLogout
        [HttpGet]
        public IActionResult DiscordLogout()
        {
            return null;
        }
    }
}
