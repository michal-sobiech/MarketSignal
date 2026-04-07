using System.Text.Json;

using MarketSignal.Contracts.Job.Store;

using StackExchange.Redis;

namespace MarketSignal.Infrastructure.Job;

public class RedisJobStore(
    IConnectionMultiplexer connectionMultiplexer,
    string queueKey
) : IJobStore {

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _queueKey = queueKey;

    public async Task Save(JobEntity job) {
        string key = $"{_queueKey}:{job.JobId}";
        var serializedJob = JsonSerializer.Serialize(job);
        await _database.StringSetAsync(key, serializedJob);
    }

    public async Task<JobEntity?> Fetch(Guid jobId) {
        string key = $"{_queueKey}:{jobId}";
        RedisValue serializedJob = await _database.StringGetAsync(key);

        return serializedJob.IsNullOrEmpty
            ? null
            : JsonSerializer.Deserialize<JobEntity>(serializedJob!);
    }

}