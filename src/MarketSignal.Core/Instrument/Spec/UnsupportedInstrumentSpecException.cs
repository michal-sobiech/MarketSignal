using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument.Spec;

public class UnsupportedInstrumentSpecException : Exception {
    public InstrumentSpec Spec { get; }

    public UnsupportedInstrumentSpecException(
        InstrumentSpec spec
    ) : base($"Instance of {nameof(InstrumentSpec)} \"{spec}\" is not registered.") {
        Spec = spec;
    }
}