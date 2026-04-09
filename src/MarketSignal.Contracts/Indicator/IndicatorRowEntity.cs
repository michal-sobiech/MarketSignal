using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.MarketDataProvider;

using NodaTime;

namespace MarketSignal.Contracts.Indicator;

public record IndicatorRowEntity(
    long RowId,
    long InstrumentSpecId,
    long IndicatorSpecId,
    Instant Time,
    double Value
);