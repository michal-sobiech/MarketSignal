using MarketSignal.Api.Mappers;
using MarketSignal.Application;
using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Contracts.MarketDataProvider;

using Microsoft.AspNetCore.Mvc;

using NodaTime;

namespace MarketSignal.Api.InstrumentIndicator;

[ApiController]
[Route("api/instruments/indicators")]
public class InstrumentIndicatorController(
    IJobQueueProducer jobQueueProducer,
    IJobStore jobStore,
    IndicatorService indicatorService,
    KeyValuePairsStringParser keyValuePairsStringParser
) : ControllerBase {

    private readonly IJobQueueProducer _jobQueueProducer = jobQueueProducer;
    private readonly IJobStore _jobStore = jobStore;
    private readonly IndicatorService _indicatorService = indicatorService;
    private readonly KeyValuePairsStringParser _keyValuePairsStringParser = keyValuePairsStringParser;

    [HttpGet("values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<GetIndicatorValuesResponse> GetIndicatorValues(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider,
        [FromQuery] string indicatorName,
        [FromQuery] string indicatorArgs,
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to
    ) {
        MarketDataProviderKind dataProviderKind = MarketDataProviderKindMapper.FromString(dataProvider);
        IndicatorKind indicatorKind = IndicatorKindMapper.FromString(indicatorName);
        Instant fromInstant = Instant.FromDateTimeOffset(from);
        Instant toInstant = Instant.FromDateTimeOffset(to);

        var indicatorArgsDict = _keyValuePairsStringParser.Parse(indicatorArgs);
        IndicatorSpec indicatorSpec = IndicatorSpecFactory.Of(indicatorKind, indicatorArgsDict);

        var instrumentIndicatorSpec = InstrumentIndicatorSpec.Of(symbol, mic, dataProviderKind, indicatorSpec);

        var rows = _indicatorService.FetchByTimeRange(
            instrumentIndicatorSpec,
            fromInstant,
            toInstant);
        return Ok(rows);
    }

    [HttpPost("calculate-values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<JobIdResponse> CalcIndicatorValues(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider,
        [FromQuery] string indicatorName,
        [FromQuery] string indicatorArgs
    ) {
        MarketDataProviderKind dataProviderKind = MarketDataProviderKindMapper.FromString(dataProvider);
        IndicatorKind indicatorKind = IndicatorKindMapper.FromString(indicatorName);

        var indicatorArgsDict = _keyValuePairsStringParser.Parse(indicatorArgs);
        IndicatorSpec indicatorSpec = IndicatorSpecFactory.Of(indicatorKind, indicatorArgsDict);

        var instrumentIndicatorSpec = InstrumentIndicatorSpec.Of(symbol, mic, dataProviderKind, indicatorSpec);

        Guid jobId = new();
        var payload = new CalcIndicatorJobPayload(instrumentIndicatorSpec);
        JobEntity jobEntity = JobEntity.CreateNew(jobId, payload);

        _jobStore.Save(jobEntity);
        _jobQueueProducer.EnqueueJob(jobId);

        var response = new JobIdResponse(jobId);
        return Ok(response);
    }

}