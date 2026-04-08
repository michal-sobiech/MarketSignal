using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Api.Mappers;

public class IndicatorKindMapper {

    public static IndicatorKind FromString(string value) => value switch {
        "SMA" => IndicatorKind.SMA,
        "RSI" => IndicatorKind.RSI,
        _ => throw new ArgumentException("Invalid indicator kind")
    };

}