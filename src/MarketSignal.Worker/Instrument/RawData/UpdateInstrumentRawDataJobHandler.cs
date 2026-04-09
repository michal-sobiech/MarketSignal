using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Store;

namespace MarketSignal.Worker.Instrument.RawData;

public class UpdateInstrumentRawDataJobHandler(
    InstrumentRawDataUpdater instrumentRawDataUpdater,
    IJobStore jobStore
) {

    private readonly InstrumentRawDataUpdater _instrumentRawDataUpdater = instrumentRawDataUpdater;
    private readonly IJobStore _jobStore = jobStore;

    public async Task HandleJob(Guid jobId, UpdateInstrumentRawDataJobPayload payload) {
        // TODO
        // await _instrumentRawDataUpdater.UpdateInstrumentRawData(payload.InstrumentSpec);

        JobEntity entity = (await _jobStore.Fetch(jobId))!;
        JobEntity updatedEntity = entity with { JobStatus = JobStatus.SUCCESS };
        await _jobStore.Save(updatedEntity);

        await Task.CompletedTask;
    }

}