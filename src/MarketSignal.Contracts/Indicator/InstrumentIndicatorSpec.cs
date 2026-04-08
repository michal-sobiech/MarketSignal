using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.MarketDataProvider;

namespace MarketSignal.Contracts.Indicator;

public record InstrumentIndicatorSpec(
    InstrumentSpec InstrumentSpec,
    IIndicatorSpec IndicatorSpec
) {
    public static Create(
        string symbol,
        string mic,
        MarketDataProviderKind dataProviderKind,
        IIndicatorSpec indicatorSpec
    ) {
        retu
    }
}