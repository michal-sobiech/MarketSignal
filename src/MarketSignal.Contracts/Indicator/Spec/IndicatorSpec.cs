using System.Text.Json.Serialization;

namespace MarketSignal.Contracts.Indicator.Spec;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type_")]
[JsonDerivedType(typeof(SmaSpec), "SMA")]
public abstract record IndicatorSpec {
    public abstract IndicatorKind Kind { get; }
}