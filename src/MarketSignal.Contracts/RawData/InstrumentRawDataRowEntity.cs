using NodaTime;

namespace MarketSignal.Contracts.RawData;

public record InstrumentRawDataRowEntity(
    long Id,
    Instant Time,
    double Open,
    double High,
    double Low,
    double Close,
    double Volume
) {
    public InstrumentRawDataRow ToDomain() => new(Time, Open, High, Low, Close, Volume);
}