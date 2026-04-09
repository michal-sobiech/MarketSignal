using NodaTime;

namespace MarketSignal.Contracts.RawData;

public record InstrumentRawDataRow(
    Instant Time,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    decimal Volume
) {
    public decimal GetValue(InstrumentRawDataField field) =>
        field switch {
            InstrumentRawDataField.OPEN => Open,
            InstrumentRawDataField.HIGH => High,
            InstrumentRawDataField.LOW => Low,
            InstrumentRawDataField.CLOSE => Close,
            InstrumentRawDataField.VOLUME => Volume,
            _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
        };
}