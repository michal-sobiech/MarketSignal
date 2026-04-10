using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Infrastructure.Instrument.Spec;

using Microsoft.EntityFrameworkCore;

using NodaTime;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public class InstrumentRawDataRowEntity {
    [Key]
    public long Id { get; set; }
    public long InstrumentSpecId { get; set; }
    public DateTimeOffset Time { get; set; }
    [Precision(18, 8)] public decimal Open { get; set; }
    [Precision(18, 8)] public decimal High { get; set; }
    [Precision(18, 8)] public decimal Low { get; set; }
    [Precision(18, 8)] public decimal Close { get; set; }
    [Precision(18, 8)] public decimal Volume { get; set; }

    public InstrumentRawDataRow ToDomain() => new(
        Instant.FromDateTimeOffset(Time),
        Open,
        High,
        Low,
        Close,
        Volume);

}