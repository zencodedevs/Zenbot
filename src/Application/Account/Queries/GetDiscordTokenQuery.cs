using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Zenbot.Application.Account.Dtos;
using Zenbot.Domain.Interfaces;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Application.Account.Queries
{
    public class GetDiscordTokenQuery : IRequest<GetDiscordTokenDto>
    {
        public string Code { get; set; }
    }

    public class GetDiscordTokenQueryHandler : IRequestHandler<GetDiscordTokenQuery, GetDiscordTokenDto>
    {
        IDiscordAuthService _discordAuthService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetDiscordTokenQueryHandler(
            IDiscordAuthService discordAuthService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _discordAuthService = discordAuthService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<GetDiscordTokenDto> Handle(GetDiscordTokenQuery request, CancellationToken cancellationToken)
        {
            var clientId = _configuration.GetSection("DiscordAuth")["ClientId"];
            var clientSecret = _configuration.GetSection("DiscordAuth")["ClientSecret"];

            var requestHostUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
            var redirectActionUrl = _configuration.GetSection("DiscordAuth")["RedirectActionUrl"];
            var redirectUrl = "https://" + requestHostUrl + redirectActionUrl;

            var GetDiscordTokenRequest = new GetDiscordTokenRequest
            {
                client_id = clientId,
                client_secret = clientSecret,
                code = request.Code,
                redirect_uri = redirectUrl
            };
            var discordToken = _discordAuthService.GetDiscordToken(GetDiscordTokenRequest);

            var mapped = _mapper.Map<GetDiscordTokenDto>(discordToken);
            return mapped;
        }
    }
}
