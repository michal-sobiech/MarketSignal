namespace MarketSignal.Contracts.Indicator.Spec;

public class SmaSpecFactory {

    public static SmaSpec Of(Dictionary<string, string> indicatorArgs) {
        try {
            int period = int.Parse(indicatorArgs[nameof(SmaSpec.Period)]);
            return new SmaSpec(period);
        }
        catch {
            throw new InvalidIndicatorArgsException();
        }
    }

}