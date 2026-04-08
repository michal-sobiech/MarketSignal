namespace MarketSignal.Contracts.Indicator.Spec;

public class IndicatorSpecFactory {

    public static IndicatorSpec Of(
        IndicatorKind indicatorKind,
        Dictionary<string, string> indicatorArgs
    ) => indicatorKind switch {
        IndicatorKind.SMA => SmaSpecFactory.Of(indicatorArgs),
        _ => throw new ArgumentException("Invalid indicator kind")
    };

}