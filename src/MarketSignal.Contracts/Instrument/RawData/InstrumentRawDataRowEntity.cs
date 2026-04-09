using NodaTime;

namespace MarketSignal.Contracts.RawData;

public record InstrumentRawDataRowEntity(
    long Id,
    Instant Time,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    decimal Volume
) {
    public InstrumentRawDataRow ToDomain() => new(Time, Open, High, Low, Close, Volume);
}