namespace MarketSignal.Contracts.Indicator.Spec;

public abstract record IndicatorSpec {
    public abstract IndicatorKind Kind { get; }
}