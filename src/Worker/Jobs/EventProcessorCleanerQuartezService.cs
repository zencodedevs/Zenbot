using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Quartz;
using System;
using System.Threading.Tasks;
using Zen.EventProcessor;

namespace Worker.Jobs
{
    [DisallowConcurrentExecution]
    public class EventProcessorCleanerQuartezService : IJob
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<EventProcessorCleanerQuartezService> _logger;
        private readonly IAsynchronousEventProcessor _asynchronousEventProcessor;

        public EventProcessorCleanerQuartezService(
            IFeatureManager featureManager
            , IAsynchronousEventProcessor asynchronousEventProcessor
            , ILogger<EventProcessorCleanerQuartezService> logger)
        {
            _logger = logger;
            _featureManager = featureManager;
            _asynchronousEventProcessor = asynchronousEventProcessor;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (await _featureManager.IsEnabledAsync("EventProcessorQuartezService"))
            {
                try
                {
                    await _asynchronousEventProcessor.RemoveProcessed();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "EventProcessorCleanerQuartezService processing error ocuured!");
                }

            }
        }

    }
}
