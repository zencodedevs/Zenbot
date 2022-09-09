namespace ZenAchitecture.Infrastructure.Identity
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Localization;
    using System.Text;

    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer _stringLocalizer;

        public CustomIdentityErrorDescriber(IStringLocalizer stringLocalizer) : base()
        {
            _stringLocalizer = stringLocalizer;
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return base.RecoveryCodeRedemptionFailed();
        }


        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return base.PasswordRequiresUniqueChars(uniqueChars);
        }

        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = _stringLocalizer.GetString("DefaultError") }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = _stringLocalizer.GetString("ConcurrencyFailure") }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = _stringLocalizer.GetString("PasswordMismatch") }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = _stringLocalizer.GetString("InvalidToken") }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = _stringLocalizer.GetString("LoginAlreadyAssociated") }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = string.Format(_stringLocalizer.GetString("InvalidUserName"), userName) }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = string.Format(_stringLocalizer.GetString("InvalidEmail"), email) }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = string.Format(_stringLocalizer.GetString("DuplicateUserName"), userName) }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(_stringLocalizer.GetString("DuplicateEmail"), email) }; }
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = string.Format(_stringLocalizer.GetString("InvalidRoleName"), role) }; }
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = string.Format(_stringLocalizer.GetString("DuplicateRoleName"), role) }; }
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = _stringLocalizer.GetString("UserAlreadyHasPassword") }; }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = _stringLocalizer.GetString("UserLockoutNotEnabled") }; }
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = string.Format(_stringLocalizer.GetString("UserAlreadyInRole"), role) }; }
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = string.Format(_stringLocalizer.GetString("UserNotInRole"), role) }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = string.Format(_stringLocalizer.GetString("PasswordTooShort"), length) }; }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = _stringLocalizer.GetString("PasswordRequiresNonAlphanumeric") }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = _stringLocalizer.GetString("PasswordRequiresDigit") }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = _stringLocalizer.GetString("PasswordRequiresLower") }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = _stringLocalizer.GetString("PasswordRequiresUpper") }; }
    }
}
