using Discord;
using Discord.Interactions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Zenbot.Interactions.Modules
{
    [Group("birthday", "birthday commands")]
    public class BirthdayModule : InteractionModuleBase<SocketInteractionContext>
    {
        public UsersService UsersService { get; set; }



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
            var user = UsersService.addBotUser(Context.User.Username, form.userMail, Context.User.Id, dateTime, nexttime, form.JiraAccountID);

            await FollowupAsync($"Done, your brithday added, <t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:D>");
        }





        [SlashCommand("time", "time of user brithday.")]
        public async Task time(IGuildUser user)
        {
            await DeferAsync();
            var target = await UsersService.GetUser(Context.User.Id);

            await FollowupAsync($"{MentionUtils.MentionUser(user.Id)}'s brithday is <t:{((DateTimeOffset)target.Birthday).ToUnixTimeSeconds()}:R>.");
        }



    }
}