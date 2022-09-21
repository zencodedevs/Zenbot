using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities;

namespace Zenbot.Infrastructure.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager, IUnitOfWorkManager unitOfWorkManager)
        {
            _userManager = userManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            List<Claim> claims;

            using (var uow = _unitOfWorkManager.Begin())
            {
                var user = await _userManager.GetUserAsync(context.Subject);

                var tenantId = user.Id;

                var userName = user.NormalizedUserName;

                if (!string.IsNullOrEmpty(user.Id))
                {
                    claims = new List<Claim>
                    {
                        new Claim("x-tenant-id", tenantId.ToString()),
                        new Claim("x-user-name", userName.ToString())
                    };

                    context.IssuedClaims.AddRange(claims);
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            ApplicationUser user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = user != null;
        }
    }
}
