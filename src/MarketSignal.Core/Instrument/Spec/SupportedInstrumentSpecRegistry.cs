using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

namespace MarketSignal.Core.Instrument.Spec;

public static class SupportedInstrumentSpecRegistry {

    public static readonly HashSet<InstrumentSpec> InstrumentSpecs = [
        new InstrumentSpec("TSCO", "XLON", InstrumentRawDataProviderKind.ALPHA_VANTAGE),
        new InstrumentSpec("TSCDF", "OTCM", InstrumentRawDataProviderKind.ALPHA_VANTAGE),
        new InstrumentSpec("TCO2", "XFRA", InstrumentRawDataProviderKind.ALPHA_VANTAGE),
        new InstrumentSpec("VOD", "XLON", InstrumentRawDataProviderKind.ALPHA_VANTAGE),
        new InstrumentSpec("IDEA", "XBOM", InstrumentRawDataProviderKind.ALPHA_VANTAGE)
    ];

    public static void AssertHasInstrumentSpec(InstrumentSpec spec) {
        if (!InstrumentSpecs.Contains(spec)) {
            throw new InvalidOperationException($"Instrument spec \"{spec}\" is not registered.");
        }
    }

}