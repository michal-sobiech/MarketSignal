using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

using NodaTime;

namespace MarketSignal.Application.RawData;

public class InstrumentRawDataUpdater(
    IInstrumentRawDataProvider instrumentRawDataProvider,
    InstrumentRawDataService instrumentRawDataService
) {

    private readonly IInstrumentRawDataProvider _rawDataProvider = instrumentRawDataProvider;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;

    public async Task UpdateInstrumentDailyRawData(InstrumentSpec instrumentSpec) {
        IEnumerable<InstrumentRawDataRow> newRows = await FetchNewDailyRows(instrumentSpec);
        await _rawDataService.SaveMany(instrumentSpec, newRows);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchNewDailyRows(InstrumentSpec instrumentSpec) {
        Instant? newestSavedRowTime = await _rawDataService.FetchNewestRowTime(instrumentSpec);
        Instant now = SystemClock.Instance.GetCurrentInstant();

        Instant from = newestSavedRowTime ?? Instant.MaxValue;

        return await _rawDataProvider.FetchDailyRawData(instrumentSpec, from, now);
    }

}