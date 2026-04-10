namespace MarketSignal.Contracts.Indicator.Spec;

public interface IIndicatorSpecRepository {
    public Task<long?> GetId(IndicatorSpec indicatorSpec);
    public Task SaveMany(IEnumerable<IndicatorSpec> specs);
}