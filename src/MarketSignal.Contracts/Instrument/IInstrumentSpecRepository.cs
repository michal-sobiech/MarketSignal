namespace MarketSignal.Contracts.Instrument;

public interface IInstrumentSpecRepository {
    public Task<long?> GetId(InstrumentSpec instrumentSpec);
    public Task<long> GetOrCreateId(InstrumentSpec instrumentSpec);
    public Task<long> Save(InstrumentSpec spec);
}