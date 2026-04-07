namespace MarketSignal.Application.Sma;

public class SmaCalculator {
    public static IEnumerable<double> CalculateSma(List<double> values, int windowLength) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(windowLength, nameof(windowLength));

        List<double> output = [];

        for (int windowLastIndex = windowLength - 1; windowLastIndex < values.Count; windowLastIndex++) {
            int windowFirstIndex = windowLastIndex - windowLength + 1;
            double windowAvg = values.Slice(windowFirstIndex, windowLastIndex + 1).Average();
            output.Add(windowAvg);
        }

        return output;
    }
}