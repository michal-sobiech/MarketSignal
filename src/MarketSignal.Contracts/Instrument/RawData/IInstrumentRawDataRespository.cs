using NodaTime;

namespace MarketSignal.Contracts.Instrument.RawData;

public interface IInstrumentRawDataRepository {
    public Task<Instant?> FetchNewestRowTime(InstrumentSpec instrumentSpec);

    public Task SaveMany(
        InstrumentSpec instrumentSpec,
        IEnumerable<InstrumentRawDataRow> rows
    );

    public Task<IEnumerable<InstrumentRawDataRow>> FetchByTimeRange(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive
    );
}