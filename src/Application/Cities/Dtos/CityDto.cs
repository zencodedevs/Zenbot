using AutoMapper;
using ZenAchitecture.Domain.Shared.Entities.Geography;
using Zen.Application.Mappings;

namespace ZenAchitecture.Application.Account.Cities.Dtos
{
    public class CityDto : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public void Mapping(Profile Profile)
        {
            Profile.CreateMap<City, CityDto>().ForMember(m => m.Title, o => o.MapFrom<TitleResolver>());
        }

        private class TitleResolver : IValueResolver<City, CityDto, string>
        {
            public string Resolve(City source, CityDto destination, string destMember, ResolutionContext context)
            {
                return source.Name;
            }
        }
    }
}