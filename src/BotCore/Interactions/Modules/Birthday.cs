using BotCore;
using Discord;
using Discord.Interactions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Zenbot.BotCore.Interactions.Modules
{
    /// <summary>
    /// All about Birthday Command and register 
    /// </summary>
    [Group("birthday", "birthday commands")]
    public class BirthdayModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UsersService UsersService { get; set; }



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

            var users = await UsersService.GetUpComingUsersBrithday();

            if (users != null && users.Count() != 0)
            {
                string times = "";
                string usersContent = "";
                foreach (var user in users)
                {
                    times += $"<t:{((DateTimeOffset)user.Birthday).ToUnixTimeSeconds()}:R>\n";
                    usersContent += $"{MentionUtils.MentionUser(user.UserId)}\n";
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

            var nexttime = DateTime.UtcNow;
            var user = UsersService.addBotUser(Context.User.Username, Context.User.Username, Context.User.Id, dateTime, nexttime);

            await FollowupAsync($"Done, your brithday added, <t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:D>");
        }



        // check by each user the exact time of birthday date

        [SlashCommand("time", "time of user brithday.")]
        public async Task time(IGuildUser user)
        {
            await DeferAsync();
            var target = await UsersService.GetUser(Context.User.Id);

            await FollowupAsync($"{MentionUtils.MentionUser(user.Id)}'s brithday is <t:{((DateTimeOffset)target.Birthday).ToUnixTimeSeconds()}:R>.");
        }



    }
}