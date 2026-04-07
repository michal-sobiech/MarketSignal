using MarketSignal.Application.Math;
using MarketSignal.Application.RawData;
using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.RawData;

using NodaTime;

namespace MarketSignal.Application.Indicator.Sma;

public class SmaUpdater(
    IndicatorService indicatorService,
    InstrumentRawDataService instrumentRawDataService,
    SimpleMovingAverageCalculator simpleMovingAverageCalculator
) {

    private readonly IndicatorService _indicatorService = indicatorService;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;
    private readonly SimpleMovingAverageCalculator _simpleMovingAverageCalculator = simpleMovingAverageCalculator;

    public async Task UpdateSma(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        InstrumentRawDataField rawDataField,
        int windowLength
    ) {
        var smaMissingTimeRange = await getMissingIndicatorTimeRange(instrumentIndicatorSpec);
        if (smaMissingTimeRange is null) {
            return;
        }
        var (missingSmaFrom, missingSmaTo) = smaMissingTimeRange.Value;

        Instant rawDataFrom = missingSmaFrom - Duration.FromDays(windowLength);
        IEnumerable<InstrumentRawDataRow> rawData = (await _rawDataService.FetchByTimeRange(
            instrumentIndicatorSpec.InstrumentSpec,
            rawDataFrom,
            missingSmaTo
        ))
            .Select(row => row.ToDomain());

        IEnumerable<Instant> times = rawData.Select(row => row.Time);

        List<double> rawDataFieldValues = rawData
            .Select(row => row.GetValue(rawDataField))
            .ToList();

        IEnumerable<double> smaValues = _simpleMovingAverageCalculator.CalculateSimpleMovingAverage(
            rawDataFieldValues,
            windowLength);

        IEnumerable<IndicatorRow> smaRows = times.Zip(
            smaValues,
            (time, smaValue) => new IndicatorRow(time, smaValue));

        await _indicatorService.SaveMany(instrumentIndicatorSpec, smaRows);
    }

    private async Task<(Instant from, Instant to)?> getMissingIndicatorTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec
    ) {
        Instant newestRawDataRowTime = await _rawDataService.FetchNewestRowTime(instrumentIndicatorSpec.InstrumentSpec);
        Instant newestIndicatorRowTime = await _indicatorService.FetchNewestRowTime(instrumentIndicatorSpec);

        return newestIndicatorRowTime < newestRawDataRowTime
            ? (newestIndicatorRowTime, newestRawDataRowTime)
            : null;
    }
}