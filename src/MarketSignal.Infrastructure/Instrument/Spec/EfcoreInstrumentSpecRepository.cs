using MarketSignal.Contracts.Instrument;
using MarketSignal.Infrastructure.MarketDb;

namespace MarketSignal.Infrastructure.Instrument.Spec;

public class EfcoreInstrumentSpecRepository(
    MarketDbContext dbContext
) : IInstrumentSpecRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public async Task<long> GetId(InstrumentSpec instrumentSpec) {
        return _dbContext.InstrumentSpecs
            .Where(x => x.Symbol == instrumentSpec.Symbol &&
                        x.Mic == instrumentSpec.Mic &&
                        x.DataProvider == instrumentSpec.DataProviderKind.ToString())
            .First()
            .Id;
    }

}