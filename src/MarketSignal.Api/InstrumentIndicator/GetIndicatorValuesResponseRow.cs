namespace MarketSignal.Api.InstrumentIndicator;

public record GetIndicatorValuesResponseRow(
    DateTimeOffset Time,
    decimal Value
);