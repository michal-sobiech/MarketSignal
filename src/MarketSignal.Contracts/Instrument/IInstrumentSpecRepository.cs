namespace MarketSignal.Contracts.Instrument;

public interface IInstrumentSpecRepository {
    public Task<long?> GetId(InstrumentSpec instrumentSpec);
    public Task SaveMany(IEnumerable<InstrumentSpec> specs);
}