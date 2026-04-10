using System.Text.Json.Nodes;

using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Infrastructure.MarketDb;

namespace MarketSignal.Infrastructure.Indicator;

public class EfcoreIndicatorSpecRepository(
    MarketDbContext dbContext
) : IIndicatorSpecRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public Task<long> GetId(IndicatorSpec indicatorSpec) {
        return indicatorSpec switch {
            SmaSpec spec => GetSmaSpecId(spec.Period),
            _ => throw new ArgumentOutOfRangeException(nameof(indicatorSpec), indicatorSpec, "Invalid indicator spec")
        };
    }

    private async Task<long> GetSmaSpecId(int period) {
        string indicatorKindStr = IndicatorKind.SMA.ToString();

        JsonNode expectedIndicatorArgs = new JsonObject { ["period"] = period };

        return _dbContext.IndicatorSpecs
            .Where(x => x.IndictorName == indicatorKindStr)
            .AsEnumerable()
            .First(x => JsonNode.DeepEquals(expectedIndicatorArgs, JsonNode.Parse(x.IndicatorArgsJson)))
            .Id;
    }

}