using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Infrastructure.Indicator;

public class EfcoreIndicatorKindMapper {

    public static string ToString(IndicatorKind kind) {
        return kind switch {
            IndicatorKind.SMA => "SMA",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Invalid indicator kind")
        };
    }

}