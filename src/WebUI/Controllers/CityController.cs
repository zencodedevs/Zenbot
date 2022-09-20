﻿using System.Threading.Tasks;
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
    public class CityController : ZenController
    {
        private readonly DiscordBotService _botService;
        public CityController(ILogger<CityController> logger, DiscordBotService botService)
        {
            this._botService = botService;

            logger.LogInformation("Test LogInformation");
            logger.LogWarning("Test LogWarning");
            logger.LogError("Test LogError");
            logger.LogCritical("Test LogCritical");
            logger.LogTrace("Test LogTrace");
        }

        [HttpGet]
        [Route(nameof(ReadCity))]
        [Authorize]
        public async Task<CityDto> ReadCity() => await Mediator.Send(new GetCityQuery());

        [HttpGet]
        [Authorize]
        [Route(nameof(GetCities))]
        public async Task<List<CityDto>> GetCities() => await Mediator.Send(new GetCitiesQuery());

        [HttpPost]
        //[Authorize]
        [Route(nameof(CreateCity))]
        public async Task<int> CreateCity(CreateCityCommand command)
        {
            await Mediator.Send(command);
            return 0;
        }
        [HttpPost]
        [Authorize]
        [Route((nameof(UpdateCity)))]
        public async Task<int> UpdateCity(UpdateCityCommand command) => await Mediator.Send(command);

        [HttpDelete]
        [Authorize]
        [Route((nameof(DeleteCity)))]
        public async Task<int> DeleteCity([FromQuery] DeleteCityCommand command) => await Mediator.Send(command);



        [HttpPost]
        //[Authorize]
        [Route(nameof(CreateJira))]
        public Task CreateJira(string jiraId)
        {
            _botService.Event(jiraId);

            return Task.CompletedTask;
        }

    }
}
