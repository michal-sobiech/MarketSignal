using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Infrastructure.MarketDb;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public class EfcoreInstrumentRawDataRepository(
    MarketDbContext dbContext
) : IInstrumentRawDataRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public async Task<Instant?> FetchNewestRowTime(long instrumentSpecId) {
        DateTimeOffset? time = await _dbContext.InstrumentRawDataRows
            .Where(x => x.InstrumentSpecId == instrumentSpecId)
            .Select(x => (DateTimeOffset?)x.Time)
            .MaxAsync();

        return time is { } timeNotNull
            ? Instant.FromDateTimeOffset(timeNotNull)
            : null;
    }

    public async Task SaveMany(
        long instrumentSpecId,
        IEnumerable<InstrumentRawDataRow> rows
    ) {
        List<InstrumentRawDataRowEntity> entities = rows
            .Select(row => new InstrumentRawDataRowEntity {
                InstrumentSpecId = instrumentSpecId,
                Time = row.Time.ToDateTimeOffset(),
                Open = row.Open,
                High = row.High,
                Low = row.Low,
                Close = row.Close,
                Volume = row.Volume
            })
            .ToList();

        await _dbContext.InstrumentRawDataRows.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<InstrumentRawDataRow>> FetchByTimeRange(
        long instrumentSpecId,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        DateTimeOffset? from = fromInclusive == Instant.MinValue
            ? null
            : fromInclusive.ToDateTimeOffset();

        DateTimeOffset? to = toInclusive == Instant.MinValue
            ? null
            : toInclusive.ToDateTimeOffset();

        return _dbContext.InstrumentRawDataRows
            .Where(x => x.InstrumentSpecId == instrumentSpecId &&
                (from == null || x.Time >= from) &&
                (to == null || x.Time <= to))
            .Select(row => row.ToDomain())
            .ToList();
    }

}