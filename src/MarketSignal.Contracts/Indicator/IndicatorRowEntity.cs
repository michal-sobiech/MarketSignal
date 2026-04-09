using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public record IndicatorRowEntity(
    long RowId,
    long InstrumentSpecId,
    long IndicatorSpecId,
    Instant Time,
    decimal Value
);