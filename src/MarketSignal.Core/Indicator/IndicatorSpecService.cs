using MarketSignal.Contracts.Indicator.Spec;

namespace MarketSignal.Core.Indicator;

public class IndicatorSpecService(
    IIndicatorSpecRepository indicatorSpecRepository
) {

    private readonly IIndicatorSpecRepository _indicatorSpecRepository = indicatorSpecRepository;

    public Task Save(IndicatorSpec spec) {
        return _indicatorSpecRepository.Save(spec);
    }

}