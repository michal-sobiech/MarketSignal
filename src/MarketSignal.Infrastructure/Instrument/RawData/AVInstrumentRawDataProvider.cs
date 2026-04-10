using System.Text.Json;

using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Instrument.RawData;

using NodaTime;

namespace MarketSignal.Infrastructure.Instrument.RawData;

public class AVInstrumentRawDataProvider(
    AVInstrumentRawDataProviderOptions options,
    HttpClient httpClient
) : IInstrumentRawDataProvider {

    private readonly AVInstrumentRawDataProviderOptions _options = options;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<InstrumentRawDataRow>> FetchDailyRawData(
        InstrumentSpec instrumentSpec,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        string alphaVantageInstrumentId = AVInstrumentIdMapper.FromSymbolAndMic(
            instrumentSpec.Symbol,
            instrumentSpec.Mic);

        return await FetchDailyRawDataWithTimeRangeFiltering(
            alphaVantageInstrumentId,
            fromInclusive,
            toInclusive);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchDailyRawDataWithTimeRangeFiltering(
        string alphaVantageInstrumentId,
        Instant fromInclusive,
        Instant toInclusive
    ) {
        var rows = await FetchDailyRawData(alphaVantageInstrumentId);
        return rows.Where(x => x.Time >= fromInclusive && x.Time <= toInclusive);
    }

    private async Task<IEnumerable<InstrumentRawDataRow>> FetchDailyRawData(string instrumentId) {
        Uri uri = CreateUri(instrumentId);

        HttpResponseMessage response = await _httpClient.GetAsync(uri.ToString());
        response.EnsureSuccessStatusCode();

        string bodyJson = await response.Content.ReadAsStringAsync();

        AVDailyResponseDto? body = JsonSerializer.Deserialize<AVDailyResponseDto>(bodyJson);
        return body is null
            ? throw new InvalidOperationException("Invalid format of Alpha Vantage response")
            : AVDailyResponseMapper.FromDto(body);
    }

    // No time range here, because AlphaVantage does not allow
    // server-side filtering by time range.
    private Uri CreateUri(string instrumentId) {
        UriBuilder builder = new(_options.BaseUrl);
        builder.Path = "query";
        builder.Query = $"function=TIME_SERIES_DAILY&symbol={instrumentId}&apikey={_options.ApiKey}";

        return builder.Uri;
    }


}