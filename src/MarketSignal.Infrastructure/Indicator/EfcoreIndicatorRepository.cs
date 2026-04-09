using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Infrastructure.MarketDb;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.Indicator;

public class EfcoreIndicatorRepository(
    MarketDbContext dbContext,
    IInstrumentSpecRepository instrumentSpecRepository,
    IIndicatorSpecRepository indicatorSpecRepository
) : IIndicatorRepository {

    private readonly MarketDbContext _dbContext = dbContext;
    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;
    private readonly IIndicatorSpecRepository _indicatorSpecRepository = indicatorSpecRepository;

    public async Task<Instant?> FetchNewestRowTime(InstrumentIndicatorSpec instrumentIndicatorSpec) {
        long instrumentSpecId = _instrumentSpecRepository.GetId(instrumentIndicatorSpec.InstrumentSpec);
        long indicatorSpecId = _indicatorSpecRepository.GetId(instrumentIndicatorSpec.IndicatorSpec);

        return await _dbContext.IndicatorRows
            .Where(x => x.InstrumentSpecId == instrumentSpecId && x.IndicatorSpecId == indicatorSpecId)
            .Select(x => x.Time)
            .MaxAsync();
    }

    public async Task SaveMany(InstrumentIndicatorSpec indicatorSpec, IEnumerable<IndicatorRow> rows) {
        long instrumentSpecId = _instrumentSpecRepository.GetId(indicatorSpec.InstrumentSpec);
        long indicatorSpecId = _indicatorSpecRepository.GetId(indicatorSpec.IndicatorSpec);

        List<IndicatorRowEntity> entities = rows
            .Select(row => new IndicatorRowEntity(
                0,
                instrumentSpecId,
                indicatorSpecId,
                row.Time,
                row.Value
            ))
            .ToList();

        await _dbContext.IndicatorRows.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public Task<IEnumerable<IndicatorRowEntity>> FetchByTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        Instant fromInclusive,
        Instant toInclusive);


}