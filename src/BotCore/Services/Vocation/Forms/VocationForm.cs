using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.VocationForms
{
    public class VocationForm : IModal
    {
        public string Title => "Vocation Request";

        [InputLabel("Start Date")]
        [ModalTextInput("startDate", Discord.TextInputStyle.Short, "day/month/year => 12/07/2022", 9, 10, null)]
        [RequiredInput]
        public DateTime StartDate { get; set; }


        [InputLabel("End Date")]
        [ModalTextInput("endDate", Discord.TextInputStyle.Short, "day/month/year => 13/07/2022", 9, 10, null)]
        [RequiredInput]
        public DateTime EndDate { get; set; }


        [InputLabel("Description")]
        [ModalTextInput("description", Discord.TextInputStyle.Paragraph, "Reason or any description", 5, 1000, null)]
        [RequiredInput]
        public string Description { get; set; }

    }
}
