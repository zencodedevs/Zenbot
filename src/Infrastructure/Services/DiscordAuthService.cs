using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Zenbot.Domain.Interfaces;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Infrastructure.Services
{
    public class DiscordAuthService : IDiscordAuthService
    {
        private readonly IConfiguration _configuration;

        public DiscordAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GetDiscordTokenResponse> GetDiscordTokenAsync(GetDiscordTokenRequest getDiscordTokenRequest)
        {
            return default;
        }
    }
}
