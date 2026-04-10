using NodaTime;

namespace MarketSignal.Contracts.Instrument.RawData;

public interface IInstrumentRawDataRepository {
    public Task<Instant?> FetchNewestRowTime(long instrumentSpecId);

    public Task SaveMany(
        long instrumentSpecId,
        IEnumerable<InstrumentRawDataRow> rows
    );

    public Task<IEnumerable<InstrumentRawDataRow>> FetchByTimeRange(
        long instrumentSpecId,
        Instant fromInclusive,
        Instant toInclusive
    );
}