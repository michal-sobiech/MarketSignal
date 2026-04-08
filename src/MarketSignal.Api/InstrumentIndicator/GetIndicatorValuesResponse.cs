namespace MarketSignal.Api.InstrumentIndicator;

public record GetIndicatorValuesResponse(
    IEnumerable<(DateTimeOffset time, double value)> Rows
);