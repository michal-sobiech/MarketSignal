using MarketSignal.Contracts.Indicator.Spec;

namespace MarketSignal.Core.Indicator;

public class IndicatorSpecNotFoundException : Exception {

    public IndicatorSpecNotFoundException(IndicatorSpec spec) : base($"Indicator spec not found: {spec}") { }

}