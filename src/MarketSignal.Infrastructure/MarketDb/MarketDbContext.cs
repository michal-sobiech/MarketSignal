using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Infrastructure.Indicator;
using MarketSignal.Infrastructure.Instrument.RawData;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.MarketDb;

public class MarketDbContext : DbContext {
    public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }

    public DbSet<InstrumentRawDataRowEntity> InstrumentRawDataRows => Set<InstrumentRawDataRowEntity>();
    public DbSet<IndicatorRowEntity> IndicatorRows => Set<IndicatorRowEntity>();
}