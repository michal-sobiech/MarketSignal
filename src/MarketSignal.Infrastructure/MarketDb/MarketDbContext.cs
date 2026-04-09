using MarketSignal.Infrastructure.Indicator;
using MarketSignal.Infrastructure.Instrument.RawData;
using MarketSignal.Infrastructure.Instrument.Spec;

using Microsoft.EntityFrameworkCore;

namespace MarketSignal.Infrastructure.MarketDb;

public class MarketDbContext : DbContext {
    public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }

    public DbSet<InstrumentRawDataRowEntity> InstrumentRawDataRows => Set<InstrumentRawDataRowEntity>();
    public DbSet<IndicatorRowEntity> IndicatorRows => Set<IndicatorRowEntity>();
    public DbSet<InstrumentSpecEntity> InstrumentSpecs => Set<InstrumentSpecEntity>();
    public DbSet<IndicatorSpecEntity> IndicatorSpecs => Set<IndicatorSpecEntity>();
}