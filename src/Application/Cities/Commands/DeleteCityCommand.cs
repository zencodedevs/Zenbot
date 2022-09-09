using MediatR;
using System.Threading;
using Zen.Domain.Interfaces;
using System.Threading.Tasks;
using ZenAchitecture.Domain.Shared.Entities.Geography;

namespace ZenAchitecture.Application.Account.Cities.Commands
{
    public class DeleteCityCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, int>
    {
        private readonly IEntityFrameworkRepository<City> _repository;

        public DeleteCityCommandHandler(IEntityFrameworkRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, true, cancellationToken);

            return request.Id;
        }
    }
}
