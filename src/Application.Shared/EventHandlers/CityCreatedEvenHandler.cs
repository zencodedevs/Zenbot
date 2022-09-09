using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Zen.Domain.Events;
using ZenAchitecture.Domain.Shared.Events;

namespace ZenAchitecture.Application.Shared.EventHandlers
{
    public class CityCreatedEvenHandler : INotificationHandler<DomainEventNotification<CityCreatedEvent>>
    {
        private readonly ILogger<CityCreatedEvenHandler> _logger;

        public CityCreatedEvenHandler(ILogger<CityCreatedEvenHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<CityCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace("CityCreatedEvenHandler {@event}", notification);
        }
    }
}
