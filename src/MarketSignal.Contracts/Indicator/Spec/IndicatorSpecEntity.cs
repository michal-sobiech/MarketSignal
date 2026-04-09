namespace MarketSignal.Contracts.Indicator.Spec;

public record IndicatorSpecEntity(
    long RowId,
    string IndictorName,
    string IndicatorArgsJson
);