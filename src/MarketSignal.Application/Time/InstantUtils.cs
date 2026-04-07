using NodaTime;

namespace MarketSignal.Application.Time;

public class InstantUtils {

    public static IEnumerable<Instant> DaysBetweenInstants(Instant fromInclusive, Instant toInclusive) {
        var days = new List<Instant>();

        var current = fromInclusive;
        while (current <= toInclusive) {
            days.Add(current);
            current += Duration.FromDays(1);
        }

        return days;
    }

}