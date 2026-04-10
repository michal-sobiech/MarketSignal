using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument;

public class InstrumentSpecNotFoundException : Exception {

    public InstrumentSpecNotFoundException(InstrumentSpec spec) : base($"Instrument spec not found: {spec}") { }

}