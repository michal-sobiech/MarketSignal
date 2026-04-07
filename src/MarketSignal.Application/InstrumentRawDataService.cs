using MarketSignal.Contracts;

using NodaTime;

namespace MarketSignal.Application;

public class InstrumentRawDataService(IInstrumentRawDataRepository instrumentRawDataRepository) {

    private readonly IInstrumentRawDataRepository _repository = instrumentRawDataRepository;

    public Task<Instant> FetchNewestRowTime() {
        return _repository.FetchNewestRowTime();
    }

    public Task SaveMany(IEnumerable<InstrumentRawDataRow> rows) {
        return _repository.SaveMany(rows);
    }
}