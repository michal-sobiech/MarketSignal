using System.Text.Json.Serialization;

using MarketSignal.Contracts.Instrument.RawData;

namespace MarketSignal.Contracts.Indicator.Spec;

public sealed record SmaSpec : IndicatorSpec {

    [JsonConstructor]
    public SmaSpec(InstrumentRawDataField field, int period) {
        Field = field;
        Period = period;
    }

    public override IndicatorKind Kind => IndicatorKind.SMA;

    public InstrumentRawDataField Field { get; }
    public int Period { get; }

}