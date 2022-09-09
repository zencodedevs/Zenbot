using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAchitecture.Application.Common.Security
{
    public class ClaimsRequirementFilter : IAuthorizationFilter
    {
        readonly string _claimType;
        readonly string[] _claimValues;

        public ClaimsRequirementFilter(string[] claimValues, string claimType = null)
        {
            _claimType = claimType;
            _claimValues = claimValues;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllClaimValue = true;
            foreach(var userClaim in _claimValues)
            {
                if (!context.HttpContext.User.Claims.Any(c => c.Value == userClaim))
                {
                    hasAllClaimValue = false;
                    break;
                }
            }

            if(_claimType != null)
            {
                var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claimType) && hasAllClaimValue;
                if (!hasClaim)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                if (!hasAllClaimValue)
                {
                    context.Result = new ForbidResult();
                }
            }
            
        }
    }
}
