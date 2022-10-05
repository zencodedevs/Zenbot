using BotCore;
using BotCore.Shared;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Zen.Mvc;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.BitbucketWebHook;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.JiraWebHook;

namespace Zenbot.WebUI.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ZenbotController : ZenController
    {
        private readonly IServiceProvider _services;
        private readonly JiraService _jiraService;
        private readonly BitbucketService _bitbucketService;
        public ZenbotController(IServiceProvider services, JiraService jiraService)
        {
            _services = services;
            _jiraService = jiraService;
            _bitbucketService = services.GetRequiredService<BitbucketService>();
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
                Description = $"**{jiraWH.ReporterName}** has assigned you a new task"
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Reporter ------>", "1", ButtonStyle.Secondary, new Emoji("👤"), "", true, row: 0)
                .WithButton(jiraWH.ReporterName, null, ButtonStyle.Link, null, jiraWH.IssueSelf, row: 0)

                .WithButton("Project Name ->", "2", ButtonStyle.Secondary, new Emoji("🛠"), "", true, row: 1)
                .WithButton(jiraWH.ProjectName, null, ButtonStyle.Link, null, jiraWH.ProjectUrl, row: 1)

                .WithButton("Priority -------->", "3", ButtonStyle.Secondary, new Emoji("♦"), "", true, row: 2)
                .WithButton(jiraWH.PriorityName, null, ButtonStyle.Link, null, jiraWH.IssueSelf, row: 2)

                .Build();

            var message = await this._jiraService.SendMessageToUserAsync(jiraWH.AssigneeId, "", false, embed: embed, components: component);
        }


        // For Bitbucket Webhook

        [HttpPost("bitbucket")]
        public async Task BitbucketWebHook([FromBody] BitbucketWebHookRequest value)
        {
            var reviewers = value.pullrequest.reviewers.ToArray();

            var bitbucketWH = new BitbucketWebHook
            {
                AuthorName = value.pullrequest.author.display_name,
                PullRequestDate = value.pullrequest.created_on,
                PullRequestLink = value.pullrequest.links.html.href,
                PullRequestTitle = value.pullrequest.title,
                RepositoryName = value.repository.name,
                RepositoryLink = value.repository.links.html.href,
                PullRequestId = value.pullrequest.id,
            };
            var embed = new EmbedBuilder()
            {
                Title = "New Task Assigned",
                Description = $"**{bitbucketWH.AuthorName}**  added you as a reviewer on pull request #{bitbucketWH.PullRequestId}"
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Author ------>", "1", ButtonStyle.Success, null, null, true, row: 0)
                .WithButton(bitbucketWH.AuthorName, Buttons.BtnTextCustomId, ButtonStyle.Secondary, row: 0)

                .WithButton("Repository ->", "3", ButtonStyle.Success, null, "", true, row: 1)
                .WithButton(bitbucketWH.RepositoryName, null, ButtonStyle.Link, null, bitbucketWH.RepositoryLink, row: 1)

                .WithButton("Pull Request Message ->", "4", ButtonStyle.Success, null, "", true, row: 2)
                .WithButton(bitbucketWH.PullRequestTitle, Buttons.BtnTextCustomId, ButtonStyle.Link, null,bitbucketWH.PullRequestLink, row: 2)

                 .WithButton("Date -------->", "6", ButtonStyle.Success, null, null, true, row: 3)
                .WithButton(bitbucketWH.PullRequestDate.ToString("dddd, dd MMMM yyyy"), Buttons.BtnTextCustomId, ButtonStyle.Secondary, row: 3)

                .Build();

            foreach (var item in reviewers)
            {
                var message = await this._bitbucketService.SendMessageToUserAsync(item.account_id, "", false, embed: embed, components: component);
                await Task.Delay(100);
            }

        }

    }
}
