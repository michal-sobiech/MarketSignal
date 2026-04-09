using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.MarketDataProvider;

using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public record IndicatorRowEntity(
    long RowId,
    IndicatorSpec IndicatorSpec,
    MarketDataProviderKind DataProviderKind,
    Instant Time,
    double Value
);