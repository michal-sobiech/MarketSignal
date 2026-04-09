using System.ComponentModel.DataAnnotations;

namespace MarketSignal.Infrastructure.Indicator;

public class IndicatorSpecEntity {
    [Key]
    public long Id { get; set; }
    public string IndictorName { get; set; } = string.Empty;
    public string IndicatorArgsJson { get; set; } = string.Empty;
}
