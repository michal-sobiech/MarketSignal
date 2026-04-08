using MarketSignal.Api.Mappers;
using MarketSignal.Application;
using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.MarketDataProvider;

using Microsoft.AspNetCore.Mvc;

using NodaTime;

namespace MarketSignal.Api.InstrumentIndicator;

[ApiController]
[Route("/instruments/indicators")]
public class InstrumentIndicatorController(
    IndicatorService indicatorService,
    KeyValuePairsStringParser keyValuePairsStringParser
) : ControllerBase {

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

}