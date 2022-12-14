using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.GSuite.Form
{
    public class GSuiteForm : IModal
    {
        public string Title => "G Suite Account";

        [InputLabel("First Name")]
        [ModalTextInput("firstname", Discord.TextInputStyle.Short, "Enter the first name", 1, 30, null)]
        [RequiredInput]
        public string FirstName { get; set; }


        [InputLabel("Family")]
        [ModalTextInput("family", Discord.TextInputStyle.Short, "Enter the Family", 1, 30, null)]
        [RequiredInput]
        public string Family { get; set; }

        [InputLabel("Primary Email")]
        [ModalTextInput("email", Discord.TextInputStyle.Short, "Enter the Primary Email", 1, 30, null)]
        [RequiredInput]
        [EmailAddress(ErrorMessage ="Please enter the correct email address")]
        public string Email { get; set; }

        [InputLabel("Password")]
        [ModalTextInput("password", Discord.TextInputStyle.Short, "Enter the Passowrd", 8, 20, null)]
        [RequiredInput]
        public string Password { get; set; }

       
    }
}
