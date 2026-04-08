using MarketSignal.Contracts.DataProvider;
using MarketSignal.Contracts.Indicator.Spec;

using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public record IndicatorRowEntity(
    long RowId,
    IIndicatorSpec IndicatorSpec,
    DataProviderKind DataProviderKind,
    Instant Time,
    double Value
);