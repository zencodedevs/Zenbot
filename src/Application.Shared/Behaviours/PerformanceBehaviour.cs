using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.Application.Shared.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;

        public PerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,
              IConfiguration configuration)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _configuration = configuration;
            _currentUserService = currentUserService;

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            var milliseconds = _configuration.GetSection("Constants:PerformanceElapsedMilliseconds").Get<int>();

            if (elapsedMilliseconds > milliseconds)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId ?? string.Empty;
                var userName = "APP USER";

                _logger.LogWarning("Zenbot Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }
}
