using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public interface IIndicatorRepository {
    public Task<Instant?> FetchNewestRowTime(long instrumentSpecId, long indicatorSpecId);

    public Task SaveMany(long instrumentSpecId, long indicatorSpecId, IEnumerable<IndicatorRow> rows);

    public Task<IEnumerable<IndicatorRow>> FetchByTimeRange(
        long instrumentSpecId,
        long indicatorSpecId,
        Instant fromInclusive,
        Instant toInclusive);
}