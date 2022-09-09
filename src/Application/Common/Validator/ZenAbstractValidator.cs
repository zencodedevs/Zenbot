using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Zen.Uow;

namespace ZenAchitecture.Application.Common.Validator
{
    public class ZenAbstractValidator<T> : AbstractValidator<T>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public readonly IUnitOfWorkManager _unitOfWorkManager;

        public readonly IStringLocalizer _stringLocalizer;

        public ZenAbstractValidator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _stringLocalizer = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IStringLocalizer>();

            _unitOfWorkManager = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        }
    }
}
