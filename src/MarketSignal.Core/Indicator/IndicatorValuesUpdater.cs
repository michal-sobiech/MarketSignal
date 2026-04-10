using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Core.Indicator;

public class IndicatorValuesUpdater {

    public Task CalcAndUpdateIndicatorValues(InstrumentIndicatorSpec instrumentIndicatorSpec) {
        return Task.CompletedTask;
    }

}