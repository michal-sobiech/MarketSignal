namespace MarketSignal.Core.Math;

public class SimpleMovingAverageCalculator {
    public IEnumerable<decimal> CalculateSimpleMovingAverage(List<decimal> values, int period) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(period, nameof(period));

        List<decimal> output = [];

        for (int windowLastIndex = period - 1; windowLastIndex < values.Count; windowLastIndex++) {
            int windowFirstIndex = windowLastIndex - period + 1;
            decimal windowAvg = values.Slice(windowFirstIndex, period).Average();
            output.Add(windowAvg);
        }

        return output;
    }
}