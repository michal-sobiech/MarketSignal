using System.Text.Json.Serialization;

namespace MarketSignal.Contracts.Indicator.Spec;

public sealed record SmaSpec : IndicatorSpec {
    [JsonConstructor]
    public SmaSpec(int period) { Period = period; }
    public int Period { get; }
    public override IndicatorKind Kind => IndicatorKind.SMA;
}