namespace MarketSignal.Api.InstrumentIndicator;

public record GetIndicatorValuesResponse(
    List<GetIndicatorValuesResponseRow> Rows
);