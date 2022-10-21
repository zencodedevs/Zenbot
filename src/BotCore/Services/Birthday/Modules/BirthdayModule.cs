using BotCore.Entities;
using BotCore.Services;
using BotCore.Services.Birthday.Forms;
using Discord;
using Discord.Interactions;
using Domain.Shared.Entities.Bot;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace BotCore.Services.Birthday.Modules
{
    /// <summary>
    /// All about Birthday Command and register 
    /// </summary>
    [Group("birthday", "birthday commands")]
    public class BirthdayModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public BirthdayMessageService _birthdayMessageService { get; set; }

        public UserService _usersService { get; set; }
        public BirthdayService brithdayService { get; set; }
        /// <summary>
        /// Check if there is some upcomming birthday by Admin
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [SlashCommand("upcoming", "upcoming members brithday")]
        [ComponentInteraction("brithday-list:*", true)]
        public async Task list(int page = 0)
        {
            await DeferAsync();

            var users = await _usersService.GetUpComingUsersBrithday();

            if (users != null && users.Count() != 0)
            {
                string times = "";
                string usersContent = "";
                foreach (var user in users)
                {
                    times += $"<t:{((DateTimeOffset)user.Birthday).ToUnixTimeSeconds()}:R>\n";
                    usersContent += $"{MentionUtils.MentionUser(user.DiscordId)}\n";
                }

                var embed = new EmbedBuilder()
                    .AddField("User", usersContent, true)
                    .AddField("Brithday", times, true)
                    .Build();

                var components = new ComponentBuilder()
                    .WithButton("Previous", $"brithday-list:{page - 1}", ButtonStyle.Primary, disabled: page > 0)
                    .WithButton("Next", $"brithday-list:{page + 1}", ButtonStyle.Primary, disabled: users.Count() > 30)
                    .Build();

                await FollowupAsync(embed: embed, components: components);
               
                return;
            }
            await FollowupAsync("No user found.");
        }




        /// <summary>
        /// Add birthday data by user
        /// </summary>
        /// <returns></returns>
        [SlashCommand("add", "add your brithday")]
        public async Task add()
        {
            await RespondWithModalAsync<BirthdayForm>($"set-brithday");
        }
        [ModalInteraction("set-brithday", true)]
        public async Task set_modal(BirthdayForm form)
        {
            await DeferAsync();

            DateTime dateTime = DateTime.MinValue
                .AddYears(form.Year - 1)
                .AddMonths(form.Month - 1)
                .AddDays(form.Day - 1);

            var user = _usersService.UpdateAsync(Context.BotUser, x =>
            {
                x.Birthday = dateTime;
                x.Username = Context.User.Username;
                x.GuildId = Context.BotGuild.Id;
            });

            await FollowupAsync($"Done, your brithday added, <t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:D>", ephemeral:true);
            
            var todayDay = DateTime.UtcNow.Day;
            var todayMonth = DateTime.UtcNow.Month;
            if (dateTime.Day == todayDay && dateTime.Month == todayMonth)
            {
                await brithdayService.NotficationUsersBirthdayAsync(new BotUser[] { Context.BotUser });
            }
        }


     

        // check by each user the exact time of birthday date

        [SlashCommand("time", "time of user brithday.")]
        public async Task time(IGuildUser user)
        {
            await DeferAsync();

            var targetUser = await _usersService.GetAsync(a => a.DiscordId == user.Id);
            if(targetUser is null)
            {
                await FollowupAsync($"User not found.");
                return;
            }
            await FollowupAsync($"{MentionUtils.MentionUser(user.Id)}'s brithday is <t:{((DateTimeOffset)targetUser.Birthday).ToUnixTimeSeconds()}:R>.");
        }



    }
}