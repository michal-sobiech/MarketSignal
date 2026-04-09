using MarketSignal.Contracts.Job.Queue;

using StackExchange.Redis;

public class RedisJobQueueProducer(
    IConnectionMultiplexer connectionMultiplexer,
    string queueKey
) : IJobQueueProducer {

    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly string _queueKey = queueKey;

    public Task EnqueueJob(Guid jobId) => _database.ListLeftPushAsync(_queueKey, jobId.ToString());
}