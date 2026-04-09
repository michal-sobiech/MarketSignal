using System.Text.Json.Serialization;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public class AVDailyResponseDto {
    [JsonPropertyName("Meta Data")]
    public AlphaVantageMetaDataDto MetaData { get; set; } = null!;

    [JsonPropertyName("Time Series (Daily)")]
    public Dictionary<string, AlphaVantageDailyRowDto> TimeSeriesDaily { get; set; } = new();
}

public class AlphaVantageMetaDataDto {
    [JsonPropertyName("5. Time Zone")]
    public string TimeZone { get; set; } = null!;
}

public class AlphaVantageDailyRowDto {
    [JsonPropertyName("1. open")]
    public string Open { get; set; } = null!;

    [JsonPropertyName("2. high")]
    public string High { get; set; } = null!;

    [JsonPropertyName("3. low")]
    public string Low { get; set; } = null!;

    [JsonPropertyName("4. close")]
    public string Close { get; set; } = null!;

    [JsonPropertyName("5. volume")]
    public string Volume { get; set; } = null!;
}