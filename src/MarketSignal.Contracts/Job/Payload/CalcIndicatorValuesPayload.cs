using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Contracts.Job.Payload;

public record CalcIndicatorJobPayload(
    InstrumentIndicatorSpec InstrumentIndicatorSpec
) : JobPayload;