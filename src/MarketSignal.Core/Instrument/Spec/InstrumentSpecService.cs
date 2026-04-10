using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument.Spec;

public class InstrumentSpecService(
    IInstrumentSpecRepository instrumentSpecRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;

    public async Task<bool> Exists(InstrumentSpec spec) {
        return await _instrumentSpecRepository.GetId(spec) is { };
    }

    public Task Save(InstrumentSpec spec) {
        return _instrumentSpecRepository.Save(spec);
    }

}