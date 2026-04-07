using MarketSignal.Contracts.RawData;

using NodaTime;

namespace MarketSignal.Contracts;

public interface IInstrumentRawDataProvider {
    public IEnumerable<InstrumentRawDataRow> FetchRawData(Instant fromInclusive, Instant toInclusive);
}