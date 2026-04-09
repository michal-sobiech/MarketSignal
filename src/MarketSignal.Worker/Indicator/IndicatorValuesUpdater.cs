using MarketSignal.Contracts.Indicator;

namespace MarketSignal.Worker.Indicator;

public class IndicatorValuesUpdater {

    public Task CalcAndUpdateIndicatorValues(InstrumentIndicatorSpec instrumentIndicatorSpec) {
        return Task.CompletedTask;
    }

}