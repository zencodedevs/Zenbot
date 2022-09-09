using ZenAchitecture.Application.Account.Cities.Commands;
using ZenAchitecture.Application.Common.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace ZenAchitecture.Application.Account.Cities.Validators
{
    public class CreateCityCommandValidator : ZenAbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }
    }
}
