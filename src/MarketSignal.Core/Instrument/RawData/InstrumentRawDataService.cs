using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

using NodaTime;

namespace MarketSignal.Core.Instrument.RawData;

public class InstrumentRawDataService(
    IInstrumentSpecRepository instrumentSpecRepository,
    IInstrumentRawDataRepository instrumentRawDataRepository
) {

    private readonly IInstrumentSpecRepository _instrumentSpecRepo = instrumentSpecRepository;
    private readonly IInstrumentRawDataRepository _instrumentRawDataRepo = instrumentRawDataRepository;

    public async Task<Instant?> FetchNewestRowTime(InstrumentSpec instrumentSpec) {
        long instrumentSpecId = await _instrumentSpecRepo.GetId(instrumentSpec)
            ?? throw new InvalidOperationException("Instrument spec not found");

        return await _instrumentRawDataRepo.FetchNewestRowTime(instrumentSpecId);
    }

    public async Task SaveMany(
        InstrumentSpec instrumentSpec,
        IEnumerable<InstrumentRawDataRow> rows
    ) {
        long instrumentSpecId = await _instrumentSpecRepo.GetId(instrumentSpec)
            ?? throw new InvalidOperationException("Instrument spec not found");

        await _instrumentRawDataRepo.SaveMany(instrumentSpecId, rows);
    }

    public async Task<IEnumerable<InstrumentRawDataRow>> FetchByTimeRange(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        long instrumentSpecId = await _instrumentSpecRepo.GetId(instrumentSpec)
            ?? throw new InvalidOperationException("Instrument spec not found");

        return await _instrumentRawDataRepo.FetchByTimeRange(instrumentSpecId, fromInclusive, toInclusive);
    }
}