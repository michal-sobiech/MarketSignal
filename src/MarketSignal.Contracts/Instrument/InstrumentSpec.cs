using MarketSignal.Contracts.DataProvider;

namespace MarketSignal.Contracts.Instrument;

public record InstrumentSpec(
    string Symbol,
    string Mic,
    DataProviderKind DataProviderKind
);