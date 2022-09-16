using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;

namespace Zenbot
{
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
}