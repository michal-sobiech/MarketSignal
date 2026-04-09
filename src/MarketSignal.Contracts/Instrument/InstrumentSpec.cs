namespace MarketSignal.Contracts.Instrument;

public record InstrumentSpec(
    string Symbol,
    string Mic,
    InstrumentRawDataProviderKind DataProviderKind
);