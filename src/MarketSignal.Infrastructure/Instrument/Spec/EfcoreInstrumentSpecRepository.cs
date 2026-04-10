using MarketSignal.Contracts.Instrument;
using MarketSignal.Infrastructure.MarketDb;

namespace MarketSignal.Infrastructure.Instrument.Spec;

public class EfcoreInstrumentSpecRepository(
    MarketDbContext dbContext
) : IInstrumentSpecRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public async Task<long?> GetId(InstrumentSpec instrumentSpec) {
        return _dbContext.InstrumentSpecs
            .Where(x => x.Symbol == instrumentSpec.Symbol &&
                        x.Mic == instrumentSpec.Mic &&
                        x.DataProvider == instrumentSpec.DataProviderKind.ToString())
            .FirstOrDefault()
            ?.Id;
    }

    public async Task<long> GetOrCreateId(InstrumentSpec instrumentSpec) {
        long? existingId = await GetId(instrumentSpec);
        return existingId is { } id
            ? id
            : await Save(instrumentSpec);
    }

    public async Task<long> Save(InstrumentSpec spec) {
        var entity = new InstrumentSpecEntity {
            Symbol = spec.Symbol,
            Mic = spec.Mic,
            DataProvider = spec.DataProviderKind.ToString()
        };

        _dbContext.InstrumentSpecs.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

}