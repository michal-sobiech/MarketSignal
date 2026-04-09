using System.Globalization;

using MarketSignal.Contracts.RawData;

using NodaTime;
using NodaTime.Text;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public class AVDailyResponseMapper {

    public static IEnumerable<InstrumentRawDataRow> FromDto(AVDailyResponseDto dto) {
        string timezoneString = dto.MetaData.TimeZone;

        return dto.TimeSeriesDaily
        .Select(x => new InstrumentRawDataRow(
            Time: ParseDate(x.Key, timezoneString),
            Open: decimal.Parse(x.Value.Open, CultureInfo.InvariantCulture),
            High: decimal.Parse(x.Value.High, CultureInfo.InvariantCulture),
            Low: decimal.Parse(x.Value.Low, CultureInfo.InvariantCulture),
            Close: decimal.Parse(x.Value.Close, CultureInfo.InvariantCulture),
            Volume: long.Parse(x.Value.Volume, CultureInfo.InvariantCulture)
        ))
        .OrderBy(x => x.Time)
        .ToList();
    }


    private static Instant ParseDate(string dateString, string timezoneString) {
        LocalDate localDate = LocalDatePattern.Iso.Parse(dateString).Value;
        DateTimeZone timezone = DateTimeZoneProviders.Tzdb[timezoneString];
        return localDate.AtStartOfDayInZone(timezone).ToInstant();
    }

}