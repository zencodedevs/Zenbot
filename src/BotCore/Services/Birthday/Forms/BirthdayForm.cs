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

    // Modal form for adding Birthday date
    public class BirthdayMessageForm : IModal
    {
        public string Title => "Brithday Message";

        [InputLabel("Status")]
        [ModalTextInput("status", Discord.TextInputStyle.Short, "Enter the day of the month", 1, 5, null)]
        [RequiredInput]
        public string Status { get; set; }


        [InputLabel("Message")]
        [ModalTextInput("message", Discord.TextInputStyle.Paragraph, "Enter the Birthday Message here", 1, 500, null)]
        [RequiredInput]
        public string Message { get; set; }

       
    }
}