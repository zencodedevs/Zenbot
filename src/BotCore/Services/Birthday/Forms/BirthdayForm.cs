using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;

namespace BotCore.Services.Birthday.Forms
{
    // Modal form for adding Birthday date
    public class BirthdayForm : IModal
    {
        public string Title => "Brithday";

        [InputLabel("Day")]
        [ModalTextInput("day", Discord.TextInputStyle.Short, "Enter the day of the month", 1, 2, null)]
        [RequiredInput]
        public int Day { get; set; }


        [InputLabel("Month")]
        [ModalTextInput("month", Discord.TextInputStyle.Short, "Enter the month", 1, 2, null)]
        [RequiredInput]
        public int Month { get; set; }

        [InputLabel("Year")]
        [ModalTextInput("year", Discord.TextInputStyle.Short, "Enter the year", 1, 4, null)]
        [RequiredInput]
        public int Year { get; set; }
    }

    // Modal form for adding Birthday new Birthday Message
    public class BirthdayMessageForm : IModal
    {
        public string Title => "Brithday Message";

        [InputLabel("Message")]
        [ModalTextInput("message", Discord.TextInputStyle.Paragraph, "Enter the Birthday Message here, use {username} where ever you want to put the Discord's username", 1, 500, null)]
        [RequiredInput]
        public string Message { get; set; }

       
    }
}