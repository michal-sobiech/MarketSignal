using MarketSignal.Contracts.Instrument.RawData;

namespace MarketSignal.Contracts.Indicator.Spec;

public class SmaSpecFactory {

    public static SmaSpec Of(Dictionary<string, string> indicatorArgs) {
        try {
            var field = Enum.Parse<InstrumentRawDataField>(indicatorArgs[nameof(SmaSpec.Field)]);
            int period = int.Parse(indicatorArgs[nameof(SmaSpec.Period)]);
            return new SmaSpec(field, period);
        }
        catch {
            throw new InvalidIndicatorArgsException();
        }
    }

}