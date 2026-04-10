using System.Text.Json.Serialization;

namespace MarketSignal.Contracts.Indicator.Spec;

// public sealed record SmaSpec(
//     int Period
// ) : IndicatorSpec {

//     [JsonConstructor]
//     public SmaSpec(int period) { this.Period = period; }

//     public override IndicatorKind Kind => IndicatorKind.SMA;

// }


public sealed record SmaSpec : IndicatorSpec {
    [JsonConstructor]
    public SmaSpec(int period) { Period = period; }
    public int Period { get; }
    public override IndicatorKind Kind => IndicatorKind.SMA;
}