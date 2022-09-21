using Zenbot.Application.Account.Cities.Commands;
using Zenbot.Application.Common.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace Zenbot.Application.Account.Cities.Validators
{
    public class CreateCityCommandValidator : ZenAbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }
    }
}
