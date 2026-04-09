using System.ComponentModel.DataAnnotations;

namespace MarketSignal.Infrastructure.Instrument.Spec;

public class InstrumentSpecEntity {
    [Key]
    public long Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Mic { get; set; } = string.Empty;
    public string DataProvider { get; set; } = string.Empty;
}