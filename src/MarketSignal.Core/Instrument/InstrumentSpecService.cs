using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument;

public class InstrumentSpecService(
    IInstrumentSpecRepository instrumentSpecRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;

    public Task Save(InstrumentSpec spec) {
        return _instrumentSpecRepository.Save(spec);
    }

}