using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

using NodaTime;

namespace MarketSignal.Application.RawData;

public class InstrumentRawDataService(IInstrumentRawDataRepository instrumentRawDataRepository) {

    private readonly IInstrumentRawDataRepository _repository = instrumentRawDataRepository;

    public Task<Instant?> FetchNewestRowTime(InstrumentSpec instrumentSpec) {
        return _repository.FetchNewestRowTime(instrumentSpec);
    }

    public Task SaveMany(
        InstrumentSpec instrumentSpec,
        IEnumerable<InstrumentRawDataRow> rows
    ) {
        return _repository.SaveMany(instrumentSpec, rows);
    }

    public Task<IEnumerable<InstrumentRawDataRowEntity>> FetchByTimeRange(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        return _repository.FetchByTimeRange(instrumentSpec, fromInclusive, toInclusive);
    }
}