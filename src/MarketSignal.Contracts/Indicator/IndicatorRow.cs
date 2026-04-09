using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public record IndicatorRow(
    Instant Time,
    decimal Value
);