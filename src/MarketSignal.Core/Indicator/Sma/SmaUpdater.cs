using MarketSignal.Core.Math;
using MarketSignal.Core.Instrument.RawData;

using NodaTime;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Core.Indicator.Sma;

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
        int period
    ) {
        var smaMissingTimeRange = await GetMissingIndicatorTimeRange(instrumentIndicatorSpec);
        if (smaMissingTimeRange is null) {
            return;
        }
        var (missingSmaFrom, missingSmaTo) = smaMissingTimeRange.Value;

        Instant rawDataFrom = missingSmaFrom - Duration.FromDays(period);
        IEnumerable<InstrumentRawDataRow> rawData = await _rawDataService.FetchByTimeRange(
            instrumentIndicatorSpec.InstrumentSpec,
            rawDataFrom,
            missingSmaTo);

        IEnumerable<Instant> times = rawData.Select(row => row.Time);

        List<decimal> rawDataFieldValues = rawData
            .Select(row => row.GetValue(rawDataField))
            .ToList();

        IEnumerable<decimal> smaValues = _simpleMovingAverageCalculator.CalculateSimpleMovingAverage(
            rawDataFieldValues,
            period);

        IEnumerable<IndicatorRow> smaRows = times.Zip(
            smaValues,
            (time, smaValue) => new IndicatorRow(time, smaValue));

        await _indicatorService.SaveMany(instrumentIndicatorSpec, smaRows);
    }

    private async Task<(Instant from, Instant to)?> GetMissingIndicatorTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec
    ) {
        Instant? newestRawDataRowTime = await _rawDataService.FetchNewestRowTime(instrumentIndicatorSpec.InstrumentSpec);
        Instant? newestIndicatorRowTime = await _indicatorService.FetchNewestRowTime(instrumentIndicatorSpec);

        if (newestRawDataRowTime is null) {
            return null;
        }

        if (newestIndicatorRowTime is null) {
            return (Instant.MinValue, newestRawDataRowTime.Value);
        }

        return newestIndicatorRowTime < newestRawDataRowTime
            ? (newestIndicatorRowTime.Value, newestRawDataRowTime.Value)
            : null;
    }
}