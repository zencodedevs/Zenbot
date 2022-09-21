using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Microsoft.Extensions.Localization;
using Zenbot.Domain.Shared.Entities.Geography;
using Zenbot.Application.Account.Cities.Commands;
using Zenbot.Application.Common.Validator;
using Microsoft.Extensions.DependencyInjection;
using Zen.Uow;

namespace Zenbot.Application.Account.Cities.Validators
{
    public class UpdateCityCommandValidator : ZenAbstractValidator<UpdateCityCommand>
    {
        private readonly IEntityFrameworkRepository<City> _repository;

        public UpdateCityCommandValidator(IEntityFrameworkRepository<City> repository, IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {

            _repository = repository;

            RuleFor(v => v.Id).MustAsync(DepartmentExists).WithMessage(_stringLocalizer.GetString("RecordNotFound"));

        }

        private async Task<bool> DepartmentExists(int departmentId, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                return await _repository.ExistAsync(departmentId, cancellationToken);
            }
        }

    }
}