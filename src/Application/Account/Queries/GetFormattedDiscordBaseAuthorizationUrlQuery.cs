using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Zenbot.Domain.Interfaces;

namespace Zenbot.Application.Account.Queries
{
    public class GetFormattedDiscordBaseAuthorizationUrlQuery : IRequest<string> { }

    public class GetFormattedDiscordBaseAuthorizationUrlQueryHandler : IRequestHandler<GetFormattedDiscordBaseAuthorizationUrlQuery, string>
    {
        IDiscordAuthService _discordAuthService;

        public GetFormattedDiscordBaseAuthorizationUrlQueryHandler(IDiscordAuthService discordAuthService)
        {
            _discordAuthService = discordAuthService;
        }

        public async Task<string> Handle(GetFormattedDiscordBaseAuthorizationUrlQuery request, CancellationToken cancellationToken)
        {
            string formattedDiscordBaseAuthorizationUrl = string.Empty;

            await Task.Run(() =>
            {
                formattedDiscordBaseAuthorizationUrl = _discordAuthService.GetFormattedBaseAuthorizationUrl();
            });

            return formattedDiscordBaseAuthorizationUrl;
        }
    }
}
