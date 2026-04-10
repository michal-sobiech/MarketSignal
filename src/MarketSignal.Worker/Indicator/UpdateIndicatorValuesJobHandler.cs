using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Core.Indicator;

namespace MarketSignal.Worker.Indicator;

public class UpdateIndicatorValuesJobHandler(
    IndicatorValuesUpdater indicatorValuesUpdater,
    IJobStore jobStore
) {

    private readonly IndicatorValuesUpdater _indicatorValuesUpdater = indicatorValuesUpdater;
    private readonly IJobStore _jobStore = jobStore;

    public async Task HandleJob(Guid jobId, CalcIndicatorJobPayload jobPayload) {
        // TODO call indicatorValuesUpdater

        JobEntity entity = (await _jobStore.Fetch(jobId))!;
        JobEntity updatedEntity = entity with { JobStatus = JobStatus.SUCCESS };
        await _jobStore.Save(updatedEntity);

        await Task.CompletedTask;
    }

}