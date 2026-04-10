using MarketSignal.Api.Exceptions;

using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Job;
using MarketSignal.Contracts.Job.Payload;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Core.Indicator;
using MarketSignal.Core.Instrument.Spec;
using MarketSignal.Core.KeyValuePairsStringParser;

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetIndicatorValuesResponse>> GetIndicatorValues(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider,
        [FromQuery] string indicatorName,
        [FromQuery] string indicatorArgs,
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to
    ) {
        var instrumentIndicatorSpec = ParseInstrumentIndicatorSpec(symbol, mic, dataProvider, indicatorName, indicatorArgs);

        Instant fromInstant = Instant.FromDateTimeOffset(from);
        Instant toInstant = Instant.FromDateTimeOffset(to);

        var rows = (await _indicatorService.FetchByTimeRange(
            instrumentIndicatorSpec,
            fromInstant,
            toInstant))
            .Select(x => new GetIndicatorValuesResponseRow(x.Time.ToDateTimeOffset(), x.Value))
            .ToList();

        var response = new GetIndicatorValuesResponse(rows);

        return Ok(response);
    }

    [HttpPost("calculate-values")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<JobIdResponse>> CalcIndicatorValues(
        [FromQuery] string symbol,
        [FromQuery] string mic,
        [FromQuery] string dataProvider,
        [FromQuery] string indicatorName,
        [FromQuery] string indicatorArgs
    ) {
        var instrumentIndicatorSpec = ParseInstrumentIndicatorSpec(symbol, mic, dataProvider, indicatorName, indicatorArgs);

        Guid jobId = Guid.NewGuid();
        var payload = new CalcIndicatorJobPayload(instrumentIndicatorSpec);
        JobEntity jobEntity = JobEntity.CreateNew(jobId, payload);

        await _jobStore.Save(jobEntity);
        await _jobQueueProducer.EnqueueJob(jobId);

        var response = new JobIdResponse(jobId);
        return Ok(response);
    }

    private InstrumentIndicatorSpec ParseInstrumentIndicatorSpec(
        string symbol,
        string mic,
        string dataProvider,
        string indicatorName,
        string indicatorArgs
    ) {
        if (!Enum.TryParse<InstrumentRawDataProviderKind>(dataProvider, out var dataProviderKind))
            throw new InvalidRequestException($"Invalid data provider: {dataProvider}");

        if (!Enum.TryParse<IndicatorKind>(indicatorName, out var indicatorKind))
            throw new InvalidRequestException($"Invalid indicator name: {indicatorName}");

        Dictionary<string, string> indicatorArgsDict = ParseIndicatorArgsDict(indicatorArgs);

        IndicatorSpec indicatorSpec = IndicatorSpecFactory.Of(indicatorKind, indicatorArgsDict);
        var instrumentIndicatorSpec = InstrumentIndicatorSpec.Of(symbol, mic, dataProviderKind, indicatorSpec);

        SupportedInstrumentSpecRegistry.AssertHasInstrumentSpec(instrumentIndicatorSpec.InstrumentSpec);

        return instrumentIndicatorSpec;
    }

    private Dictionary<string, string> ParseIndicatorArgsDict(string value) {
        try {
            return _keyValuePairsStringParser.Parse(value);
        }
        catch {
            throw new InvalidRequestException($"Invalid indicator args");
        }
    }

}