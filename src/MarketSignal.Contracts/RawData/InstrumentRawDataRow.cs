using NodaTime;

namespace MarketSignal.Contracts.RawData;

public record InstrumentRawDataRow(
    Instant Time,
    double Open,
    double High,
    double Low,
    double Close,
    double Volume
) {
    public double GetValue(InstrumentRawDataField field) =>
        field switch {
            InstrumentRawDataField.OPEN => Open,
            InstrumentRawDataField.HIGH => High,
            InstrumentRawDataField.LOW => Low,
            InstrumentRawDataField.CLOSE => Close,
            InstrumentRawDataField.VOLUME => Volume,
            _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
        };
}