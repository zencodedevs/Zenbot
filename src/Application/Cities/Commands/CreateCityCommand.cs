using ZenAchitecture.Domain.Shared.Entities.Geography;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;

namespace ZenAchitecture.Application.Account.Cities.Commands
{
    public class CreateCityCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, int>
    {
        private readonly IEntityFrameworkRepository<City> _repository;

        public CreateCityCommandHandler(IEntityFrameworkRepository<City> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = new City();
            city.Create(request.Name);
            return (await _repository.InsertAsync(city, autoSave: true, cancellationToken: cancellationToken)).Id;
        }
    }
}
