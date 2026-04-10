using MarketSignal.Contracts.Indicator;
using MarketSignal.Infrastructure.MarketDb;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.Indicator;

public class EfcoreIndicatorRepository(
    MarketDbContext dbContext
) : IIndicatorRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public async Task<Instant?> FetchNewestRowTime(long instrumentSpecId, long indicatorSpecId) {
        DateTimeOffset? max = await _dbContext.IndicatorRows
            .Where(x => x.InstrumentSpecId == instrumentSpecId && x.IndicatorSpecId == indicatorSpecId)
            .MaxAsync(x => (DateTimeOffset?)x.Time);

        return max is null
            ? null
            : Instant.FromDateTimeOffset(max.Value);
    }

    public async Task SaveMany(
        long instrumentSpecId,
        long indicatorSpecId,
        IEnumerable<IndicatorRow> rows
    ) {
        List<IndicatorRowEntity> entities = rows
            .Select(row => new IndicatorRowEntity {
                Id = 0,
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
        long instrumentSpecId,
        long indicatorSpecId,
        Instant fromInclusive,
        Instant toInclusive
    ) {
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