using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Core.Indicator.Sma;

namespace MarketSignal.Core.Indicator;

public class IndicatorValuesUpdater(
    SmaUpdater smaUpdater
) {

    private readonly SmaUpdater _smaUpdater = smaUpdater;

    public async Task CalcAndUpdateIndicatorValues(InstrumentIndicatorSpec spec) {
        await (spec.IndicatorSpec switch {
            SmaSpec smaSpec => _smaUpdater.UpdateSma(spec.InstrumentSpec, smaSpec),
            _ => throw new ArgumentOutOfRangeException(nameof(spec), spec, $"Invalid indicator spec: {spec}")
        });
    }

}