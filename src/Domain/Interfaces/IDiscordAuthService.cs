using System.Threading.Tasks;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Domain.Interfaces
{
    public interface IDiscordAuthService
    {
        public Task<GetDiscordTokenResponse> GetDiscordToken(GetDiscordTokenRequest getDiscordTokenRequest);
    }
}
