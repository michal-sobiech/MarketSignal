namespace MarketSignal.Infrastructure.Instrument.RawData;

public class AVInstrumentIdMapper {

    private static readonly Dictionary<(string symbol, string mic), string> SymbolAndMicToAlphaVantageId = new() {
        { ("TSCO", "XLON"), "TSCO.LON" },
        { ("TSCDF", "OTCM"), "TSCDF" },
        { ("TCO2", "XFRA"), "TCO2.FRK" } ,
        { ("VOD", "XLON"), "VOD" },
        { ("IDEA", "XBOM"), "IDEA.BSE"}
    };

    public string FromSymbolAndMic(string symbol, string mic) {
        return SymbolAndMicToAlphaVantageId[(symbol, mic)];
    }

}