using MarketSignal.Contracts.Indicator.Spec;

namespace MarketSignal.Core.Indicator;

public class IndicatorSpecService(
    IIndicatorSpecRepository indicatorSpecRepository
) {

    private readonly IIndicatorSpecRepository _indicatorSpecRepository = indicatorSpecRepository;

    public async Task<bool> Exists(IndicatorSpec spec) {
        return await _indicatorSpecRepository.GetId(spec) is { };
    }

    public Task Save(IndicatorSpec spec) {
        return _indicatorSpecRepository.Save(spec);
    }

}