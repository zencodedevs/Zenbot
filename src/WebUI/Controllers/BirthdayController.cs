using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;
using Zenbot.WebUI.Extensions;

namespace Zenbot.WebUI.Controllers
{
    public class BirthdayController : Controller
    {
        private readonly IBirthdayMessageService _birthdayMessageService;

        public BirthdayController(IBirthdayMessageService birthdayMessageService)
        {
            _birthdayMessageService = birthdayMessageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.GetOperatorUserId();

            var messages = await _birthdayMessageService.GetBirthdayMessagesByGuildId((ulong)userid);
            return View(messages);
        }
    }
}
