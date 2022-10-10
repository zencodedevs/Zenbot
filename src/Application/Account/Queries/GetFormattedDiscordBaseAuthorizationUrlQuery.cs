using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Zenbot.Application.Account.Queries
{
    public class GetFormattedDiscordBaseAuthorizationUrlQuery : IRequest<string> { }

    public class GetFormattedDiscordBaseAuthorizationUrlQueryHandler : IRequestHandler<GetFormattedDiscordBaseAuthorizationUrlQuery, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetFormattedDiscordBaseAuthorizationUrlQueryHandler(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Handle(GetFormattedDiscordBaseAuthorizationUrlQuery request, CancellationToken cancellationToken)
        {
            string formattedDiscordBaseAuthorizationUrl = string.Empty;

            await Task.Run(() =>
            {
                var discordBaseUrl = _configuration.GetSection("DiscordBase")["BaseUrl"];
                var clientId = _configuration.GetSection("DiscordAuth")["ClientId"];
                var baseAuthorizationUrl = _configuration.GetSection("DiscordAuth")["BaseAuthorizationUrl"];
                var baseAuthorizationParamsFormat = _configuration.GetSection("DiscordAuth")["BaseAuthorizationParamsFormat"];

                var requestHostUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                var redirectActionUrl = _configuration.GetSection("DiscordAuth")["RedirectActionUrl"];
                var redirectUrl = "https://" + requestHostUrl + redirectActionUrl;

                var formattedDiscordBaseAuthorizationUrl =
                    discordBaseUrl +
                    baseAuthorizationUrl +
                    baseAuthorizationParamsFormat
                    .Replace("{clientId}", clientId)
                    .Replace("{redirectUrl}", redirectUrl);
            });

            return formattedDiscordBaseAuthorizationUrl;
        }
    }
}
