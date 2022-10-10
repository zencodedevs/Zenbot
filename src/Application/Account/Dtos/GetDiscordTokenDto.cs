using AutoMapper;
using Zen.Application.Mappings;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Application.Account.Dtos
{
    public class GetDiscordTokenDto : IMapFrom<GetDiscordTokenResponse>
    {
        public void Mapping(Profile Profile)
        {
            //Profile.CreateMap<GetDiscordTokenResponse, GetDiscordTokenDto>()
            //    .ForMember(m => m.Id, m => m.MapFrom(o => o.Id));
        }
    }
}
