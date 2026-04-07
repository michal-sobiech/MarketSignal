using NodaTime;

namespace MarketSignal.Contracts;

public interface IInstrumentRawDataProvider {
    public IEnumerable<InstrumentRawDataRow> FetchRawData(Instant from, Instant to);
}