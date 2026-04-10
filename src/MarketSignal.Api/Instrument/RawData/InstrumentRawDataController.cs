using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;

using Microsoft.AspNetCore.Mvc;

namespace MarketSignal.Api.Instrument.RawData;

[ApiController]
[Route("api/instruments/raw-data")]
public class InstrumentRawDataController(
    IJobQueueProducer jobQueueProducer,
    IJobStore jobStore
) : ControllerBase {

    private readonly IJobQueueProducer _jobQueueProducer = jobQueueProducer;
    private readonly IJobStore _jobStore = jobStore;

    [HttpPost("pull")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<JobIdResponse> UpdateInstrumentRawDataAsync(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider
    ) {
        var dataProviderKind = Enum.Parse<InstrumentRawDataProviderKind>(dataProvider);

        InstrumentSpec instrumentSpec = new(symbol, mic, dataProviderKind);

        Guid jobId = Guid.NewGuid();
        var payload = new UpdateInstrumentRawDataJobPayload(instrumentSpec);
        JobEntity jobEntity = JobEntity.CreateNew(jobId, payload);

        _jobStore.Save(jobEntity);
        _jobQueueProducer.EnqueueJob(jobId);

        var response = new JobIdResponse(jobId);
        return Ok(response);
    }

}