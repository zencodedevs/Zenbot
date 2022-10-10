using AutoMapper;
using MediatR;
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
        private readonly IMapper _mapper;

        public GetDiscordTokenQueryHandler(
            IDiscordAuthService discordAuthService,
            IMapper mapper)
        {
            _discordAuthService = discordAuthService;
            _mapper = mapper;
        }

        public async Task<GetDiscordTokenDto> Handle(GetDiscordTokenQuery request, CancellationToken cancellationToken)
        {
            var GetDiscordTokenRequest = new GetDiscordTokenRequest
            {
                Code = request.Code
            };
            var discordToken = _discordAuthService.GetDiscordToken(GetDiscordTokenRequest);

            var mapped = _mapper.Map<GetDiscordTokenDto>(discordToken);
            return mapped;
        }
    }
}
