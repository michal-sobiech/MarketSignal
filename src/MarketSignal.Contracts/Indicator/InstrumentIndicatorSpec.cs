using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Contracts.Indicator;

public record InstrumentIndicatorSpec(
    InstrumentSpec InstrumentSpec,
    IndicatorSpec IndicatorSpec
) {
    public static InstrumentIndicatorSpec Of(
        string symbol,
        string mic,
        InstrumentRawDataProviderKind dataProviderKind,
        IndicatorSpec indicatorSpec
    ) {
        InstrumentSpec instrumentSpec = new(symbol, mic, dataProviderKind);
        return new InstrumentIndicatorSpec(instrumentSpec, indicatorSpec);
    }
}