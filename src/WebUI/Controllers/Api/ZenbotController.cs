using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.BotCore;

namespace Zenbot.WebUI.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class ZenbotController : ZenController
    {
        private readonly DiscordBotService _botService;

        public ZenbotController(DiscordBotService botService)
        {
            _botService = botService;
        }

        [HttpGet]
        public async Task GetJiraWebhook(string id)
        {
            // statements
        }
    }
}
