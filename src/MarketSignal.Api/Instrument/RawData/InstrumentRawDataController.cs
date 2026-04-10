using MarketSignal.Api.Exceptions;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Core.Instrument.Spec;

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

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<JobIdResponse> UpdateInstrumentRawDataAsync(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider
    ) {
        if (!Enum.TryParse<InstrumentRawDataProviderKind>(dataProvider, out var dataProviderKind))
            throw new InvalidRequestException($"Invalid data provider: {dataProvider}");

        InstrumentSpec instrumentSpec = new(symbol, mic, dataProviderKind);
        SupportedInstrumentSpecRegistry.AssertHasInstrumentSpec(instrumentSpec);

        Guid jobId = Guid.NewGuid();
        var payload = new UpdateInstrumentRawDataJobPayload(instrumentSpec);
        JobEntity jobEntity = JobEntity.CreateNew(jobId, payload);

        _jobStore.Save(jobEntity);
        _jobQueueProducer.EnqueueJob(jobId);

        var response = new JobIdResponse(jobId);
        return Ok(response);
    }

}