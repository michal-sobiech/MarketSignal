namespace MarketSignal.Contracts.Indicator.Spec;

public class SmaSpecFactory {

    public static SmaSpec Of(Dictionary<string, string> indicatorArgs) {
        int period = int.Parse(indicatorArgs[nameof(SmaSpec.Period)]);

        return new SmaSpec(period);
    }

}