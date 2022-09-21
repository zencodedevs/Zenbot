using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZenAchitecture.Application.Account.Cities.Dtos;
using ZenAchitecture.Application.Account.Cities.Queries;
using ZenAchitecture.Application.Account.Cities.Commands;
using Zen.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Zenbot;

namespace ZenAchitecture.WebUI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {
        private readonly DiscordBotService _botService;
        public ZenbotController(DiscordBotService botService)
        {
            this._botService = botService;
        }

        // testing if data comes to our bot we shoul check if we have this user in discord then send message to 
        [HttpPost]
        [Route(nameof(CreateJira))]
        public Task CreateJira(string jiraId)
        {
            _botService.Event(jiraId);

            return Task.CompletedTask;
        }

    }
}
