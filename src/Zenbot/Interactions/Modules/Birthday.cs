using CsvHelper.TypeConversion;
using Discord;
using Discord.Interactions;
using Ical.Net;
using Microsoft.VisualBasic;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Common.Extensions;
using static AutoMapper.Internal.ExpressionFactory;

namespace Zenbot.Interactions.Modules
{
    [Group("birthday", "birthday commands")]
    public class BirthdayModule : InteractionModuleBase<SocketInteractionContext>
    {
        public UsersService UsersService { get; set; }

        [SlashCommand("list", "members brithday")]
        [ComponentInteraction("brithday-list:*", true)]
        public async Task list(int page = 0)
        {
            await DeferAsync();

            var users = UsersService.GetUsersBirthday().Skip(page * 30).Take(31).ToList();

            if (users != null && users.Count != 0)
            {
                string times = "";
                string usersContent = "";
                foreach (var user in users)
                {
                    times += $"<t:{((DateTimeOffset)user.Brithday).ToUnixTimeSeconds()}:R>\n";
                    usersContent += $"{MentionUtils.MentionUser(user.Id)}\n";
                }

                var embed = new EmbedBuilder()
                    .AddField("User", usersContent, true)
                    .AddField("Brithday", times, true)
                    .Build();

                var components = new ComponentBuilder()
                    .WithButton("Previous", $"brithday-list:{page - 1}", ButtonStyle.Primary, disabled: page > 0)
                    .WithButton("Next", $"brithday-list:{page + 1}", ButtonStyle.Primary, disabled: users.Count > 30)
                    .Build();

                await FollowupAsync(embed: embed, components: components);
                return;
            }
            await FollowupAsync("No user found.");
        }

        [SlashCommand("add", "add user brithday")]
        public async Task add(IGuildUser user)
        {
            await RespondWithModalAsync<BirthdayForm>($"set-brithday:{user.Id}");
        }
        [ModalInteraction("set-brithday:*", true)]
        public async Task set_modal(ulong userId, BirthdayForm form)
        {
            await DeferAsync();

            DateTime dateTime = DateTime.MinValue
                .AddYears(form.Year - 1)
                .AddMonths(form.Month - 1)
                .AddDays(form.Day - 1);

            var user = UsersService.GetUser(userId);
            user.Brithday = dateTime;

            await FollowupAsync($"Done, user brithday added <t:{((DateTimeOffset)user.Brithday).ToUnixTimeSeconds()}:R>");
        }


        [SlashCommand("notice-brithday", "enable notification for user brithday.")]
        public async Task notice_modal(IGuildUser user, BooleanType state = BooleanType.Enable)
        {
            await DeferAsync();
            var target = UsersService.GetUser(user.Id);
            target.NoticeBrithday = state == BooleanType.Enable;

            await FollowupAsync($"Done, brithday notification has been enabled.");
        }
        public enum BooleanType
        {
            Disable,
            Enable,
        }
        [SlashCommand("time", "time of user brithday.")]
        public async Task time(IGuildUser user)
        {
            await DeferAsync();
            var target = UsersService.GetUser(user.Id);

            await FollowupAsync($"{MentionUtils.MentionUser(user.Id)}'s brithday is <t:{((DateTimeOffset)target.Brithday).ToUnixTimeSeconds()}:R>.");
        }


    }
}