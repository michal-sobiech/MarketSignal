using MarketSignal.Contracts.MarketDataProvider;

namespace MarketSignal.Api;

public class MarketDataProviderKindMapper {

    public static MarketDataProviderKind FromString(string value) => value switch {
        "alphaVantage" => MarketDataProviderKind.ALPHA_VANTAGE,
        _ => throw new ArgumentException("Invalid market data provider")
    };

}