using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument.Spec;

public class InstrumentSpecService(
    IInstrumentSpecRepository instrumentSpecRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;

    public async Task<long?> GetId(InstrumentSpec spec) {
        return await _instrumentSpecRepository.GetId(spec);
    }

    public async Task<bool> Exists(InstrumentSpec spec) {
        return await GetId(spec) is { };
    }

    public Task Save(InstrumentSpec spec) {
        return _instrumentSpecRepository.Save(spec);
    }

    public Task<long> GetOrCreateId(InstrumentSpec spec) {
        return _instrumentSpecRepository.GetOrCreateId(spec);
    }

}