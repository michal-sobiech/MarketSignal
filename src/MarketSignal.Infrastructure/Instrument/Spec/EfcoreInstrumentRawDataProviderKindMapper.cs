using MarketSignal.Contracts.Instrument.RawData;

namespace MarketSignal.Infrastructure.Instrument.Spec;

public class EfcoreInstrumentRawDataProviderKindMapper {

    public static string ToString(InstrumentRawDataProviderKind value) {
        return value switch {
            InstrumentRawDataProviderKind.ALPHA_VANTAGE => "alphaVantage",
            _ => throw new ArgumentException("Unknown value of instrument raw data provider kind")
        };
    }

}