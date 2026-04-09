using MarketSignal.Contracts.Job.Queue;

using StackExchange.Redis;

namespace MarketSignal.Infrastructure.Job;

public class RedisJobQueueConsumer(
    IConnectionMultiplexer connectionMultiplexer,
    string queueKey
) : IJobQueueConsumer {

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _queueKey = queueKey;

    public async Task<Guid?> DequeueJob() {
        RedisValue value = await _database.ListRightPopAsync(_queueKey);
        return !value.IsNullOrEmpty ? Guid.Parse(value!) : null;
    }

}