using MarketSignal.Contracts.Instrument;

using NodaTime;

namespace MarketSignal.Contracts.RawData;

public interface IInstrumentRawDataRepository {
    public Task<Instant> FetchNewestRowTime(InstrumentSpec instrumentSpec);

    public Task SaveMany(
        InstrumentSpec instrumentSpec,
        IEnumerable<InstrumentRawDataRow> rows
    );

    public Task<IEnumerable<InstrumentRawDataRowStored>> FetchByTimeRange(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive
    );
}