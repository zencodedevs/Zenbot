using Discord.Interactions;
using System;
using System.Collections.Generic;
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
        public string Email { get; set; }

        [InputLabel("Phone Number")]
        [ModalTextInput("phoneNumber", Discord.TextInputStyle.Short, "Enter the Phone Number", 1, 14, null)]
        [RequiredInput]
        public string PhoneNumber { get; set; }

        [InputLabel("Password")]
        [ModalTextInput("password", Discord.TextInputStyle.Short, "Enter the Passowrd", 1, 20, null)]
        [RequiredInput]
        public string Password { get; set; }

       
    }
}
