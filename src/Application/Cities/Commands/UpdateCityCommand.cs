using MediatR;
using System.Threading;
using Zen.Domain.Interfaces;
using System.Threading.Tasks;
using ZenAchitecture.Domain.Shared.Entities.Geography;

namespace ZenAchitecture.Application.Account.Cities.Commands
{
    public class UpdateCityCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, int>
    {
        private readonly IEntityFrameworkRepository<City> _repository;

        public UpdateCityCommandHandler(IEntityFrameworkRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.FindAsync(request.Id, cancellationToken);

            entity.UpdateInfo(request.Name);

            var res = await _repository.UpdateAsync(entity, true, cancellationToken);

            return res.Id;
        }
    }
}
