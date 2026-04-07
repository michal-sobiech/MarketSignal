using MarkeSignal.Infrastructure.Job;

using NodaTime;

using StackExchange.Redis;

namespace MarketSignal.Worker;

public class RedisJobQueueConsumer(
    IConnectionMultiplexer connectionMultiplexer,
    Duration period,
    string queueKey
) : IJobQueueConsumer {

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly Duration _period = period;
    private readonly string _queueKey = queueKey;

    public async Task<Guid> DequeueJob() {
        while (true) {
            RedisValue value = await _database.ListRightPopAsync(_queueKey);
            if (!value.IsNullOrEmpty) {
                return Guid.Parse(value!);
            }

            int periodMs = checked((int)_period.ToTimeSpan().TotalMilliseconds);
            await Task.Delay(periodMs);
        }
    }

}