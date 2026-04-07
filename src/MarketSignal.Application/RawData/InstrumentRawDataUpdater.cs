using MarketSignal.Contracts;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.RawData;

using NodaTime;

namespace MarketSignal.Application.RawData;

public class InstrumentRawDataUpdater(
    IInstrumentRawDataProvider instrumentRawDataProvider,
    InstrumentRawDataService instrumentRawDataService
) {

    private readonly IInstrumentRawDataProvider _rawDataProvider = instrumentRawDataProvider;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;

    public async Task UpdateInstrumentRawData(InstrumentSpec instrumentSpec) {
        IEnumerable<InstrumentRawDataRow> newRows = await FetchNewRows(instrumentSpec);
        await _rawDataService.SaveMany(instrumentSpec, newRows);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchNewRows(InstrumentSpec instrumentSpec) {
        Instant newestSavedRowTime = await _rawDataService.FetchNewestRowTime(instrumentSpec);
        Instant now = SystemClock.Instance.GetCurrentInstant();

        return _rawDataProvider.FetchRawData(newestSavedRowTime, now);
    }

}