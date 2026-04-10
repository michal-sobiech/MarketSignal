namespace MarketSignal.Contracts.Indicator.Spec;

public class IndicatorSpecFactory {

    // TODO this might be a duplicate
    public static IndicatorSpec Of(
        IndicatorKind indicatorKind,
        Dictionary<string, string> indicatorArgs
    ) => indicatorKind switch {
        IndicatorKind.SMA => SmaSpecFactory.Of(indicatorArgs),
        _ => throw new ArgumentOutOfRangeException(nameof(indicatorKind), indicatorKind, $"Invalid indicator kind: {indicatorKind}")
    };

}