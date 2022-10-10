using AutoMapper;
using Zen.Application.Mappings;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Application.Account.Dtos
{
    public class GetDiscordTokenDto : IMapFrom<GetDiscordTokenResponse>
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public void Mapping(Profile Profile)
        {
            Profile.CreateMap<GetDiscordTokenResponse, GetDiscordTokenDto>()
                .ForMember(m => m.AccessToken, m => m.MapFrom(o => o.access_token))
                .ForMember(m => m.TokenType, m => m.MapFrom(o => o.token_type))
                .ForMember(m => m.ExpiresIn, m => m.MapFrom(o => o.expires_in))
                .ForMember(m => m.RefreshToken, m => m.MapFrom(o => o.refresh_token))
                .ForMember(m => m.Scope, m => m.MapFrom(o => o.scope));
        }
    }
}
