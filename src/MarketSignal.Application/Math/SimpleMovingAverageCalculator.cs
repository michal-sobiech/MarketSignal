namespace MarketSignal.Application.Math;

public class SimpleMovingAverageCalculator {
    public IEnumerable<double> CalculateSimpleMovingAverage(List<double> values, int period) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(period, nameof(period));

        List<double> output = [];

        for (int windowLastIndex = period - 1; windowLastIndex < values.Count; windowLastIndex++) {
            int windowFirstIndex = windowLastIndex - period + 1;
            double windowAvg = values.Slice(windowFirstIndex, windowLastIndex + 1).Average();
            output.Add(windowAvg);
        }

        return output;
    }
}