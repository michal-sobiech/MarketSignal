using MarketSignal.Contracts.Indicator;

using NodaTime;

namespace MarketSignal.Application;

public class IndicatorService(IIndicatorRepository indicatorRepository) {

    private readonly IIndicatorRepository _repository = indicatorRepository;

    public Task<Instant> FetchNewestRowTime(InstrumentIndicatorSpec instrumentIndicatorSpec) {
        return _repository.FetchNewestRowTime(instrumentIndicatorSpec);
    }

    public Task SaveMany(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        IEnumerable<IndicatorRow> rows
    ) {
        return _repository.SaveMany(instrumentIndicatorSpec, rows);
    }

    public Task<IndicatorRowEntity> FetchByTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        Instant from,
        Instant to
    ) {
        return _repository.FetchByTimeRange();
    }

}