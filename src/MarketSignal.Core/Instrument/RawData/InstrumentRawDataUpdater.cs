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
        Instant from = await ChooseFromTime(instrumentSpec);
        Console.WriteLine("RRRRRR");
        Console.WriteLine(from.ToDateTimeOffset().ToString());
        Instant now = SystemClock.Instance.GetCurrentInstant();

        return await _rawDataProvider.FetchDailyRawData(instrumentSpec, from, now);
    }

    private async Task<Instant> ChooseFromTime(InstrumentSpec spec) {
        if (await _specService.Exists(spec)) {
            Instant? newestRowTime = await _rawDataService.FetchNewestRowTime(spec);
            if (newestRowTime is { } newestRowTimeDefined) {
                Instant nextDay = newestRowTimeDefined.Plus(Duration.FromDays(1));
                return nextDay;
            }
        }
        return Instant.MinValue;
    }

}