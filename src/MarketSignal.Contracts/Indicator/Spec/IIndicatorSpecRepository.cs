namespace MarketSignal.Contracts.Indicator.Spec;

public interface IIndicatorSpecRepository {
    public Task<long?> GetId(IndicatorSpec indicatorSpec);
    public Task<long> GetOrCreateId(IndicatorSpec indicatorSpec);
    public Task<long> Save(IndicatorSpec spec);
}