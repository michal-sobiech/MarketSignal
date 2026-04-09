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
            .Select(x => Instant.FromDateTimeOffset(x.Time))
            .MaxAsync();
    }

    public async Task SaveMany(InstrumentIndicatorSpec instrumentIndicatorSpec, IEnumerable<IndicatorRow> rows) {
        long instrumentSpecId = _instrumentSpecRepository.GetId(instrumentIndicatorSpec.InstrumentSpec);
        long indicatorSpecId = _indicatorSpecRepository.GetId(instrumentIndicatorSpec.IndicatorSpec);

        List<IndicatorRowEntity> entities = rows
            .Select(row => new IndicatorRowEntity {
                RowId = 0,
                InstrumentSpecId = instrumentSpecId,
                IndicatorSpecId = indicatorSpecId,
                Time = row.Time.ToDateTimeOffset(),
                Value = row.Value
            })
            .ToList();

        await _dbContext.IndicatorRows.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IndicatorRow>> FetchByTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        long instrumentSpecId = _instrumentSpecRepository.GetId(instrumentIndicatorSpec.InstrumentSpec);
        long indicatorSpecId = _indicatorSpecRepository.GetId(instrumentIndicatorSpec.IndicatorSpec);

        return await _dbContext.IndicatorRows
            .Where(x =>
                x.InstrumentSpecId == instrumentSpecId &&
                x.IndicatorSpecId == indicatorSpecId &&
                x.Time >= fromInclusive.ToDateTimeOffset() &&
                x.Time <= toInclusive.ToDateTimeOffset())
            .OrderBy(x => x.Time)
            .Select(x => x.toDomain())
            .ToListAsync();
    }

}