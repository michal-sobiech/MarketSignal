using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.RawData;

using NodaTime;

namespace MarketSignal.Worker.Instrument.RawData;

public class InstrumentRawDataUpdater(
// IInstrumentRawDataProvider instrumentRawDataProvider
// InstrumentRawDataService instrumentRawDataService
) {

    // private readonly IInstrumentRawDataProvider _dataProvider = instrumentRawDataProvider;
    // private readonly InstrumentRawDataService _service = instrumentRawDataService;

    public Task UpdateInstrumentRawData(InstrumentSpec instrumentSpec) {
        return Task.CompletedTask;
    }
    //     Instant newestEntityTime = _service.
    // }

}