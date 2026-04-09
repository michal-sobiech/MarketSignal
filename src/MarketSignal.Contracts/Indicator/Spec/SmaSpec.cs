namespace MarketSignal.Contracts.Indicator.Spec;

public sealed record SmaSpec(
    int Period
) : IndicatorSpec {

    public override IndicatorKind Kind => IndicatorKind.SMA;

}