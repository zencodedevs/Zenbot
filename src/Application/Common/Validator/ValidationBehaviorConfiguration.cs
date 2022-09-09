using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAchitecture.Application.Common.Validator
{
    public class ValidationBehaviorConfiguration
    {
        public bool HideTitle { get; set; }

        public bool DisableTimeOut { get; set; }

        public string Title { get; set; }

        public ValidationType Type { get; set; }

        public enum ValidationType
        {
            Success,
            Error,
            Info,
            Warning
        }
    }
}
