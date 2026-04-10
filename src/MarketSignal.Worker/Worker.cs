using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Worker.Indicator;
using MarketSignal.Worker.Instrument.RawData;

namespace MarketSignal.Worker;

public class Worker(
    IJobQueueConsumer jobQueueConsumer,
    IJobStore jobStore,
    UpdateInstrumentRawDataJobHandler updateInstrumentRawDataJobHandler,
    UpdateIndicatorValuesJobHandler updateIndicatorValuesJobHandler,
    ILogger<Worker> logger
) : BackgroundService {

    private readonly IJobQueueConsumer _jobQueueConsumer = jobQueueConsumer;
    private readonly IJobStore _jobStore = jobStore;
    private readonly UpdateInstrumentRawDataJobHandler _updateInstrumentRawDataJobHandler = updateInstrumentRawDataJobHandler;
    private readonly UpdateIndicatorValuesJobHandler _updateIndicatorValuesJobHandler = updateIndicatorValuesJobHandler;
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            // if (_logger.IsEnabled(LogLevel.Information)) {
            //     _logger.LogInformation("Try to execute a job");
            // }
            await TryToDequeueAndExecuteJob();
            await Task.Delay(100, stoppingToken);
        }
    }

    private async Task TryToDequeueAndExecuteJob() {
        Guid? jobId = await _jobQueueConsumer.DequeueJob();
        if (jobId is Guid id) {
            await ExecuteJob(id);
        }
    }

    private async Task ExecuteJob(Guid jobId) {
        JobEntity? jobEntity = await _jobStore.Fetch(jobId);
        if (jobEntity is null) {
            return;
        }

        await (jobEntity.JobPayload switch {
            UpdateInstrumentRawDataJobPayload payload => _updateInstrumentRawDataJobHandler.HandleJob(jobId, payload),
            CalcIndicatorJobPayload payload => _updateIndicatorValuesJobHandler.HandleJob(jobId, payload),
            _ => throw new ArgumentOutOfRangeException(nameof(jobEntity.JobPayload), jobEntity.JobPayload, "Invalid payload")
        });
    }
}
