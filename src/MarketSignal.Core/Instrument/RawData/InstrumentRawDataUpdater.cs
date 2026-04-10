using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

using NodaTime;

namespace MarketSignal.Core.Instrument.RawData;

public class InstrumentRawDataUpdater(
    IInstrumentRawDataProvider instrumentRawDataProvider,
    InstrumentRawDataService instrumentRawDataService,
    InstrumentSpecService instrumentSpecService
) {

    private readonly IInstrumentRawDataProvider _rawDataProvider = instrumentRawDataProvider;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;
    private readonly InstrumentSpecService _specService = instrumentSpecService;

    public async Task UpdateInstrumentDailyRawData(InstrumentSpec instrumentSpec) {
        IEnumerable<InstrumentRawDataRow> newRows = await FetchNewDailyRows(instrumentSpec);
        await _rawDataService.SaveMany(instrumentSpec, newRows);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchNewDailyRows(InstrumentSpec instrumentSpec) {
        Instant from = Instant.MinValue;
        if (await _specService.Exists(instrumentSpec)) {
            from = await _rawDataService.FetchNewestRowTime(instrumentSpec) ?? Instant.MinValue;
        }

        Instant now = SystemClock.Instance.GetCurrentInstant();

        return await _rawDataProvider.FetchDailyRawData(instrumentSpec, from, now);
    }

}