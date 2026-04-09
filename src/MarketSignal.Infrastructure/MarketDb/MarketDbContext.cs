using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.RawData;

using Microsoft.EntityFrameworkCore;

namespace MarketSignal.Infrastructure.MarketDb;

public class MarketDbContext : DbContext {
    public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }

    public DbSet<InstrumentRawDataRowEntity> InstrumentRawDataRows => Set<InstrumentRawDataRowEntity>();
    public DbSet<IndicatorRowEntity> IndicatorRows => Set<IndicatorRowEntity>();
}