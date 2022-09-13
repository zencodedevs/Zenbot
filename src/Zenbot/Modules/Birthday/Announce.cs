using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

using System;

namespace Zenbot.Modules.Birthday
{
    // This class is used for the timed (daily) birthday announcements.
    // It is also used for the forceAnnouncement command.
    public class Announce
    {
        public static Task AnnounceBirthdays(DiscordSocketClient Client)
        {
            //Database.Birthday[] birthdays = Data.Data.GetBirthdays();
            var builder = new EmbedBuilder()
                .WithTitle("Happy Birthday")
                .WithColor(new Color(0x8CE3C5))
                .WithImageUrl("https://media.giphy.com/media/xUOxf0vukEHTKkD4ic/giphy.gif");

            int numOfBirthdays = 0;
            DateTime today = DateTime.Now;
            var channel = Client.GetChannel(1018765311215947816) as IMessageChannel;
            
            string bdayMessage = "Happy Birthday to You";

            //foreach (Birthday bday in birthdays)
            //{
            //    if (bday.Month == today.Month && bday.Day == today.Day)
            //    {
                    //numOfBirthdays++;
                    //bdayMessage += $" {Client.GetUser(bday.UserId).Mention}";
            //    }
            //}

            // No announcement will be made if there are no birthdays.
                channel.SendMessageAsync($"{bdayMessage}!", false, builder.Build());
            
            return Task.CompletedTask;
        }
    }
}
