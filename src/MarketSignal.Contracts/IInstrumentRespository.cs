using NodaTime;

namespace MarketSignal.Contracts;

public interface IInstrumentRawDataRepository {
    public Task<Instant> FetchNewestRowTime();
    public Task SaveMany(IEnumerable<InstrumentRawDataRow> rows);
}