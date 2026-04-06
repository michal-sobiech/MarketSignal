using NodaTime;

public interface IInstrumentDataProvider {
    public IEnumerable<InstrumentRawDataRow> fetchRawData(Instant from, Instant to);
}