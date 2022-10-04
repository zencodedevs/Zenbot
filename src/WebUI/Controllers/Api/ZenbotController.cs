using BotCore;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.BotCore;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.JiraWebHook;

namespace Zenbot.WebUI.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {
        private readonly IServiceProvider _services;
        private readonly JiraService _jiraService;
        public ZenbotController(IServiceProvider services, JiraService jiraService)
        {
            _services = services;
            _jiraService = jiraService;
        }
        [HttpPost]
        public async Task GetJiraWebHook([FromBody] JiraWebhookObject value)
        {
            var jiraWH = new JiraWebHook
            {
                AssigneeId = value.issue.fields.assignee.accountId,
                AssigneeName = value.issue.fields.assignee.displayName,
                IssueSelf = value.issue.self,
                PriorityIconUrl = value.issue.fields.priority.iconUrl,
                PriorityName = value.issue.fields.priority.name,
                ProjectName = value.issue.fields.project.name,
                ProjectUrl = value.issue.fields.project.self,
                ReporterName = value.issue.fields.reporter.displayName
            };
            var embed = new EmbedBuilder()
            {
                Title = "New Task Assigned",
                Description = jiraWH.ReporterName + " has assigned you a new task"
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Reporter ------>", "1", ButtonStyle.Secondary, new Emoji("👤"), "",true, row: 0)
                .WithButton(jiraWH.ReporterName, null, ButtonStyle.Link, null, jiraWH.IssueSelf, row: 0)

                .WithButton("Project Name ->", "2", ButtonStyle.Secondary, new Emoji("🛠"), "", true, row: 1)
                .WithButton(jiraWH.ProjectName, null, ButtonStyle.Link, null, jiraWH.ProjectUrl, row: 1)

                .WithButton("Priority -------->", "3", ButtonStyle.Secondary, new Emoji("♦"), "", true, row: 2)
                .WithButton(jiraWH.PriorityName, null, ButtonStyle.Link, null, jiraWH.IssueSelf, row: 2)

                .Build();

            var message = await this._jiraService.TrySendMessageToUserAsync(jiraWH.AssigneeId, "", false, embed: embed, components: component);
        }

    }
}
