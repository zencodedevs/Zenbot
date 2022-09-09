using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAchitecture.Application.Common.Security
{
    public class ClaimsRequirementAttribute : TypeFilterAttribute
    {
        public ClaimsRequirementAttribute(string[] claimValues, string claimType = null ) : base(typeof(ClaimsRequirementFilter))
        {
            if (claimType == null)
                Arguments = new object[] { claimValues };
            else
                Arguments = new object[] { claimType, claimValues };
        }
    }
}
