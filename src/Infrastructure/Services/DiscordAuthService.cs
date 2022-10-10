using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Zenbot.Domain.Interfaces;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Infrastructure.Services
{
    public class DiscordAuthService : IDiscordAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiscordAuthService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetFormattedBaseAuthorizationUrl()
        {
            var discordBaseUrl = _configuration.GetSection("DiscordBase")["BaseUrl"];
            var clientId = _configuration.GetSection("DiscordAuth")["ClientId"];
            var baseAuthorizationUrl = _configuration.GetSection("DiscordAuth")["BaseAuthorizationUrl"];
            var baseAuthorizationParamsFormat = _configuration.GetSection("DiscordAuth")["BaseAuthorizationParamsFormat"];

            var requestHostUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
            var redirectActionUrl = _configuration.GetSection("DiscordAuth")["RedirectActionUrl"];
            var redirectUrl = "https://" + requestHostUrl + redirectActionUrl;

            var formattedBaseAuthorizationUrl =
                discordBaseUrl +
                baseAuthorizationUrl +
                baseAuthorizationParamsFormat
                .Replace("{clientId}", clientId)
                .Replace("{redirectUrl}", redirectUrl);

            return formattedBaseAuthorizationUrl;
        }

        public async Task<GetDiscordTokenResponse> GetDiscordToken(GetDiscordTokenRequest getDiscordTokenRequest)
        {
            return default;
        }
    }
}
