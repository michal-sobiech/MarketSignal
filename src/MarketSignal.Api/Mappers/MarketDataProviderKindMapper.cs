
using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Api;

public class MarketDataProviderKindMapper {

    public static InstrumentRawDataProviderKind FromString(string value) => value switch {
        "alphaVantage" => InstrumentRawDataProviderKind.ALPHA_VANTAGE,
        _ => throw new ArgumentException("Invalid market data provider")
    };

}