using ZenAchitecture.Domain.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace ZenAchitecture.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }


        public static Result ToApplicationResult(this SignInResult result)
        {

            if (!result.Succeeded)
                return Result.Failure(new List<string>() { "UserAuthorizationFailed" });

            if (result.IsLockedOut)
                return Result.Failure(new List<string>() { "UserIsLockedOut" });

            if (result.IsNotAllowed)
                return Result.Failure(new List<string>() { "UserIsNotAllowed "});

            if (result.RequiresTwoFactor)
                return Result.Failure(new List<string>() {" UserRequiresTwoFactor "});

            return Result.Success();

        }
    }
}