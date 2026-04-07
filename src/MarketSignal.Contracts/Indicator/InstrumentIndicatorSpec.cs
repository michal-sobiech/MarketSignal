using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Contracts.Indicator;

public record InstrumentIndicatorSpec(
    InstrumentSpec InstrumentSpec,
    IIndicatorSpec IndicatorSpec
);