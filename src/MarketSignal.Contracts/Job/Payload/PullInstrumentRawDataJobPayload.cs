using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Contracts.Job.Payload;

public record UpdateInstrumentRawDataJobPayload(
    InstrumentSpec InstrumentSpec
) : JobPayload;