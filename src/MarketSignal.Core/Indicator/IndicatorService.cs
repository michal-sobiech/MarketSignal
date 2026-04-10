using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;

using NodaTime;

namespace MarketSignal.Core.Indicator;

public class IndicatorService(
    IInstrumentSpecRepository instrumentSpecRepository,
    IIndicatorSpecRepository indicatorSpecRepository,
    IIndicatorRepository indicatorRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepository = instrumentSpecRepository;
    private readonly IIndicatorSpecRepository _indicatorSpecRepository = indicatorSpecRepository;
    private readonly IIndicatorRepository _repository = indicatorRepository;

    public async Task<Instant?> FetchNewestRowTime(InstrumentIndicatorSpec instrumentIndicatorSpec) {
        var (instrumentSpecId, indicatorSpecId) = await FetchSpecsOrThrow(instrumentIndicatorSpec);
        return await _repository.FetchNewestRowTime(instrumentSpecId, indicatorSpecId);
    }

    public async Task SaveMany(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        IEnumerable<IndicatorRow> rows
    ) {
        long instrumentSpecId = await _instrumentSpecRepository.GetOrCreateId(instrumentIndicatorSpec.InstrumentSpec);
        long indicatorSpecId = await _indicatorSpecRepository.GetOrCreateId(instrumentIndicatorSpec.IndicatorSpec);
        await _repository.SaveMany(instrumentSpecId, indicatorSpecId, rows);
    }

    public async Task<IEnumerable<IndicatorRow>> FetchByTimeRange(
        InstrumentIndicatorSpec instrumentIndicatorSpec,
        Instant from,
        Instant to
    ) {
        var (instrumentSpecId, indicatorSpecId) = await FetchSpecsOrThrow(instrumentIndicatorSpec);
        return await _repository.FetchByTimeRange(instrumentSpecId, indicatorSpecId, from, to);
    }

    private async Task<(long instrumentSpecId, long indicatorSpecId)> FetchSpecsOrThrow(InstrumentIndicatorSpec spec) {
        long instrumentSpecId = await _instrumentSpecRepository.GetId(spec.InstrumentSpec)
            ?? throw new InvalidOperationException($"Instrument spec not found: {spec.InstrumentSpec}");

        long indicatorSpecId = await _indicatorSpecRepository.GetId(spec.IndicatorSpec)
            ?? throw new InvalidOperationException($"Indicator spec not found: {spec.IndicatorSpec}");

        return (instrumentSpecId, indicatorSpecId);
    }

}