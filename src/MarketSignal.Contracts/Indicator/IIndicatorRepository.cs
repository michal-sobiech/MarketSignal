using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public interface IIndicatorRepository {
    public Task<Instant> FetchNewestRowTime(InstrumentIndicatorSpec indicatorSpec);

    public Task SaveMany(InstrumentIndicatorSpec indicatorSpec, IEnumerable<IndicatorRow> rows);

    public Task<IEnumerable<IndicatorRowEntity>> FetchByTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        Instant fromInclusive,
        Instant toInclusive);
}