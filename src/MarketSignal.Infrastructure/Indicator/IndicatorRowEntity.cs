using System.ComponentModel.DataAnnotations;

using MarketSignal.Contracts.Indicator;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.Indicator;

public class IndicatorRowEntity {
    [Key]
    public long Id { get; set; }
    public long InstrumentSpecId { get; set; }
    public long IndicatorSpecId { get; set; }
    public DateTimeOffset Time { get; set; }
    [Precision(18, 8)] public decimal Value { get; set; }

    public IndicatorRow toDomain() => new(Instant.FromDateTimeOffset(Time), Value);
}