using MarketSignal.Contracts.MarketDataProvider;

namespace MarketSignal.Contracts.Instrument;

public record InstrumentSpec(
    string Symbol,
    string Mic,
    MarketDataProviderKind DataProviderKind
);