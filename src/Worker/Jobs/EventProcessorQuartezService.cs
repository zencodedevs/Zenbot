namespace Worker.Jobs
{
    using Microsoft.Extensions.Logging;
    using Microsoft.FeatureManagement;
    using Quartz;
    using System;
    using System.Threading.Tasks;
    using Zen.EventProcessor;

    [DisallowConcurrentExecution]
    public class EventProcessorQuartezService : IJob
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<EventProcessorQuartezService> _logger;
        private readonly IAsynchronousEventProcessor _asynchronousEventProcessor;

        public EventProcessorQuartezService(
            IFeatureManager featureManager
            , IAsynchronousEventProcessor asynchronousEventProcessor
            , ILogger<EventProcessorQuartezService> logger)
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
                    await _asynchronousEventProcessor.Process();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "EventProcessorQuartezService processing error ocuured!");
                }
            }
        }

    }
}
