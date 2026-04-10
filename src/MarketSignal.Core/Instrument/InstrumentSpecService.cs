using MarketSignal.Contracts.Instrument;

namespace MarketSignal.Core.Instrument;

public class InstrumentSpecService(
    IInstrumentSpecRepository instrumentSpecRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;

    public Task SaveMany(IEnumerable<InstrumentSpec> specs) {
        return _instrumentSpecRepository.SaveMany(specs);
    }

}