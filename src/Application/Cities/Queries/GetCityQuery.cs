using MediatR;
using AutoMapper;
using System.Threading;
using Zen.Domain.Interfaces;
using System.Threading.Tasks;
using Zenbot.Application.Account.Cities.Dtos;
using Zenbot.Domain.Shared.Entities.Geography;

namespace Zenbot.Application.Account.Cities.Queries
{
    public class GetCityQuery : IRequest<CityDto>
    {
        public int Id { get; set; }
    }
    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, CityDto>
    {
        private readonly IMapper _mapper;
        private readonly IEntityFrameworkRepository<City> _repository;

        public GetCityQueryHandler(IEntityFrameworkRepository<City> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CityDto> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.FindAsync(request.Id, cancellationToken);

            return _mapper.Map<CityDto>(entity);
        }
    }
}
