using NodaTime;

namespace MarketSignal.Contracts.Instrument.RawData;

public interface IInstrumentRawDataProvider {

    public Task<IEnumerable<InstrumentRawDataRow>> FetchDailyRawData(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive);

}