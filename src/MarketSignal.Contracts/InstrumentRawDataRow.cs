using NodaTime;

public record InstrumentRawDataRow(
    Instant Time,
    float Open,
    float High,
    float Low,
    float Close,
    float Volume
) { }