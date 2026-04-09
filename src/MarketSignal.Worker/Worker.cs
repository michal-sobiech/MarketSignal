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
    InstrumentRawDataUpdater instrumentRawDataUpdater,
    IndicatorValuesUpdater indicatorValuesUpdater,
    ILogger<Worker> logger
) : BackgroundService {

    private readonly IJobQueueConsumer _jobQueueConsumer = jobQueueConsumer;
    private readonly IJobStore _jobStore = jobStore;
    private readonly InstrumentRawDataUpdater _instrumentRawDataUpdater = instrumentRawDataUpdater;
    private readonly IndicatorValuesUpdater _indicatorValuesUpdater = indicatorValuesUpdater;
    // private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            // if (_logger.IsEnabled(LogLevel.Information)) {
            //     _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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
            UpdateInstrumentRawDataJobPayload payload => _instrumentRawDataUpdater.UpdateInstrumentRawData(payload.InstrumentSpec),
            CalcIndicatorJobPayload payload => _indicatorValuesUpdater.CalcAndUpdateIndicatorValues(payload.InstrumentIndicatorSpec),
            _ => throw new ArgumentException("Invalid payload")
        });
    }
}
