using StackExchange.Redis;

namespace MarkeSignal.Infrastructure.Job;

public class RedisJobQueueProducer(
    IConnectionMultiplexer connectionMultiplexer,
    string queueKey
) : IJobQueueProducer {

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _queueKey = queueKey;

    public Task EnqueueJob(Guid jobId) => _database.ListLeftPushAsync(_queueKey, jobId.ToString());
}