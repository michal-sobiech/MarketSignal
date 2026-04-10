using System.Text.Json.Nodes;

using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Infrastructure.MarketDb;

namespace MarketSignal.Infrastructure.Indicator;

public class EfcoreIndicatorSpecRepository(
    MarketDbContext dbContext
) : IIndicatorSpecRepository {

    private readonly MarketDbContext _dbContext = dbContext;

    public Task<long?> GetId(IndicatorSpec indicatorSpec) {
        return indicatorSpec switch {
            SmaSpec spec => GetSmaSpecId(spec),
            _ => throw new ArgumentOutOfRangeException(nameof(indicatorSpec), indicatorSpec, "Invalid indicator spec")
        };
    }

    public async Task<long> GetOrCreateId(IndicatorSpec indicatorSpec) {
        long? existingId = await GetId(indicatorSpec);

        return existingId is { } id
            ? id
            : await Save(indicatorSpec);
    }

    public Task<long> Save(IndicatorSpec indicatorSpec) {
        return indicatorSpec switch {
            SmaSpec smaSpec => SaveSmaSpec(smaSpec),
            _ => throw new ArgumentOutOfRangeException(nameof(indicatorSpec), indicatorSpec, "Invalid indicator spec")
        };
    }

    private async Task<long?> GetSmaSpecId(SmaSpec spec) {
        string indicatorKindStr = IndicatorKind.SMA.ToString();

        JsonNode expectedIndicatorArgs = new JsonObject { ["period"] = spec.Period, ["field"] = spec.Field.ToString() };

        return _dbContext.IndicatorSpecs
            .Where(x => x.IndictorName == indicatorKindStr)
            .AsEnumerable()
            .FirstOrDefault(x => JsonNode.DeepEquals(expectedIndicatorArgs, JsonNode.Parse(x.IndicatorArgsJson)))
            ?.Id;
    }

    private async Task<long> SaveSmaSpec(SmaSpec spec) {
        JsonNode indicatorArgs = new JsonObject { ["period"] = spec.Period, ["field"] = spec.Field.ToString() };

        var entity = new IndicatorSpecEntity {
            IndictorName = spec.Kind.ToString(),
            IndicatorArgsJson = indicatorArgs.ToJsonString()
        };

        _dbContext.IndicatorSpecs.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

}