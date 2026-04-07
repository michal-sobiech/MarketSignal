using MarketSignal.Contracts;
using MarketSignal.Contracts.RawData;

using NodaTime;

namespace MarketSignal.Application.RawData;

public class InstrumentRawDataUpdater(
    IInstrumentRawDataProvider instrumentRawDataProvider,
    InstrumentRawDataService instrumentRawDataService
) {

    private readonly IInstrumentRawDataProvider _rawDataProvider = instrumentRawDataProvider;
    private readonly InstrumentRawDataService _rawDataService = instrumentRawDataService;

    public async Task UpdateInstrumentRawData() {
        IEnumerable<InstrumentRawDataRow> newRows = await FetchNewRows();
        await _rawDataService.SaveMany(newRows);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchNewRows() {
        Instant newestSavedRowTime = await _rawDataService.FetchNewestRowTime();
        Instant now = SystemClock.Instance.GetCurrentInstant();

        return _rawDataProvider.FetchRawData(newestSavedRowTime, now);
    }

}