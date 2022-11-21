using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    [Authorize]
    public class IntegrationController : Controller
    {
        private readonly IBotUserGuildService _botUserGuildService;
        private readonly IBotUserService _botUserService;
        private readonly INotyfService _notyf;

        public IntegrationController(IBotUserGuildService botUserGuildService, IBotUserService botUserService, INotyfService notyf)
        {
            _botUserGuildService = botUserGuildService;
            _botUserService = botUserService;
            _notyf = notyf;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();

            var guilds = await _botUserGuildService.GetAllGuildsByUserId((ulong)userid);
            return View(guilds);
        }

        [HttpGet]
        public async Task<IActionResult> BotUsers(int guildId, string guildName)
        {
            var users = await _botUserGuildService.GetAllGuildsByGuildId(guildId);
            ViewBag.guildName = guildName;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserBirthday(DateTime birthdayDate, int userId, string guildName, int guildId)
        {
            var update = await _botUserService.UpdateBirthday(birthdayDate, userId);
            if (update)
            {
                _notyf.Success("Birthday Date updated successfully");
                return RedirectToAction("BotUsers", new { guildId = guildId, guildName = guildName });
            }
            // Toas Error message
            _notyf.Error("An error occured during updating the birthday date");
            return RedirectToAction("BotUsers", new { guildId = guildId, guildName = guildName });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserIntegration(string jiraAccount, string bitbucketAccount, bool flexSwitchCheckChecked, int userId, string guildName, int guildId)
        {
            var update = await _botUserService.UpdateIntegration(jiraAccount,bitbucketAccount, flexSwitchCheckChecked, userId);
            if (update)
            {
                _notyf.Success("Integration updated successfully");
                return RedirectToAction("BotUsers", new { guildId = guildId, guildName = guildName });
            }
            // Toas Error message
            _notyf.Error("An error occured during updating the Integrating");
            return RedirectToAction("BotUsers", new { guildId = guildId, guildName = guildName });
        }

    }
}
