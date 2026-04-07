using MarketSignal.Contracts.Indicator.Spec;

using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public interface IIndicatorRepository {
    public Task<Instant> FetchNewestRowTime(IIndicatorSpec indicatorSpec);
    public Task SaveMany(IIndicatorSpec indicatorSpec, IEnumerable<IndicatorRow> rows);
    public Task<IEnumerable<IndicatorRowStored>> FetchByTimeRange(Instant fromInclusive, Instant toInclusive);
}