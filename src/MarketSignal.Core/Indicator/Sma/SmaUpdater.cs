using MarketSignal.Core.Math;
using MarketSignal.Core.Instrument.RawData;

using NodaTime;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Core.Instrument.Spec;

namespace MarketSignal.Core.Indicator.Sma;

public class SmaUpdater(
    InstrumentSpecService instrumentSpecService,
    IndicatorSpecService indicatorSpecService,
    IndicatorService indicatorService,
    InstrumentRawDataService instrumentRawDataService,
    SimpleMovingAverageCalculator simpleMovingAverageCalculator
) {

    private readonly InstrumentSpecService _instrumentSpecService = instrumentSpecService;
    private readonly IndicatorSpecService _indicatorSpecService = indicatorSpecService;
    private readonly IndicatorService _indicatorService = indicatorService;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;
    private readonly SimpleMovingAverageCalculator _simpleMovingAverageCalculator = simpleMovingAverageCalculator;

    public async Task UpdateSma(InstrumentSpec instrumentSpec, SmaSpec smaSpec) {
        var smaMissingTimeRange = await GetMissingIndicatorTimeRange(instrumentSpec, smaSpec);
        if (smaMissingTimeRange is null) {
            return;
        }
        var (missingSmaFrom, missingSmaTo) = smaMissingTimeRange.Value;

        Instant rawDataFrom = missingSmaFrom == Instant.MinValue
            ? Instant.MinValue
            : missingSmaFrom - Duration.FromDays(smaSpec.Period);
        IEnumerable<InstrumentRawDataRow> rawData = await _rawDataService.FetchByTimeRange(
            instrumentSpec,
            rawDataFrom,
            missingSmaTo);

        IEnumerable<Instant> times = rawData.Select(row => row.Time);

        List<decimal> rawDataFieldValues = rawData
            .Select(row => row.GetValue(smaSpec.Field))
            .ToList();

        IEnumerable<decimal> smaValues = _simpleMovingAverageCalculator.CalculateSimpleMovingAverage(
            rawDataFieldValues,
            smaSpec.Period);

        IEnumerable<IndicatorRow> smaRows = times.Zip(
            smaValues,
            (time, smaValue) => new IndicatorRow(time, smaValue));

        InstrumentIndicatorSpec instrumentIndicatorSpec = new(instrumentSpec, smaSpec);
        await _indicatorService.SaveMany(instrumentIndicatorSpec, smaRows);
    }

    private async Task<(Instant from, Instant to)?> GetMissingIndicatorTimeRange(
        InstrumentSpec instrumentSpec,
        SmaSpec smaSpec
    ) {
        InstrumentIndicatorSpec instrumentIndicatorSpec = new(instrumentSpec, smaSpec);

        Instant newestRawDataRowTime = await _instrumentSpecService.Exists(instrumentSpec)
            ? await _rawDataService.FetchNewestRowTime(instrumentSpec) ?? Instant.MinValue
            : Instant.MinValue;

        Instant newestIndicatorRowTime = await _indicatorSpecService.Exists(smaSpec)
            ? await _indicatorService.FetchNewestRowTime(instrumentIndicatorSpec) ?? Instant.MinValue
            : Instant.MinValue;

        return newestIndicatorRowTime < newestRawDataRowTime
            ? (newestIndicatorRowTime, newestRawDataRowTime)
            : null;
    }
}