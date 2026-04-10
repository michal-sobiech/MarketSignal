using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Core.Instrument.Spec;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public static class AVInstrumentIdMapper {

    private static readonly Dictionary<(string symbol, string mic), string> SymbolAndMicToAlphaVantageId = new() {
        { ("TSCO", "XLON"), "TSCO.LON" },
        { ("TSCDF", "OTCM"), "TSCDF" },
        { ("TCO2", "XFRA"), "TCO2.FRK" } ,
        { ("VOD", "XLON"), "VOD" },
        { ("IDEA", "XBOM"), "IDEA.BSE"}
    };

    static AVInstrumentIdMapper() {
        var allAvInstruments = SupportedInstrumentSpecRegistry.InstrumentSpecs
            .Where(x => x.DataProviderKind == InstrumentRawDataProviderKind.ALPHA_VANTAGE)
            .Select(x => (x.Symbol, x.Mic))
            .ToHashSet();

        var definedAvInstruments = SymbolAndMicToAlphaVantageId.Keys.ToHashSet();

        if (!allAvInstruments.SetEquals(definedAvInstruments)) {
            var missing = allAvInstruments.Except(definedAvInstruments).ToList();
            var extra = definedAvInstruments.Except(allAvInstruments).ToList();

            string? missingInstrumentsMessage = missing.Count != 0
                ? $"Missing mappings: {string.Join(", ", missing.Select(x => $"{x.Item1}/{x.Item2}"))}"
                : null;

            string? extraInstrumentsMessage = extra.Count != 0
                ? $"Unused mappings: {string.Join(", ", extra.Select(x => $"{x.Item1}/{x.Item2}"))}"
                : null;

            var message = string.Join("\n", missingInstrumentsMessage, extraInstrumentsMessage);
            throw new InvalidOperationException(message);
        }
    }

    public static string FromSymbolAndMic(string symbol, string mic) {
        return SymbolAndMicToAlphaVantageId[(symbol, mic)];
    }

}