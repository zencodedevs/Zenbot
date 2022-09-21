using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Zen.Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zenbot.Application.Account.Cities.Dtos;
using Zenbot.Domain.Shared.Entities.Geography;

namespace Zenbot.Application.Account.Cities.Queries
{
    public class GetCitiesQuery : IRequest<List<CityDto>> { }


    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
    {
        private readonly IMapper _mapper;
        private readonly IEntityFrameworkRepository<City> _repository;

        public GetCitiesQueryHandler(IEntityFrameworkRepository<City> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetQueryableWithDataFilterAsync();

            var entities = await query.OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

            return _mapper.Map<List<CityDto>>(entities);

        }
    }
}
