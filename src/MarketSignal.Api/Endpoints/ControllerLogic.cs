using MarkeSignal.Infrastructure.Job;

using MarketSignal.Api.Generated;
using MarketSignal.Application.RawData;
using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Store;

namespace MarketSignal.Api.Endpoints;

public class ControllerLogic(
    InstrumentRawDataUpdater instrumentRawDataUpdater,
    IJobQueueProducer jobQueueProducer,
    IJobStore jobStore
) : IController {

    private readonly InstrumentRawDataUpdater _instrumentRawDataUpdater = instrumentRawDataUpdater;
    private readonly IJobQueueProducer _jobQueueProducer = jobQueueProducer;
    private readonly IJobStore _jobStore = jobStore;

    public Task<Response> PullInstrumentRawDataAsync(
        InstrumentId instrumentId,
        string dataProvider,
        DateTimeOffset? from,
        DateTimeOffset? to
    ) {
        Guid jobId = new();
        JobEntity jobEntity = JobEntity.createNew(jobId, JobKind.UPDATE_INSTRUMENT_RAW_DATA);

        _jobStore.Save(jobEntity);
        _jobQueueProducer.EnqueueJob(jobId);

        Response response = new() { JobId = jobId.ToString() };
        return Task.FromResult(response);
    }

    public Task<GetIndicatorValuesResponse> GetIndicatorValuesAsync(
        InstrumentId instrumentId,
        string indicator,
        string dataProvider,
        DateTimeOffset? from,
        DateTimeOffset? to
    ) {

    }

    public Task<Response2> CalculateIndicatorValuesAsync(
        InstrumentId instrumentId,
        string indicator,
        string dataProvider
    ) {

    }

    public Task<GetJobResultResponse> GetCalculateIndicatorValuesJobResultAsync(
        double jobId
    ) {

    }
    public Task<GetJobResultResponse> GetExternalDataPullJobResultAsync(
        double jobId
    ) {

    }

}